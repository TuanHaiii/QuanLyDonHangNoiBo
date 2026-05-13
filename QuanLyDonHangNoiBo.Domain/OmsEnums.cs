namespace QuanLyDonHangNoiBo.Domain;

public enum TenantStatus
{
    Trial,
    Active,
    Suspended,
    Expired
}

public enum UserRole
{
    SuperAdmin,
    TenantAdmin,
    Sales,
    Warehouse,
    InventoryManager,
    Dispatcher,
    Driver,
    Accountant,
    Manager
}

public enum OrderStatus
{
    Draft,
    Confirmed,
    WaitingPick,
    Picking,
    Packed,
    ReadyToShip,
    InTransit,
    Delivered,
    Failed,
    Cancelled,
    Returned
}

public enum ShipmentStatus
{
    Pending,
    Assigned,
    Accepted,
    PickingUp,
    InTransit,
    Delivered,
    Failed,
    Returned
}

public enum InventoryTransactionType
{
    StockIn,
    StockOut,
    Reserve,
    Release,
    Adjustment,
    Return
}

public enum PaymentMethod
{
    Cod,
    BankTransfer,
    Cash,
    EWallet
}

public enum CodStatus
{
    Pending,
    Collected,
    Reconciled,
    Mismatch
}

public enum NotificationSeverity
{
    Info,
    Success,
    Warning,
    Critical
}
