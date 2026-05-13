using System.Text;
using QuanLyDonHangNoiBo.Application.Abstractions;
using QuanLyDonHangNoiBo.Application.Dtos;
using QuanLyDonHangNoiBo.Domain;

namespace QuanLyDonHangNoiBo.Application.Services;

public sealed class OmsApplicationService
{
    private readonly IOmsRepository _repository;

    public OmsApplicationService(IOmsRepository repository)
    {
        _repository = repository;
    }

    public LoginResponse Login(LoginRequest request)
    {
        var tenant = _repository.Tenants.FirstOrDefault(item =>
            item.Code.Equals(request.TenantCode, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException("Tenant khong ton tai.");

        if (tenant.Status is TenantStatus.Suspended or TenantStatus.Expired)
        {
            throw new InvalidOperationException("Tenant dang bi khoa hoac het han.");
        }

        var user = _repository.Users.FirstOrDefault(item =>
            item.TenantId == tenant.Id &&
            item.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase) &&
            item.IsActive)
            ?? _repository.Users.First(item => item.TenantId == tenant.Id && item.Role == UserRole.TenantAdmin);

        user.Locale = string.IsNullOrWhiteSpace(request.Locale) ? user.Locale : request.Locale;

        var rawToken = $"{tenant.Code}:{user.Email}:{DateTimeOffset.UtcNow:O}";
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawToken));

        return new LoginResponse(token, ToTenantDto(tenant), ToUserDto(user), GetPermissions(user.Role));
    }

    public IReadOnlyList<TenantDto> GetTenants()
    {
        return _repository.Tenants.Select(ToTenantDto).ToList();
    }

    public IReadOnlyList<UserDto> GetUsers(Guid? tenantId)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        return _repository.Users
            .Where(item => item.TenantId == resolvedTenantId)
            .OrderBy(item => item.Role)
            .ThenBy(item => item.FullName)
            .Select(ToUserDto)
            .ToList();
    }

    public IReadOnlyList<CustomerDto> GetCustomers(Guid? tenantId, string? search)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        var query = _repository.Customers.Where(item => item.TenantId == resolvedTenantId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item =>
                Contains(item.Code, search) ||
                Contains(item.Name, search) ||
                Contains(item.Phone, search) ||
                Contains(item.Province, search));
        }

        return query.OrderBy(item => item.Name).Select(ToCustomerDto).ToList();
    }

    public CustomerDto CreateCustomer(CreateCustomerRequest request)
    {
        var tenantId = ResolveTenantId(request.TenantId);
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidOperationException("Ten khach hang la bat buoc.");
        }

        if (string.IsNullOrWhiteSpace(request.Phone))
        {
            throw new InvalidOperationException("So dien thoai khach hang la bat buoc.");
        }

        var duplicate = _repository.Customers.Any(item =>
            item.TenantId == tenantId &&
            item.Phone.Equals(request.Phone.Trim(), StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            throw new InvalidOperationException("So dien thoai khach hang da ton tai.");
        }

        var customer = new Customer
        {
            TenantId = tenantId,
            Code = $"CUS-{_repository.Customers.Count(item => item.TenantId == tenantId) + 1:0000}",
            Name = request.Name.Trim(),
            Phone = request.Phone.Trim(),
            Email = request.Email.Trim(),
            Address = request.Address.Trim(),
            Province = request.Province.Trim(),
            Segment = string.IsNullOrWhiteSpace(request.Segment) ? "Retail" : request.Segment.Trim(),
            Note = request.Note.Trim()
        };

        _repository.AddCustomer(customer);
        _repository.AddAuditLog(new AuditLog
        {
            TenantId = tenantId,
            EntityName = nameof(Customer),
            EntityId = customer.Id.ToString(),
            Action = "CreateCustomer",
            AfterValue = customer.Code
        });

        return ToCustomerDto(customer);
    }

    public IReadOnlyList<ProductDto> GetProducts(Guid? tenantId, string? search)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        var query = _repository.Products.Where(item => item.TenantId == resolvedTenantId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item =>
                Contains(item.Code, search) ||
                Contains(item.Name, search) ||
                item.Skus.Any(sku => Contains(sku.SkuCode, search) || Contains(sku.Barcode, search)));
        }

        return query.OrderBy(item => item.Name).Select(ToProductDto).ToList();
    }

    public IReadOnlyList<WarehouseDto> GetWarehouses(Guid? tenantId)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        return _repository.Warehouses
            .Where(item => item.TenantId == resolvedTenantId)
            .OrderBy(item => item.Code)
            .Select(ToWarehouseDto)
            .ToList();
    }

    public IReadOnlyList<InventoryStockDto> GetInventoryStocks(Guid? tenantId, Guid? warehouseId, string? search)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        var query = _repository.InventoryStocks.Where(item => item.TenantId == resolvedTenantId);

        if (warehouseId.HasValue)
        {
            query = query.Where(item => item.WarehouseId == warehouseId.Value);
        }

        var result = query.Select(ToInventoryStockDto).ToList();

        if (!string.IsNullOrWhiteSpace(search))
        {
            result = result.Where(item =>
                Contains(item.SkuCode, search) ||
                Contains(item.ProductName, search) ||
                Contains(item.WarehouseName, search)).ToList();
        }

        return result
            .OrderByDescending(item => item.IsLowStock)
            .ThenBy(item => item.ProductName)
            .ToList();
    }

    public PagedResult<OrderSummaryDto> GetOrders(OrderQuery query)
    {
        var tenantId = ResolveTenantId(query.TenantId);
        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 5, 100);
        var orders = _repository.Orders.Where(item => item.TenantId == tenantId);

        if (query.Status.HasValue)
        {
            orders = orders.Where(item => item.Status == query.Status.Value);
        }

        if (query.WarehouseId.HasValue)
        {
            orders = orders.Where(item => item.WarehouseId == query.WarehouseId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            orders = orders.Where(item => MatchOrder(item, query.Search));
        }

        var ordered = orders.OrderByDescending(item => item.CreatedAt).ToList();
        var total = ordered.Count;
        var items = ordered.Skip((page - 1) * pageSize).Take(pageSize).Select(ToOrderSummaryDto).ToList();
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));

        return new PagedResult<OrderSummaryDto>(items, page, pageSize, total, totalPages);
    }

    public OrderDetailDto GetOrder(Guid orderId)
    {
        var order = FindOrder(orderId);
        return ToOrderDetailDto(order);
    }

    public OrderDetailDto CreateOrder(CreateOrderRequest request)
    {
        var tenantId = ResolveTenantId(request.TenantId);
        var customer = _repository.Customers.FirstOrDefault(item => item.Id == request.CustomerId && item.TenantId == tenantId)
            ?? throw new InvalidOperationException("Khach hang khong ton tai.");
        var warehouse = _repository.Warehouses.FirstOrDefault(item => item.Id == request.WarehouseId && item.TenantId == tenantId)
            ?? throw new InvalidOperationException("Kho khong ton tai.");

        if (request.Items.Count == 0)
        {
            throw new InvalidOperationException("Don hang can co it nhat mot SKU.");
        }

        var orderNumber = _repository.Orders.Count(item => item.TenantId == tenantId) + 1;
        var order = new Order
        {
            TenantId = tenantId,
            OrderCode = $"ORD-{DateTimeOffset.UtcNow:yyMMdd}-{orderNumber:0000}",
            CustomerId = customer.Id,
            WarehouseId = warehouse.Id,
            PaymentMethod = request.PaymentMethod,
            Discount = Math.Max(0, request.Discount),
            ShippingFee = Math.Max(0, request.ShippingFee),
            DeliveryAddress = string.IsNullOrWhiteSpace(request.DeliveryAddress) ? customer.Address : request.DeliveryAddress.Trim(),
            InternalNote = request.InternalNote.Trim(),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            SlaDeadline = DateTimeOffset.UtcNow.AddHours(36)
        };

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new InvalidOperationException("So luong SKU phai lon hon 0.");
            }

            var sku = _repository.Skus.FirstOrDefault(skuItem => skuItem.Id == item.SkuId && skuItem.TenantId == tenantId)
                ?? throw new InvalidOperationException("SKU khong ton tai.");
            var product = _repository.Products.First(productItem => productItem.Id == sku.ProductId);

            order.Items.Add(new OrderItem
            {
                OrderId = order.Id,
                SkuId = sku.Id,
                SkuCode = sku.SkuCode,
                ProductName = $"{product.Name} - {sku.Name}",
                Quantity = item.Quantity,
                UnitPrice = sku.Price
            });
        }

        order.CodAmount = request.CodAmount > 0 ? request.CodAmount : order.Total;

        _repository.AddOrder(order);
        _repository.AddOrderStatusHistory(new OrderStatusHistory
        {
            TenantId = tenantId,
            OrderId = order.Id,
            OldStatus = OrderStatus.Draft,
            NewStatus = OrderStatus.Draft,
            Note = "Create order"
        });
        _repository.AddAuditLog(new AuditLog
        {
            TenantId = tenantId,
            EntityName = nameof(Order),
            EntityId = order.Id.ToString(),
            Action = "CreateOrder",
            AfterValue = order.OrderCode
        });
        _repository.AddNotification(new NotificationItem
        {
            TenantId = tenantId,
            Title = "Don hang moi",
            Message = $"{order.OrderCode} vua duoc tao tu kenh noi bo.",
            Severity = NotificationSeverity.Info,
            Link = $"/orders/{order.Id}"
        });

        return ToOrderDetailDto(order);
    }

    public OrderDetailDto ConfirmOrder(Guid orderId, Guid? userId)
    {
        return UpdateOrderStatusInternal(orderId, OrderStatus.Confirmed, "Confirm order and reserve stock", userId, null);
    }

    public OrderDetailDto CancelOrder(Guid orderId, string note, Guid? userId)
    {
        return UpdateOrderStatusInternal(orderId, OrderStatus.Cancelled, string.IsNullOrWhiteSpace(note) ? "Cancel order" : note, userId, null);
    }

    public OrderDetailDto UpdateOrderStatus(Guid orderId, UpdateOrderStatusRequest request)
    {
        return UpdateOrderStatusInternal(orderId, request.Status, request.Note, request.UserId, null);
    }

    public IReadOnlyList<ShipmentDto> GetShipments(Guid? tenantId)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        return _repository.Shipments
            .Where(item => item.TenantId == resolvedTenantId)
            .OrderByDescending(item => item.UpdatedAt)
            .Select(ToShipmentDto)
            .ToList();
    }

    public ShipmentDto AssignShipment(Guid shipmentId, AssignShipmentRequest request)
    {
        var shipment = FindShipment(shipmentId);
        var driver = _repository.Users.FirstOrDefault(item =>
            item.Id == request.DriverId &&
            item.TenantId == shipment.TenantId &&
            item.Role == UserRole.Driver &&
            item.IsActive)
            ?? throw new InvalidOperationException("Tai xe khong ton tai.");

        shipment.DriverId = driver.Id;
        shipment.RouteName = request.RouteName;
        shipment.Status = ShipmentStatus.Assigned;
        shipment.UpdatedAt = DateTimeOffset.UtcNow;

        _repository.AddAuditLog(new AuditLog
        {
            TenantId = shipment.TenantId,
            EntityName = nameof(Shipment),
            EntityId = shipment.Id.ToString(),
            Action = "AssignDriver",
            AfterValue = driver.FullName
        });

        return ToShipmentDto(shipment);
    }

    public ShipmentDto UpdateShipmentStatus(Guid shipmentId, UpdateShipmentStatusRequest request)
    {
        var shipment = FindShipment(shipmentId);
        shipment.Status = request.Status;
        shipment.FailureReason = request.Status == ShipmentStatus.Failed ? request.Note : shipment.FailureReason;
        shipment.UpdatedAt = DateTimeOffset.UtcNow;

        var mappedStatus = request.Status switch
        {
            ShipmentStatus.InTransit => OrderStatus.InTransit,
            ShipmentStatus.Delivered => OrderStatus.Delivered,
            ShipmentStatus.Failed => OrderStatus.Failed,
            ShipmentStatus.Returned => OrderStatus.Returned,
            _ => (OrderStatus?)null
        };

        if (mappedStatus.HasValue)
        {
            UpdateOrderStatusInternal(shipment.OrderId, mappedStatus.Value, request.Note, shipment.DriverId, request.CollectedAmount);
        }

        return ToShipmentDto(shipment);
    }

    public IReadOnlyList<CodCollectionDto> GetCodCollections(Guid? tenantId)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        return _repository.CodCollections
            .Where(item => item.TenantId == resolvedTenantId)
            .OrderByDescending(item => item.CollectedAt ?? DateTimeOffset.MinValue)
            .Select(ToCodDto)
            .ToList();
    }

    public IReadOnlyList<CodCollectionDto> ReconcileCodCollections(ReconcileCodRequest request)
    {
        var updated = new List<CodCollectionDto>();

        foreach (var id in request.CodCollectionIds.Distinct())
        {
            var collection = _repository.CodCollections.FirstOrDefault(item => item.Id == id)
                ?? throw new InvalidOperationException("Phieu COD khong ton tai.");

            collection.CollectedAmount = collection.CollectedAmount <= 0 ? collection.ExpectedAmount : collection.CollectedAmount;
            collection.Status = collection.CollectedAmount == collection.ExpectedAmount ? CodStatus.Reconciled : CodStatus.Mismatch;
            collection.ReconciledAt = DateTimeOffset.UtcNow;

            _repository.AddAuditLog(new AuditLog
            {
                TenantId = collection.TenantId,
                UserId = request.UserId,
                EntityName = nameof(CodCollection),
                EntityId = collection.Id.ToString(),
                Action = "ReconcileCOD",
                AfterValue = collection.Status.ToString()
            });

            updated.Add(ToCodDto(collection));
        }

        return updated;
    }

    public IReadOnlyList<NotificationDto> GetNotifications(Guid? tenantId)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        return _repository.Notifications
            .Where(item => item.TenantId == resolvedTenantId)
            .OrderByDescending(item => item.CreatedAt)
            .Take(20)
            .Select(ToNotificationDto)
            .ToList();
    }

    public DashboardDto GetDashboard(Guid? tenantId)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        var now = DateTimeOffset.UtcNow;
        var orders = _repository.Orders.Where(item => item.TenantId == resolvedTenantId).ToList();
        var shipments = _repository.Shipments.Where(item => item.TenantId == resolvedTenantId).ToList();
        var cods = _repository.CodCollections.Where(item => item.TenantId == resolvedTenantId).ToList();
        var stocks = GetInventoryStocks(resolvedTenantId, null, null);
        var today = now.UtcDateTime.Date;
        var delivered = orders.Where(item => item.Status == OrderStatus.Delivered).ToList();
        var lateOrders = orders.Where(item =>
            item.SlaDeadline.HasValue &&
            item.SlaDeadline.Value < now &&
            item.Status is not (OrderStatus.Delivered or OrderStatus.Cancelled or OrderStatus.Returned)).ToList();

        var kpis = new List<KpiDto>
        {
            new("ordersToday", "Don moi hom nay", orders.Count(item => item.CreatedAt.UtcDateTime.Date == today).ToString("N0"), "info", 12),
            new("processing", "Don dang xu ly", orders.Count(item => item.Status is OrderStatus.Confirmed or OrderStatus.WaitingPick or OrderStatus.Picking or OrderStatus.Packed or OrderStatus.ReadyToShip).ToString("N0"), "warning", 4),
            new("deliveryRate", "Ty le giao thanh cong", FormatPercent(orders.Count == 0 ? 0 : delivered.Count / (decimal)orders.Count), "success", 3),
            new("revenue", "Doanh thu da giao", FormatMoney(delivered.Sum(item => item.Total)), "success", 18),
            new("codPending", "COD cho doi soat", FormatMoney(cods.Where(item => item.Status is CodStatus.Pending or CodStatus.Collected).Sum(item => item.ExpectedAmount)), "critical", -8),
            new("lowStock", "SKU sap het hang", stocks.Count(item => item.IsLowStock).ToString("N0"), "critical", -2)
        };

        var statusBreakdown = Enum.GetValues<OrderStatus>()
            .Select(status => new StatusMetricDto(status.ToString(), orders.Count(item => item.Status == status)))
            .Where(item => item.Count > 0)
            .ToList();

        var alerts = new List<OperationalAlertDto>();
        alerts.AddRange(lateOrders.Take(3).Select(item => new OperationalAlertDto(
            "Don co nguy co tre SLA",
            $"{item.OrderCode} da qua han xu ly, can dieu phoi lai.",
            NotificationSeverity.Critical,
            $"/orders/{item.Id}")));
        alerts.AddRange(stocks.Where(item => item.IsLowStock).Take(3).Select(item => new OperationalAlertDto(
            "Canh bao ton kho thap",
            $"{item.SkuCode} tai {item.WarehouseCode} chi con {item.Available} san pham kha dung.",
            NotificationSeverity.Warning,
            "/inventory")));

        if (shipments.Any(item => item.Status == ShipmentStatus.Failed))
        {
            alerts.Add(new OperationalAlertDto(
                "Co don giao that bai",
                $"{shipments.Count(item => item.Status == ShipmentStatus.Failed)} shipment can xu ly giao lai hoac hoan hang.",
                NotificationSeverity.Warning,
                "/delivery"));
        }

        var pipelineStatuses = new[] { OrderStatus.Confirmed, OrderStatus.Picking, OrderStatus.Packed, OrderStatus.InTransit };
        var pipeline = pipelineStatuses
            .Select(status => new PipelineColumnDto(
                status.ToString(),
                orders.Count(item => item.Status == status),
                orders.Where(item => item.Status == status).OrderByDescending(item => item.CreatedAt).Take(4).Select(ToOrderSummaryDto).ToList()))
            .ToList();

        return new DashboardDto(
            kpis,
            statusBreakdown,
            orders.OrderByDescending(item => item.CreatedAt).Take(8).Select(ToOrderSummaryDto).ToList(),
            alerts,
            _repository.AiInsights.Where(item => item.TenantId == resolvedTenantId).OrderByDescending(item => item.CreatedAt).Take(4).Select(ToAiInsightDto).ToList(),
            pipeline);
    }

    public MetadataDto GetMetadata(Guid? tenantId)
    {
        var resolvedTenantId = ResolveTenantId(tenantId);
        var drivers = _repository.Users
            .Where(item => item.TenantId == resolvedTenantId && item.Role == UserRole.Driver)
            .Select(item => new LookupDto(item.Id, item.Email, item.FullName))
            .ToList();

        var skus = _repository.Skus
            .Where(item => item.TenantId == resolvedTenantId)
            .Select(item => new LookupDto(item.Id, item.SkuCode, item.Name))
            .OrderBy(item => item.Code)
            .ToList();

        return new MetadataDto(
            resolvedTenantId,
            _repository.Customers.Where(item => item.TenantId == resolvedTenantId).Select(item => new LookupDto(item.Id, item.Code, item.Name)).OrderBy(item => item.Name).ToList(),
            _repository.Warehouses.Where(item => item.TenantId == resolvedTenantId).Select(item => new LookupDto(item.Id, item.Code, item.Name)).OrderBy(item => item.Code).ToList(),
            skus,
            drivers,
            Enum.GetNames<OrderStatus>(),
            Enum.GetNames<ShipmentStatus>());
    }

    public AiChatResponse AskAi(AiChatRequest request)
    {
        var tenantId = ResolveTenantId(request.TenantId);
        var question = request.Question.Trim();
        if (string.IsNullOrWhiteSpace(question))
        {
            throw new InvalidOperationException("Cau hoi khong duoc de trong.");
        }

        var normalized = question.ToLowerInvariant();
        var orders = _repository.Orders.Where(item => item.TenantId == tenantId).ToList();
        var cods = _repository.CodCollections.Where(item => item.TenantId == tenantId).ToList();
        var lowStocks = GetInventoryStocks(tenantId, null, null).Where(item => item.IsLowStock).ToList();
        var links = new List<RelatedLinkDto>();
        string answer;

        if (normalized.Contains("cod") || normalized.Contains("doi soat") || normalized.Contains("thu ho"))
        {
            var pending = cods.Where(item => item.Status is CodStatus.Pending or CodStatus.Collected).ToList();
            answer = $"Hien co {pending.Count} phieu COD can doi soat, tong gia tri {FormatMoney(pending.Sum(item => item.ExpectedAmount))}. Uu tien xu ly cac phieu da thu tien nhung chua reconcile.";
            links.Add(new RelatedLinkDto("Mo man hinh COD", "/finance"));
        }
        else if (normalized.Contains("ton kho") || normalized.Contains("sap het") || normalized.Contains("kho"))
        {
            answer = lowStocks.Count == 0
                ? "Chua co SKU nao duoi nguong canh bao. Nen tiep tuc theo doi hang ban nhanh trong 7 ngay gan nhat."
                : $"Co {lowStocks.Count} SKU sap het hang. SKU can uu tien: {string.Join(", ", lowStocks.Take(3).Select(item => $"{item.SkuCode} ({item.Available})"))}.";
            links.Add(new RelatedLinkDto("Mo ton kho", "/inventory"));
        }
        else if (normalized.Contains("tre") || normalized.Contains("trễ") || normalized.Contains("that bai") || normalized.Contains("thất bại"))
        {
            var risky = orders.Where(item =>
                item.Status is OrderStatus.InTransit or OrderStatus.Failed ||
                item.SlaDeadline < DateTimeOffset.UtcNow).ToList();
            answer = $"Tim thay {risky.Count} don co rui ro giao tre/that bai. Nen loc theo trang thai InTransit va Failed de dieu phoi lai.";
            links.Add(new RelatedLinkDto("Mo dieu phoi giao hang", "/delivery"));
            links.Add(new RelatedLinkDto("Mo danh sach don", "/orders"));
        }
        else
        {
            var delivered = orders.Count(item => item.Status == OrderStatus.Delivered);
            answer = $"Tong quan hom nay: he thong co {orders.Count} don, {delivered} don da giao thanh cong, {lowStocks.Count} SKU sap het hang va {cods.Count(item => item.Status != CodStatus.Reconciled)} phieu COD chua hoan tat.";
            links.Add(new RelatedLinkDto("Mo dashboard", "/dashboard"));
        }

        var suggested = new[]
        {
            "Hom nay co bao nhieu don giao tre?",
            "Kho nao sap het hang?",
            "COD nao chua doi soat?",
            "Don nao co nguy co giao that bai?"
        };

        return new AiChatResponse(answer, links, suggested);
    }

    private OrderDetailDto UpdateOrderStatusInternal(Guid orderId, OrderStatus targetStatus, string note, Guid? userId, decimal? collectedAmount)
    {
        var order = FindOrder(orderId);
        var oldStatus = order.Status;

        if (oldStatus == targetStatus)
        {
            return ToOrderDetailDto(order);
        }

        if (targetStatus is OrderStatus.Confirmed or OrderStatus.WaitingPick or OrderStatus.Picking or OrderStatus.Packed or OrderStatus.ReadyToShip or OrderStatus.InTransit)
        {
            EnsureReserved(order, userId);
        }

        if (targetStatus is OrderStatus.ReadyToShip or OrderStatus.InTransit or OrderStatus.Delivered)
        {
            EnsureStockOut(order, userId);
            EnsureShipment(order);
        }

        if (targetStatus is OrderStatus.Cancelled or OrderStatus.Returned)
        {
            ReleaseReservation(order, userId);
        }

        order.Status = targetStatus;
        order.UpdatedAt = DateTimeOffset.UtcNow;

        if (targetStatus == OrderStatus.Delivered)
        {
            EnsureCodCollection(order, collectedAmount);
        }

        var shipment = _repository.Shipments.FirstOrDefault(item => item.OrderId == order.Id);
        if (shipment is not null)
        {
            shipment.Status = targetStatus switch
            {
                OrderStatus.InTransit => ShipmentStatus.InTransit,
                OrderStatus.Delivered => ShipmentStatus.Delivered,
                OrderStatus.Failed => ShipmentStatus.Failed,
                OrderStatus.Returned => ShipmentStatus.Returned,
                _ => shipment.Status
            };
            shipment.UpdatedAt = DateTimeOffset.UtcNow;
        }

        _repository.AddOrderStatusHistory(new OrderStatusHistory
        {
            TenantId = order.TenantId,
            OrderId = order.Id,
            OldStatus = oldStatus,
            NewStatus = targetStatus,
            ChangedByUserId = userId,
            Note = note,
            ChangedAt = DateTimeOffset.UtcNow
        });
        _repository.AddAuditLog(new AuditLog
        {
            TenantId = order.TenantId,
            UserId = userId,
            EntityName = nameof(Order),
            EntityId = order.Id.ToString(),
            Action = "ChangeOrderStatus",
            BeforeValue = oldStatus.ToString(),
            AfterValue = targetStatus.ToString()
        });
        _repository.AddNotification(new NotificationItem
        {
            TenantId = order.TenantId,
            Title = "Cap nhat trang thai don",
            Message = $"{order.OrderCode}: {oldStatus} -> {targetStatus}.",
            Severity = targetStatus is OrderStatus.Failed or OrderStatus.Cancelled ? NotificationSeverity.Warning : NotificationSeverity.Info,
            Link = $"/orders/{order.Id}"
        });

        return ToOrderDetailDto(order);
    }

    private void EnsureReserved(Order order, Guid? userId)
    {
        var alreadyReserved = _repository.InventoryTransactions.Any(item =>
            item.TenantId == order.TenantId &&
            item.ReferenceCode == order.OrderCode &&
            item.Type == InventoryTransactionType.Reserve);

        if (alreadyReserved)
        {
            return;
        }

        foreach (var item in order.Items)
        {
            var stock = FindStock(order.TenantId, order.WarehouseId, item.SkuId);
            if (stock.Available < item.Quantity)
            {
                throw new InvalidOperationException($"SKU {item.SkuCode} khong du ton kha dung.");
            }

            stock.Reserved += item.Quantity;
            stock.UpdatedAt = DateTimeOffset.UtcNow;
            _repository.AddInventoryTransaction(new InventoryTransaction
            {
                TenantId = order.TenantId,
                WarehouseId = order.WarehouseId,
                SkuId = item.SkuId,
                Type = InventoryTransactionType.Reserve,
                Quantity = item.Quantity,
                ReferenceCode = order.OrderCode,
                Note = $"Reserved by {userId}"
            });
        }
    }

    private void EnsureStockOut(Order order, Guid? userId)
    {
        var alreadyStockOut = _repository.InventoryTransactions.Any(item =>
            item.TenantId == order.TenantId &&
            item.ReferenceCode == order.OrderCode &&
            item.Type == InventoryTransactionType.StockOut);

        if (alreadyStockOut)
        {
            return;
        }

        foreach (var item in order.Items)
        {
            var stock = FindStock(order.TenantId, order.WarehouseId, item.SkuId);
            if (stock.Reserved >= item.Quantity)
            {
                stock.Reserved -= item.Quantity;
            }
            else if (stock.Available < item.Quantity)
            {
                throw new InvalidOperationException($"SKU {item.SkuCode} khong du ton de xuat kho.");
            }

            stock.OnHand -= item.Quantity;
            stock.UpdatedAt = DateTimeOffset.UtcNow;
            _repository.AddInventoryTransaction(new InventoryTransaction
            {
                TenantId = order.TenantId,
                WarehouseId = order.WarehouseId,
                SkuId = item.SkuId,
                Type = InventoryTransactionType.StockOut,
                Quantity = item.Quantity,
                ReferenceCode = order.OrderCode,
                Note = $"Stock out by {userId}"
            });
        }
    }

    private void ReleaseReservation(Order order, Guid? userId)
    {
        var stockOut = _repository.InventoryTransactions.Any(item =>
            item.TenantId == order.TenantId &&
            item.ReferenceCode == order.OrderCode &&
            item.Type == InventoryTransactionType.StockOut);

        if (stockOut)
        {
            return;
        }

        var hasReservation = _repository.InventoryTransactions.Any(item =>
            item.TenantId == order.TenantId &&
            item.ReferenceCode == order.OrderCode &&
            item.Type == InventoryTransactionType.Reserve);

        if (!hasReservation)
        {
            return;
        }

        foreach (var item in order.Items)
        {
            var stock = FindStock(order.TenantId, order.WarehouseId, item.SkuId);
            stock.Reserved = Math.Max(0, stock.Reserved - item.Quantity);
            stock.UpdatedAt = DateTimeOffset.UtcNow;
            _repository.AddInventoryTransaction(new InventoryTransaction
            {
                TenantId = order.TenantId,
                WarehouseId = order.WarehouseId,
                SkuId = item.SkuId,
                Type = InventoryTransactionType.Release,
                Quantity = item.Quantity,
                ReferenceCode = order.OrderCode,
                Note = $"Release by {userId}"
            });
        }
    }

    private void EnsureShipment(Order order)
    {
        if (_repository.Shipments.Any(item => item.OrderId == order.Id))
        {
            return;
        }

        _repository.AddShipment(new Shipment
        {
            TenantId = order.TenantId,
            ShipmentCode = $"SHP-{DateTimeOffset.UtcNow:yyMMdd}-{_repository.Shipments.Count(item => item.TenantId == order.TenantId) + 1:0000}",
            OrderId = order.Id,
            Status = ShipmentStatus.Pending,
            RouteName = "Chua gan tuyen",
            UpdatedAt = DateTimeOffset.UtcNow
        });
    }

    private void EnsureCodCollection(Order order, decimal? collectedAmount)
    {
        if (order.PaymentMethod != PaymentMethod.Cod || _repository.CodCollections.Any(item => item.OrderId == order.Id))
        {
            return;
        }

        _repository.AddCodCollection(new CodCollection
        {
            TenantId = order.TenantId,
            OrderId = order.Id,
            DriverId = _repository.Shipments.FirstOrDefault(item => item.OrderId == order.Id)?.DriverId,
            ExpectedAmount = order.CodAmount,
            CollectedAmount = collectedAmount ?? order.CodAmount,
            Status = CodStatus.Collected,
            CollectedAt = DateTimeOffset.UtcNow
        });
    }

    private Guid ResolveTenantId(Guid? tenantId)
    {
        if (tenantId.HasValue && _repository.Tenants.Any(item => item.Id == tenantId.Value))
        {
            return tenantId.Value;
        }

        var tenant = _repository.Tenants.FirstOrDefault(item => item.Status is TenantStatus.Active or TenantStatus.Trial)
            ?? throw new InvalidOperationException("Chua co tenant kha dung.");
        return tenant.Id;
    }

    private Order FindOrder(Guid orderId)
    {
        return _repository.Orders.FirstOrDefault(item => item.Id == orderId)
            ?? throw new KeyNotFoundException("Don hang khong ton tai.");
    }

    private Shipment FindShipment(Guid shipmentId)
    {
        return _repository.Shipments.FirstOrDefault(item => item.Id == shipmentId)
            ?? throw new KeyNotFoundException("Shipment khong ton tai.");
    }

    private InventoryStock FindStock(Guid tenantId, Guid warehouseId, Guid skuId)
    {
        return _repository.InventoryStocks.FirstOrDefault(item =>
            item.TenantId == tenantId &&
            item.WarehouseId == warehouseId &&
            item.SkuId == skuId)
            ?? throw new InvalidOperationException("Khong tim thay ton kho cho SKU.");
    }

    private bool MatchOrder(Order order, string search)
    {
        var customer = _repository.Customers.FirstOrDefault(item => item.Id == order.CustomerId);
        return Contains(order.OrderCode, search) ||
               order.Items.Any(item => Contains(item.SkuCode, search) || Contains(item.ProductName, search)) ||
               (customer is not null && (Contains(customer.Name, search) || Contains(customer.Phone, search)));
    }

    private static bool Contains(string source, string search)
    {
        return source.Contains(search, StringComparison.OrdinalIgnoreCase);
    }

    private static string FormatMoney(decimal value)
    {
        return $"{value:N0} d";
    }

    private static string FormatPercent(decimal value)
    {
        return $"{value:P0}";
    }

    private static IReadOnlyList<string> GetPermissions(UserRole role)
    {
        var all = new[]
        {
            "dashboard.read",
            "orders.read",
            "orders.write",
            "inventory.read",
            "inventory.write",
            "delivery.read",
            "delivery.write",
            "cod.read",
            "cod.reconcile",
            "users.manage",
            "tenants.manage",
            "ai.ask"
        };

        return role switch
        {
            UserRole.SuperAdmin => all,
            UserRole.TenantAdmin => all.Where(item => item != "tenants.manage").ToArray(),
            UserRole.Sales => ["dashboard.read", "orders.read", "orders.write", "ai.ask"],
            UserRole.Warehouse => ["orders.read", "inventory.read", "inventory.write", "delivery.read"],
            UserRole.InventoryManager => ["dashboard.read", "inventory.read", "inventory.write", "ai.ask"],
            UserRole.Dispatcher => ["dashboard.read", "orders.read", "delivery.read", "delivery.write", "ai.ask"],
            UserRole.Driver => ["delivery.read", "delivery.write"],
            UserRole.Accountant => ["dashboard.read", "cod.read", "cod.reconcile", "ai.ask"],
            UserRole.Manager => ["dashboard.read", "orders.read", "inventory.read", "delivery.read", "cod.read", "ai.ask"],
            _ => []
        };
    }

    private TenantDto ToTenantDto(Tenant tenant)
    {
        return new TenantDto(tenant.Id, tenant.Code, tenant.Name, tenant.Plan, tenant.Status, tenant.UserLimit, tenant.OrderLimit, tenant.WarehouseLimit, tenant.DefaultLocale);
    }

    private static UserDto ToUserDto(AppUser user)
    {
        return new UserDto(user.Id, user.TenantId, user.FullName, user.Email, user.Role, user.WarehouseId, user.Locale, user.IsActive);
    }

    private static CustomerDto ToCustomerDto(Customer customer)
    {
        return new CustomerDto(customer.Id, customer.TenantId, customer.Code, customer.Name, customer.Phone, customer.Email, customer.Address, customer.Province, customer.Segment, customer.Note);
    }

    private ProductDto ToProductDto(Product product)
    {
        return new ProductDto(product.Id, product.TenantId, product.Code, product.Name, product.Category, product.IsActive, product.Skus.Select(ToSkuDto).ToList());
    }

    private static ProductSkuDto ToSkuDto(ProductSku sku)
    {
        return new ProductSkuDto(sku.Id, sku.ProductId, sku.SkuCode, sku.Barcode, sku.Name, sku.Unit, sku.Price, sku.LowStockThreshold);
    }

    private static WarehouseDto ToWarehouseDto(Warehouse warehouse)
    {
        return new WarehouseDto(warehouse.Id, warehouse.TenantId, warehouse.Code, warehouse.Name, warehouse.Province, warehouse.Address, warehouse.IsActive);
    }

    private InventoryStockDto ToInventoryStockDto(InventoryStock stock)
    {
        var warehouse = _repository.Warehouses.First(item => item.Id == stock.WarehouseId);
        var sku = _repository.Skus.First(item => item.Id == stock.SkuId);
        var product = _repository.Products.First(item => item.Id == sku.ProductId);

        return new InventoryStockDto(
            stock.Id,
            stock.TenantId,
            stock.WarehouseId,
            warehouse.Code,
            warehouse.Name,
            stock.SkuId,
            sku.SkuCode,
            $"{product.Name} - {sku.Name}",
            stock.OnHand,
            stock.Reserved,
            stock.Available,
            sku.LowStockThreshold,
            stock.Available <= sku.LowStockThreshold,
            stock.UpdatedAt);
    }

    private OrderSummaryDto ToOrderSummaryDto(Order order)
    {
        var customer = _repository.Customers.First(item => item.Id == order.CustomerId);
        var warehouse = _repository.Warehouses.First(item => item.Id == order.WarehouseId);
        return new OrderSummaryDto(order.Id, order.OrderCode, customer.Id, customer.Name, customer.Phone, warehouse.Id, warehouse.Code, order.Status, order.PaymentMethod, order.Total, order.CodAmount, order.CreatedAt, order.SlaDeadline);
    }

    private OrderDetailDto ToOrderDetailDto(Order order)
    {
        var customer = _repository.Customers.First(item => item.Id == order.CustomerId);
        var warehouse = _repository.Warehouses.First(item => item.Id == order.WarehouseId);
        var history = _repository.OrderStatusHistories
            .Where(item => item.OrderId == order.Id)
            .OrderBy(item => item.ChangedAt)
            .Select(item => new OrderHistoryDto(item.OldStatus, item.NewStatus, item.Note, item.ChangedAt))
            .ToList();

        var items = order.Items
            .Select(item => new OrderItemDto(item.Id, item.SkuId, item.SkuCode, item.ProductName, item.Quantity, item.UnitPrice, item.LineTotal))
            .ToList();

        return new OrderDetailDto(order.Id, order.TenantId, order.OrderCode, ToCustomerDto(customer), ToWarehouseDto(warehouse), order.Status, order.PaymentMethod, order.Discount, order.ShippingFee, order.CodAmount, order.Subtotal, order.Total, order.DeliveryAddress, order.InternalNote, order.CreatedAt, order.UpdatedAt, order.SlaDeadline, items, history);
    }

    private ShipmentDto ToShipmentDto(Shipment shipment)
    {
        var order = _repository.Orders.First(item => item.Id == shipment.OrderId);
        var customer = _repository.Customers.First(item => item.Id == order.CustomerId);
        var driver = shipment.DriverId.HasValue ? _repository.Users.FirstOrDefault(item => item.Id == shipment.DriverId.Value) : null;
        return new ShipmentDto(shipment.Id, shipment.ShipmentCode, order.Id, order.OrderCode, customer.Name, customer.Phone, order.DeliveryAddress, shipment.DriverId, driver?.FullName ?? "Chua gan", shipment.Status, shipment.RouteName, shipment.FailureReason, order.CodAmount, shipment.UpdatedAt);
    }

    private CodCollectionDto ToCodDto(CodCollection collection)
    {
        var order = _repository.Orders.First(item => item.Id == collection.OrderId);
        var customer = _repository.Customers.First(item => item.Id == order.CustomerId);
        var driver = collection.DriverId.HasValue ? _repository.Users.FirstOrDefault(item => item.Id == collection.DriverId.Value) : null;
        return new CodCollectionDto(collection.Id, order.Id, order.OrderCode, customer.Name, driver?.FullName ?? "Chua gan", collection.ExpectedAmount, collection.CollectedAmount, collection.Status, collection.CollectedAt, collection.ReconciledAt);
    }

    private static NotificationDto ToNotificationDto(NotificationItem notification)
    {
        return new NotificationDto(notification.Id, notification.Title, notification.Message, notification.Severity, notification.Link, notification.IsRead, notification.CreatedAt);
    }

    private static AiInsightDto ToAiInsightDto(AiInsight insight)
    {
        return new AiInsightDto(insight.Id, insight.Title, insight.Summary, insight.Severity, insight.Link, insight.CreatedAt);
    }
}
