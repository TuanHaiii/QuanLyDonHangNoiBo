using QuanLyDonHangNoiBo.Application.Abstractions;
using QuanLyDonHangNoiBo.Domain;

namespace QuanLyDonHangNoiBo.Infrastructure;

public sealed class InMemoryOmsRepository : IOmsRepository
{
    private readonly object _sync = new();
    private readonly List<Tenant> _tenants = [];
    private readonly List<AppUser> _users = [];
    private readonly List<Customer> _customers = [];
    private readonly List<Product> _products = [];
    private readonly List<ProductSku> _skus = [];
    private readonly List<Warehouse> _warehouses = [];
    private readonly List<InventoryStock> _inventoryStocks = [];
    private readonly List<InventoryTransaction> _inventoryTransactions = [];
    private readonly List<Order> _orders = [];
    private readonly List<OrderStatusHistory> _orderStatusHistories = [];
    private readonly List<Shipment> _shipments = [];
    private readonly List<CodCollection> _codCollections = [];
    private readonly List<NotificationItem> _notifications = [];
    private readonly List<AuditLog> _auditLogs = [];
    private readonly List<AiInsight> _aiInsights = [];

    public InMemoryOmsRepository()
    {
        Seed();
    }

    public IReadOnlyList<Tenant> Tenants => _tenants;
    public IReadOnlyList<AppUser> Users => _users;
    public IReadOnlyList<Customer> Customers => _customers;
    public IReadOnlyList<Product> Products => _products;
    public IReadOnlyList<ProductSku> Skus => _skus;
    public IReadOnlyList<Warehouse> Warehouses => _warehouses;
    public IReadOnlyList<InventoryStock> InventoryStocks => _inventoryStocks;
    public IReadOnlyList<InventoryTransaction> InventoryTransactions => _inventoryTransactions;
    public IReadOnlyList<Order> Orders => _orders;
    public IReadOnlyList<OrderStatusHistory> OrderStatusHistories => _orderStatusHistories;
    public IReadOnlyList<Shipment> Shipments => _shipments;
    public IReadOnlyList<CodCollection> CodCollections => _codCollections;
    public IReadOnlyList<NotificationItem> Notifications => _notifications;
    public IReadOnlyList<AuditLog> AuditLogs => _auditLogs;
    public IReadOnlyList<AiInsight> AiInsights => _aiInsights;

    public AppUser AddUser(AppUser user)
    {
        lock (_sync)
        {
            _users.Add(user);
        }

        return user;
    }

    public bool RemoveUser(Guid userId)
    {
        lock (_sync)
        {
            var user = _users.FirstOrDefault(item => item.Id == userId);
            return user is not null && _users.Remove(user);
        }
    }

    public Customer AddCustomer(Customer customer)
    {
        lock (_sync)
        {
            _customers.Add(customer);
        }

        return customer;
    }

    public Order AddOrder(Order order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }

        return order;
    }

    public Shipment AddShipment(Shipment shipment)
    {
        lock (_sync)
        {
            _shipments.Add(shipment);
        }

        return shipment;
    }

    public CodCollection AddCodCollection(CodCollection codCollection)
    {
        lock (_sync)
        {
            _codCollections.Add(codCollection);
        }

        return codCollection;
    }

    public NotificationItem AddNotification(NotificationItem notification)
    {
        lock (_sync)
        {
            _notifications.Add(notification);
        }

        return notification;
    }

    public AuditLog AddAuditLog(AuditLog auditLog)
    {
        lock (_sync)
        {
            _auditLogs.Add(auditLog);
        }

        return auditLog;
    }

    public OrderStatusHistory AddOrderStatusHistory(OrderStatusHistory history)
    {
        lock (_sync)
        {
            _orderStatusHistories.Add(history);
        }

        return history;
    }

    public InventoryTransaction AddInventoryTransaction(InventoryTransaction transaction)
    {
        lock (_sync)
        {
            _inventoryTransactions.Add(transaction);
        }

        return transaction;
    }

    private void Seed()
    {
        var tenant = new Tenant
        {
            Code = "demo",
            Name = "Cong ty Demo OMS",
            Plan = "Business",
            Status = TenantStatus.Active,
            UserLimit = 80,
            OrderLimit = 25000,
            WarehouseLimit = 8,
            DefaultLocale = "vi",
            CreatedAt = DateTimeOffset.UtcNow.AddMonths(-5)
        };
        _tenants.Add(tenant);

        var hn = new Warehouse { TenantId = tenant.Id, Code = "HN-01", Name = "Kho Ha Noi", Province = "Ha Noi", Address = "Cau Giay, Ha Noi" };
        var hcm = new Warehouse { TenantId = tenant.Id, Code = "HCM-01", Name = "Kho TP.HCM", Province = "TP.HCM", Address = "Quan 7, TP.HCM" };
        var dn = new Warehouse { TenantId = tenant.Id, Code = "DN-01", Name = "Kho Da Nang", Province = "Da Nang", Address = "Hai Chau, Da Nang" };
        _warehouses.AddRange([hn, hcm, dn]);

        _users.AddRange([
            new AppUser { TenantId = tenant.Id, FullName = "Nguyen Minh Anh", Email = "admin@demo.vn", Role = UserRole.TenantAdmin },
            new AppUser { TenantId = tenant.Id, FullName = "Tran Quoc Bao", Email = "manager@demo.vn", Role = UserRole.Manager },
            new AppUser { TenantId = tenant.Id, FullName = "Le Thu Ha", Email = "sales@demo.vn", Role = UserRole.Sales },
            new AppUser { TenantId = tenant.Id, FullName = "Pham Van Khoi", Email = "warehouse@demo.vn", Role = UserRole.Warehouse, WarehouseId = hn.Id },
            new AppUser { TenantId = tenant.Id, FullName = "Do Gia Linh", Email = "inventory@demo.vn", Role = UserRole.InventoryManager },
            new AppUser { TenantId = tenant.Id, FullName = "Hoang Nam", Email = "dispatcher@demo.vn", Role = UserRole.Dispatcher },
            new AppUser { TenantId = tenant.Id, FullName = "Bui Duc Tai", Email = "driver1@demo.vn", Role = UserRole.Driver },
            new AppUser { TenantId = tenant.Id, FullName = "Vo Thanh Son", Email = "driver2@demo.vn", Role = UserRole.Driver },
            new AppUser { TenantId = tenant.Id, FullName = "Nguyen Phuong Chi", Email = "accountant@demo.vn", Role = UserRole.Accountant }
        ]);

        _customers.AddRange([
            new Customer { TenantId = tenant.Id, Code = "CUS-0001", Name = "Cua hang An Phat", Phone = "0901000001", Email = "anphat@example.vn", Address = "12 Lang Ha, Ha Noi", Province = "Ha Noi", Segment = "Wholesale", Note = "Khach uu tien COD" },
            new Customer { TenantId = tenant.Id, Code = "CUS-0002", Name = "Nguyen Van An", Phone = "0901000002", Email = "an.nguyen@example.vn", Address = "45 Le Loi, TP.HCM", Province = "TP.HCM", Segment = "Retail" },
            new Customer { TenantId = tenant.Id, Code = "CUS-0003", Name = "Shop Miu Miu", Phone = "0901000003", Email = "shopmiumiu@example.vn", Address = "88 Nguyen Van Linh, Da Nang", Province = "Da Nang", Segment = "Marketplace" },
            new Customer { TenantId = tenant.Id, Code = "CUS-0004", Name = "Cong ty Bao Minh", Phone = "0901000004", Email = "baominh@example.vn", Address = "21 Nguyen Trai, Ha Noi", Province = "Ha Noi", Segment = "B2B" },
            new Customer { TenantId = tenant.Id, Code = "CUS-0005", Name = "Tran Thi Bich", Phone = "0901000005", Email = "bich.tran@example.vn", Address = "5 Phan Xich Long, TP.HCM", Province = "TP.HCM", Segment = "Retail" }
        ]);

        var coffee = AddProduct(tenant.Id, "PRD-COF", "Ca phe rang xay", "FMCG", [
            ("SKU-COF-250", "8930001002501", "Goi 250g", 68000m, 25),
            ("SKU-COF-500", "8930001005001", "Goi 500g", 125000m, 18)
        ]);
        var tea = AddProduct(tenant.Id, "PRD-TEA", "Tra thao moc", "FMCG", [
            ("SKU-TEA-BOX", "8930002001001", "Hop 20 tui", 89000m, 12)
        ]);
        var bottle = AddProduct(tenant.Id, "PRD-BOT", "Binh giu nhiet", "Accessories", [
            ("SKU-BOT-500", "8930003005001", "500ml bac", 199000m, 8),
            ("SKU-BOT-750", "8930003007501", "750ml den", 249000m, 8)
        ]);
        var snack = AddProduct(tenant.Id, "PRD-SNK", "Hat dinh duong", "FMCG", [
            ("SKU-SNK-CASHEW", "8930004001001", "Hat dieu 300g", 115000m, 15)
        ]);

        foreach (var sku in _skus)
        {
            _inventoryStocks.Add(new InventoryStock { TenantId = tenant.Id, WarehouseId = hn.Id, SkuId = sku.Id, OnHand = StockFor(sku.SkuCode, hn.Code), Reserved = 0 });
            _inventoryStocks.Add(new InventoryStock { TenantId = tenant.Id, WarehouseId = hcm.Id, SkuId = sku.Id, OnHand = StockFor(sku.SkuCode, hcm.Code), Reserved = 0 });
            _inventoryStocks.Add(new InventoryStock { TenantId = tenant.Id, WarehouseId = dn.Id, SkuId = sku.Id, OnHand = StockFor(sku.SkuCode, dn.Code), Reserved = 0 });
        }

        var driver1 = _users.First(item => item.Email == "driver1@demo.vn");
        var driver2 = _users.First(item => item.Email == "driver2@demo.vn");

        AddSeedOrder(tenant.Id, "ORD-260511-0001", _customers[0], hn, OrderStatus.Delivered, PaymentMethod.Cod, DateTimeOffset.UtcNow.AddHours(-7), [
            (coffee.Skus[1], 4),
            (snack.Skus[0], 2)
        ], driver1, ShipmentStatus.Delivered, CodStatus.Collected);

        AddSeedOrder(tenant.Id, "ORD-260511-0002", _customers[1], hcm, OrderStatus.InTransit, PaymentMethod.Cod, DateTimeOffset.UtcNow.AddHours(-5), [
            (bottle.Skus[0], 1),
            (tea.Skus[0], 3)
        ], driver2, ShipmentStatus.InTransit, null);

        AddSeedOrder(tenant.Id, "ORD-260511-0003", _customers[2], dn, OrderStatus.Picking, PaymentMethod.BankTransfer, DateTimeOffset.UtcNow.AddHours(-3), [
            (coffee.Skus[0], 5)
        ], null, null, null);

        AddSeedOrder(tenant.Id, "ORD-260510-0004", _customers[3], hn, OrderStatus.Failed, PaymentMethod.Cod, DateTimeOffset.UtcNow.AddDays(-1).AddHours(-2), [
            (bottle.Skus[1], 2)
        ], driver1, ShipmentStatus.Failed, null);

        AddSeedOrder(tenant.Id, "ORD-260510-0005", _customers[4], hcm, OrderStatus.Packed, PaymentMethod.Cod, DateTimeOffset.UtcNow.AddDays(-1), [
            (tea.Skus[0], 2),
            (snack.Skus[0], 1)
        ], null, null, null);

        AddSeedOrder(tenant.Id, "ORD-260509-0006", _customers[0], hn, OrderStatus.Confirmed, PaymentMethod.Cod, DateTimeOffset.UtcNow.AddDays(-2), [
            (coffee.Skus[0], 10)
        ], null, null, null);

        _notifications.AddRange([
            new NotificationItem { TenantId = tenant.Id, Title = "COD can doi soat", Message = "3 phieu COD da thu tien nhung chua doi soat.", Severity = NotificationSeverity.Warning, Link = "/finance", CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-20) },
            new NotificationItem { TenantId = tenant.Id, Title = "SKU sap het hang", Message = "SKU-BOT-750 tai kho HN-01 duoi nguong canh bao.", Severity = NotificationSeverity.Critical, Link = "/inventory", CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-35) },
            new NotificationItem { TenantId = tenant.Id, Title = "Don giao that bai", Message = "ORD-260510-0004 can len lich giao lai.", Severity = NotificationSeverity.Warning, Link = "/delivery", CreatedAt = DateTimeOffset.UtcNow.AddHours(-1) }
        ]);

        _aiInsights.AddRange([
            new AiInsight { TenantId = tenant.Id, Title = "COD chua doi soat tang", Summary = "Gia tri COD cho doi soat cao hon 18% so voi trung binh 7 ngay.", Severity = NotificationSeverity.Warning, Link = "/finance", CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-15) },
            new AiInsight { TenantId = tenant.Id, Title = "Kho HN sap thieu SKU binh 750ml", Summary = "Toc do ban 3 ngay gan nhat co the lam het hang trong 2 ngay.", Severity = NotificationSeverity.Critical, Link = "/inventory", CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-40) },
            new AiInsight { TenantId = tenant.Id, Title = "Tuyen TP.HCM can toi uu", Summary = "Cac shipment khu Quan 7 dang co ETA cao hon 24 phut so voi trung binh.", Severity = NotificationSeverity.Info, Link = "/delivery", CreatedAt = DateTimeOffset.UtcNow.AddHours(-2) }
        ]);
    }

    private Product AddProduct(Guid tenantId, string code, string name, string category, IReadOnlyList<(string SkuCode, string Barcode, string Name, decimal Price, int Threshold)> skuInputs)
    {
        var product = new Product
        {
            TenantId = tenantId,
            Code = code,
            Name = name,
            Category = category
        };

        foreach (var input in skuInputs)
        {
            var sku = new ProductSku
            {
                TenantId = tenantId,
                ProductId = product.Id,
                SkuCode = input.SkuCode,
                Barcode = input.Barcode,
                Name = input.Name,
                Price = input.Price,
                LowStockThreshold = input.Threshold
            };
            product.Skus.Add(sku);
            _skus.Add(sku);
        }

        _products.Add(product);
        return product;
    }

    private int StockFor(string skuCode, string warehouseCode)
    {
        return (skuCode, warehouseCode) switch
        {
            ("SKU-BOT-750", "HN-01") => 5,
            ("SKU-TEA-BOX", "DN-01") => 7,
            ("SKU-SNK-CASHEW", "HCM-01") => 12,
            ("SKU-COF-250", "HN-01") => 80,
            ("SKU-COF-500", "HCM-01") => 64,
            _ => 32
        };
    }

    private void AddSeedOrder(
        Guid tenantId,
        string orderCode,
        Customer customer,
        Warehouse warehouse,
        OrderStatus status,
        PaymentMethod paymentMethod,
        DateTimeOffset createdAt,
        IReadOnlyList<(ProductSku Sku, int Quantity)> items,
        AppUser? driver,
        ShipmentStatus? shipmentStatus,
        CodStatus? codStatus)
    {
        var order = new Order
        {
            TenantId = tenantId,
            OrderCode = orderCode,
            CustomerId = customer.Id,
            WarehouseId = warehouse.Id,
            Status = status,
            PaymentMethod = paymentMethod,
            ShippingFee = 25000,
            DeliveryAddress = customer.Address,
            InternalNote = "Du lieu mau tu tai lieu OMS",
            CreatedAt = createdAt,
            UpdatedAt = createdAt.AddHours(1),
            SlaDeadline = createdAt.AddHours(status is OrderStatus.Failed ? 12 : 36)
        };

        foreach (var item in items)
        {
            var product = _products.First(productItem => productItem.Id == item.Sku.ProductId);
            order.Items.Add(new OrderItem
            {
                OrderId = order.Id,
                SkuId = item.Sku.Id,
                SkuCode = item.Sku.SkuCode,
                ProductName = $"{product.Name} - {item.Sku.Name}",
                Quantity = item.Quantity,
                UnitPrice = item.Sku.Price
            });
        }

        order.CodAmount = order.Total;
        _orders.Add(order);
        _orderStatusHistories.Add(new OrderStatusHistory
        {
            TenantId = tenantId,
            OrderId = order.Id,
            OldStatus = OrderStatus.Draft,
            NewStatus = status,
            Note = "Seed status",
            ChangedAt = createdAt.AddMinutes(15)
        });

        if (status is OrderStatus.Confirmed or OrderStatus.WaitingPick or OrderStatus.Picking or OrderStatus.Packed)
        {
            foreach (var item in order.Items)
            {
                var stock = _inventoryStocks.First(stockItem => stockItem.WarehouseId == warehouse.Id && stockItem.SkuId == item.SkuId);
                stock.Reserved += item.Quantity;
                _inventoryTransactions.Add(new InventoryTransaction
                {
                    TenantId = tenantId,
                    WarehouseId = warehouse.Id,
                    SkuId = item.SkuId,
                    Type = InventoryTransactionType.Reserve,
                    Quantity = item.Quantity,
                    ReferenceCode = order.OrderCode,
                    Note = "Seed reserve"
                });
            }
        }

        if (status is OrderStatus.ReadyToShip or OrderStatus.InTransit or OrderStatus.Delivered or OrderStatus.Failed)
        {
            foreach (var item in order.Items)
            {
                var stock = _inventoryStocks.First(stockItem => stockItem.WarehouseId == warehouse.Id && stockItem.SkuId == item.SkuId);
                stock.OnHand -= item.Quantity;
                _inventoryTransactions.Add(new InventoryTransaction
                {
                    TenantId = tenantId,
                    WarehouseId = warehouse.Id,
                    SkuId = item.SkuId,
                    Type = InventoryTransactionType.StockOut,
                    Quantity = item.Quantity,
                    ReferenceCode = order.OrderCode,
                    Note = "Seed stock out"
                });
            }
        }

        if (shipmentStatus.HasValue)
        {
            _shipments.Add(new Shipment
            {
                TenantId = tenantId,
                ShipmentCode = $"SHP-{orderCode[^4..]}",
                OrderId = order.Id,
                DriverId = driver?.Id,
                Status = shipmentStatus.Value,
                RouteName = warehouse.Province == "TP.HCM" ? "Tuyen Nam Sai Gon" : "Tuyen noi thanh",
                FailureReason = shipmentStatus == ShipmentStatus.Failed ? "Khach hen giao lai" : "",
                UpdatedAt = createdAt.AddHours(2)
            });
        }

        if (codStatus.HasValue || (status == OrderStatus.Delivered && paymentMethod == PaymentMethod.Cod))
        {
            _codCollections.Add(new CodCollection
            {
                TenantId = tenantId,
                OrderId = order.Id,
                DriverId = driver?.Id,
                ExpectedAmount = order.CodAmount,
                CollectedAmount = codStatus == CodStatus.Mismatch ? order.CodAmount - 50000 : order.CodAmount,
                Status = codStatus ?? CodStatus.Collected,
                CollectedAt = status == OrderStatus.Delivered ? createdAt.AddHours(3) : null,
                ReconciledAt = codStatus == CodStatus.Reconciled ? createdAt.AddHours(5) : null
            });
        }
    }
}
