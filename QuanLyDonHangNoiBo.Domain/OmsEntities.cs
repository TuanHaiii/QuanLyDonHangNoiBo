namespace QuanLyDonHangNoiBo.Domain;

public sealed class Tenant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Plan { get; set; } = "Starter";
    public TenantStatus Status { get; set; } = TenantStatus.Trial;
    public int UserLimit { get; set; }
    public int OrderLimit { get; set; }
    public int WarehouseLimit { get; set; }
    public string DefaultLocale { get; set; } = "vi";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class AppUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public UserRole Role { get; set; }
    public Guid? WarehouseId { get; set; }
    public string Locale { get; set; } = "vi";
    public bool IsActive { get; set; } = true;
}

public sealed class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public string Address { get; set; } = "";
    public string Province { get; set; } = "";
    public string Segment { get; set; } = "Retail";
    public string Note { get; set; } = "";
}

public sealed class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public List<ProductSku> Skus { get; set; } = [];
}

public sealed class ProductSku
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid ProductId { get; set; }
    public string SkuCode { get; set; } = "";
    public string Barcode { get; set; } = "";
    public string Name { get; set; } = "";
    public string Unit { get; set; } = "pcs";
    public decimal Price { get; set; }
    public int LowStockThreshold { get; set; } = 10;
}

public sealed class Warehouse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Province { get; set; } = "";
    public string Address { get; set; } = "";
    public bool IsActive { get; set; } = true;
}

public sealed class InventoryStock
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid SkuId { get; set; }
    public int OnHand { get; set; }
    public int Reserved { get; set; }
    public int Available => Math.Max(0, OnHand - Reserved);
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class InventoryTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid SkuId { get; set; }
    public InventoryTransactionType Type { get; set; }
    public int Quantity { get; set; }
    public string ReferenceCode { get; set; } = "";
    public string Note { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string OrderCode { get; set; } = "";
    public Guid CustomerId { get; set; }
    public Guid WarehouseId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Draft;
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cod;
    public decimal Discount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal CodAmount { get; set; }
    public string DeliveryAddress { get; set; } = "";
    public string InternalNote { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? SlaDeadline { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public decimal Subtotal => Items.Sum(item => item.LineTotal);
    public decimal Total => Math.Max(0, Subtotal - Discount + ShippingFee);
}

public sealed class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public Guid SkuId { get; set; }
    public string SkuCode { get; set; } = "";
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}

public sealed class OrderStatusHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid OrderId { get; set; }
    public OrderStatus OldStatus { get; set; }
    public OrderStatus NewStatus { get; set; }
    public Guid? ChangedByUserId { get; set; }
    public string Note { get; set; } = "";
    public DateTimeOffset ChangedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class Shipment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string ShipmentCode { get; set; } = "";
    public Guid OrderId { get; set; }
    public Guid? DriverId { get; set; }
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    public string RouteName { get; set; } = "";
    public string FailureReason { get; set; } = "";
    public string ProofOfDeliveryUrl { get; set; } = "";
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class CodCollection
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid OrderId { get; set; }
    public Guid? DriverId { get; set; }
    public decimal ExpectedAmount { get; set; }
    public decimal CollectedAmount { get; set; }
    public CodStatus Status { get; set; } = CodStatus.Pending;
    public DateTimeOffset? CollectedAt { get; set; }
    public DateTimeOffset? ReconciledAt { get; set; }
}

public sealed class NotificationItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid? UserId { get; set; }
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
    public string Link { get; set; } = "";
    public bool IsRead { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid? UserId { get; set; }
    public string EntityName { get; set; } = "";
    public string EntityId { get; set; } = "";
    public string Action { get; set; } = "";
    public string BeforeValue { get; set; } = "";
    public string AfterValue { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class AiInsight
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
    public string Link { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
