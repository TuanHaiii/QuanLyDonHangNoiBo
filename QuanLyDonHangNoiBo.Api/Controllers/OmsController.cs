using Microsoft.AspNetCore.Mvc;
using QuanLyDonHangNoiBo.Application.Dtos;
using QuanLyDonHangNoiBo.Application.Services;

namespace QuanLyDonHangNoiBo.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class OmsController : ControllerBase
{
    private readonly OmsApplicationService _service;

    public OmsController(OmsApplicationService service)
    {
        _service = service;
    }

    [HttpGet("tenants")]
    public ActionResult<IReadOnlyList<TenantDto>> GetTenants()
    {
        return Execute(() => _service.GetTenants());
    }

    [HttpGet("users")]
    public ActionResult<IReadOnlyList<UserDto>> GetUsers([FromQuery] Guid? tenantId)
    {
        return Execute(() => _service.GetUsers(tenantId));
    }

    [HttpGet("customers")]
    public ActionResult<IReadOnlyList<CustomerDto>> GetCustomers([FromQuery] Guid? tenantId, [FromQuery] string? search)
    {
        return Execute(() => _service.GetCustomers(tenantId, search));
    }

    [HttpPost("customers")]
    public ActionResult<CustomerDto> CreateCustomer(CreateCustomerRequest request)
    {
        return Execute(() => _service.CreateCustomer(request));
    }

    [HttpGet("products")]
    public ActionResult<IReadOnlyList<ProductDto>> GetProducts([FromQuery] Guid? tenantId, [FromQuery] string? search)
    {
        return Execute(() => _service.GetProducts(tenantId, search));
    }

    [HttpGet("warehouses")]
    public ActionResult<IReadOnlyList<WarehouseDto>> GetWarehouses([FromQuery] Guid? tenantId)
    {
        return Execute(() => _service.GetWarehouses(tenantId));
    }

    [HttpGet("inventory/stocks")]
    public ActionResult<IReadOnlyList<InventoryStockDto>> GetInventoryStocks([FromQuery] Guid? tenantId, [FromQuery] Guid? warehouseId, [FromQuery] string? search)
    {
        return Execute(() => _service.GetInventoryStocks(tenantId, warehouseId, search));
    }

    [HttpGet("orders")]
    public ActionResult<PagedResult<OrderSummaryDto>> GetOrders([FromQuery] OrderQuery query)
    {
        return Execute(() => _service.GetOrders(query));
    }

    [HttpGet("orders/{orderId:guid}")]
    public ActionResult<OrderDetailDto> GetOrder(Guid orderId)
    {
        return Execute(() => _service.GetOrder(orderId));
    }

    [HttpPost("orders")]
    public ActionResult<OrderDetailDto> CreateOrder(CreateOrderRequest request)
    {
        return Execute(() => _service.CreateOrder(request));
    }

    [HttpPost("orders/{orderId:guid}/confirm")]
    public ActionResult<OrderDetailDto> ConfirmOrder(Guid orderId, UserActionRequest? request)
    {
        return Execute(() => _service.ConfirmOrder(orderId, request?.UserId));
    }

    [HttpPost("orders/{orderId:guid}/cancel")]
    public ActionResult<OrderDetailDto> CancelOrder(Guid orderId, UserActionRequest request)
    {
        return Execute(() => _service.CancelOrder(orderId, request.Note, request.UserId));
    }

    [HttpPost("orders/{orderId:guid}/status")]
    public ActionResult<OrderDetailDto> UpdateOrderStatus(Guid orderId, UpdateOrderStatusRequest request)
    {
        return Execute(() => _service.UpdateOrderStatus(orderId, request));
    }

    [HttpGet("delivery/shipments")]
    public ActionResult<IReadOnlyList<ShipmentDto>> GetShipments([FromQuery] Guid? tenantId)
    {
        return Execute(() => _service.GetShipments(tenantId));
    }

    [HttpPost("delivery/shipments/{shipmentId:guid}/assign")]
    public ActionResult<ShipmentDto> AssignShipment(Guid shipmentId, AssignShipmentRequest request)
    {
        return Execute(() => _service.AssignShipment(shipmentId, request));
    }

    [HttpPost("driver/shipments/{shipmentId:guid}/status")]
    public ActionResult<ShipmentDto> UpdateShipmentStatus(Guid shipmentId, UpdateShipmentStatusRequest request)
    {
        return Execute(() => _service.UpdateShipmentStatus(shipmentId, request));
    }

    [HttpGet("cod/collections")]
    public ActionResult<IReadOnlyList<CodCollectionDto>> GetCodCollections([FromQuery] Guid? tenantId)
    {
        return Execute(() => _service.GetCodCollections(tenantId));
    }

    [HttpPost("cod/reconcile")]
    public ActionResult<IReadOnlyList<CodCollectionDto>> ReconcileCodCollections(ReconcileCodRequest request)
    {
        return Execute(() => _service.ReconcileCodCollections(request));
    }

    [HttpGet("notifications")]
    public ActionResult<IReadOnlyList<NotificationDto>> GetNotifications([FromQuery] Guid? tenantId)
    {
        return Execute(() => _service.GetNotifications(tenantId));
    }

    [HttpGet("dashboard")]
    [HttpGet("reports/dashboard")]
    public ActionResult<DashboardDto> GetDashboard([FromQuery] Guid? tenantId)
    {
        return Execute(() => _service.GetDashboard(tenantId));
    }

    [HttpGet("metadata")]
    public ActionResult<MetadataDto> GetMetadata([FromQuery] Guid? tenantId)
    {
        return Execute(() => _service.GetMetadata(tenantId));
    }

    [HttpPost("ai/chat")]
    public ActionResult<AiChatResponse> AskAi(AiChatRequest request)
    {
        return Execute(() => _service.AskAi(request));
    }

    private ActionResult<T> Execute<T>(Func<T> action)
    {
        try
        {
            return Ok(action());
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
