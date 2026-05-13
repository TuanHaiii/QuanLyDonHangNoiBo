using QuanLyDonHangNoiBo.Domain;

namespace QuanLyDonHangNoiBo.Application.Dtos;

public sealed record TenantDto(Guid Id, string Code, string Name, string Plan, TenantStatus Status, int UserLimit, int OrderLimit, int WarehouseLimit, string DefaultLocale);

public sealed record UserDto(Guid Id, Guid TenantId, string FullName, string Email, UserRole Role, Guid? WarehouseId, string Locale, bool IsActive);

public sealed record CustomerDto(Guid Id, Guid TenantId, string Code, string Name, string Phone, string Email, string Address, string Province, string Segment, string Note);

public sealed record ProductSkuDto(Guid Id, Guid ProductId, string SkuCode, string Barcode, string Name, string Unit, decimal Price, int LowStockThreshold);

public sealed record ProductDto(Guid Id, Guid TenantId, string Code, string Name, string Category, bool IsActive, IReadOnlyList<ProductSkuDto> Skus);

public sealed record WarehouseDto(Guid Id, Guid TenantId, string Code, string Name, string Province, string Address, bool IsActive);

public sealed record InventoryStockDto(
    Guid Id,
    Guid TenantId,
    Guid WarehouseId,
    string WarehouseCode,
    string WarehouseName,
    Guid SkuId,
    string SkuCode,
    string ProductName,
    int OnHand,
    int Reserved,
    int Available,
    int LowStockThreshold,
    bool IsLowStock,
    DateTimeOffset UpdatedAt);

public sealed record OrderItemDto(Guid Id, Guid SkuId, string SkuCode, string ProductName, int Quantity, decimal UnitPrice, decimal LineTotal);

public sealed record OrderSummaryDto(
    Guid Id,
    string OrderCode,
    Guid CustomerId,
    string CustomerName,
    string CustomerPhone,
    Guid WarehouseId,
    string WarehouseCode,
    OrderStatus Status,
    PaymentMethod PaymentMethod,
    decimal Total,
    decimal CodAmount,
    DateTimeOffset CreatedAt,
    DateTimeOffset? SlaDeadline);

public sealed record OrderHistoryDto(OrderStatus OldStatus, OrderStatus NewStatus, string Note, DateTimeOffset ChangedAt);

public sealed record OrderDetailDto(
    Guid Id,
    Guid TenantId,
    string OrderCode,
    CustomerDto Customer,
    WarehouseDto Warehouse,
    OrderStatus Status,
    PaymentMethod PaymentMethod,
    decimal Discount,
    decimal ShippingFee,
    decimal CodAmount,
    decimal Subtotal,
    decimal Total,
    string DeliveryAddress,
    string InternalNote,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? SlaDeadline,
    IReadOnlyList<OrderItemDto> Items,
    IReadOnlyList<OrderHistoryDto> History);

public sealed record ShipmentDto(
    Guid Id,
    string ShipmentCode,
    Guid OrderId,
    string OrderCode,
    string CustomerName,
    string CustomerPhone,
    string DeliveryAddress,
    Guid? DriverId,
    string DriverName,
    ShipmentStatus Status,
    string RouteName,
    string FailureReason,
    decimal CodAmount,
    DateTimeOffset UpdatedAt);

public sealed record CodCollectionDto(
    Guid Id,
    Guid OrderId,
    string OrderCode,
    string CustomerName,
    string DriverName,
    decimal ExpectedAmount,
    decimal CollectedAmount,
    CodStatus Status,
    DateTimeOffset? CollectedAt,
    DateTimeOffset? ReconciledAt);

public sealed record NotificationDto(Guid Id, string Title, string Message, NotificationSeverity Severity, string Link, bool IsRead, DateTimeOffset CreatedAt);

public sealed record AiInsightDto(Guid Id, string Title, string Summary, NotificationSeverity Severity, string Link, DateTimeOffset CreatedAt);

public sealed record KpiDto(string Key, string Label, string Value, string Tone, decimal Delta);

public sealed record StatusMetricDto(string Status, int Count);

public sealed record OperationalAlertDto(string Title, string Message, NotificationSeverity Severity, string Link);

public sealed record PipelineColumnDto(string Status, int Count, IReadOnlyList<OrderSummaryDto> Orders);

public sealed record DashboardDto(
    IReadOnlyList<KpiDto> Kpis,
    IReadOnlyList<StatusMetricDto> StatusBreakdown,
    IReadOnlyList<OrderSummaryDto> RecentOrders,
    IReadOnlyList<OperationalAlertDto> Alerts,
    IReadOnlyList<AiInsightDto> AiInsights,
    IReadOnlyList<PipelineColumnDto> Pipeline);

public sealed record LookupDto(Guid Id, string Code, string Name);

public sealed record MetadataDto(
    Guid TenantId,
    IReadOnlyList<LookupDto> Customers,
    IReadOnlyList<LookupDto> Warehouses,
    IReadOnlyList<LookupDto> Skus,
    IReadOnlyList<LookupDto> Drivers,
    IReadOnlyList<string> OrderStatuses,
    IReadOnlyList<string> ShipmentStatuses);

public sealed record LoginRequest(string TenantCode, string Email, string Password, string Locale);

public sealed record LoginResponse(string AccessToken, TenantDto Tenant, UserDto User, IReadOnlyList<string> Permissions);

public sealed class CreateCustomerRequest
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public string Address { get; set; } = "";
    public string Province { get; set; } = "";
    public string Segment { get; set; } = "Retail";
    public string Note { get; set; } = "";
}

public sealed class OrderQuery
{
    public Guid? TenantId { get; set; }
    public string? Search { get; set; }
    public OrderStatus? Status { get; set; }
    public Guid? WarehouseId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public sealed class CreateOrderItemRequest
{
    public Guid SkuId { get; set; }
    public int Quantity { get; set; }
}

public sealed class CreateOrderRequest
{
    public Guid? TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid WarehouseId { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cod;
    public decimal Discount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal CodAmount { get; set; }
    public string DeliveryAddress { get; set; } = "";
    public string InternalNote { get; set; } = "";
    public IReadOnlyList<CreateOrderItemRequest> Items { get; set; } = [];
}

public sealed record UpdateOrderStatusRequest(OrderStatus Status, string Note, Guid? UserId);

public sealed class UserActionRequest
{
    public string Note { get; set; } = "";
    public Guid? UserId { get; set; }
}

public sealed record AssignShipmentRequest(Guid DriverId, string RouteName);

public sealed record UpdateShipmentStatusRequest(ShipmentStatus Status, string Note, decimal? CollectedAmount);

public sealed record ReconcileCodRequest(IReadOnlyList<Guid> CodCollectionIds, Guid? UserId);

public sealed record AiChatRequest(Guid? TenantId, string Question, Guid? UserId);

public sealed record RelatedLinkDto(string Label, string Url);

public sealed record AiChatResponse(string Answer, IReadOnlyList<RelatedLinkDto> RelatedLinks, IReadOnlyList<string> SuggestedQuestions);

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalItems, int TotalPages);
