using QuanLyDonHangNoiBo.Domain;

namespace QuanLyDonHangNoiBo.Application.Abstractions;

public interface IOmsRepository
{
    IReadOnlyList<Tenant> Tenants { get; }
    IReadOnlyList<AppUser> Users { get; }
    IReadOnlyList<Customer> Customers { get; }
    IReadOnlyList<Product> Products { get; }
    IReadOnlyList<ProductSku> Skus { get; }
    IReadOnlyList<Warehouse> Warehouses { get; }
    IReadOnlyList<InventoryStock> InventoryStocks { get; }
    IReadOnlyList<InventoryTransaction> InventoryTransactions { get; }
    IReadOnlyList<Order> Orders { get; }
    IReadOnlyList<OrderStatusHistory> OrderStatusHistories { get; }
    IReadOnlyList<Shipment> Shipments { get; }
    IReadOnlyList<CodCollection> CodCollections { get; }
    IReadOnlyList<NotificationItem> Notifications { get; }
    IReadOnlyList<AuditLog> AuditLogs { get; }
    IReadOnlyList<AiInsight> AiInsights { get; }

    AppUser AddUser(AppUser user);
    bool RemoveUser(Guid userId);
    Customer AddCustomer(Customer customer);
    Order AddOrder(Order order);
    Shipment AddShipment(Shipment shipment);
    CodCollection AddCodCollection(CodCollection codCollection);
    NotificationItem AddNotification(NotificationItem notification);
    AuditLog AddAuditLog(AuditLog auditLog);
    OrderStatusHistory AddOrderStatusHistory(OrderStatusHistory history);
    InventoryTransaction AddInventoryTransaction(InventoryTransaction transaction);
}
