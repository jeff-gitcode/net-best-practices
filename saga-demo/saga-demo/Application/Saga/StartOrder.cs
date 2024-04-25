public class StartOrder : ICommand
{
    public Guid OrderId { get; set; }
}
public class ProcessPayment : ICommand
{
    public Guid OrderId { get; set; }
}
public class PrepareShipment : ICommand
{
    public Guid OrderId { get; set; }
}
public class OrderCompleted : IEvent
{
    public Guid OrderId { get; set; }
}