namespace saga_demo.domain;

public class OrderSagaData : ContainSagaData
{
    public Guid OrderId { get; set; }
    public bool PaymentProcessed { get; set; }
    public bool ShipmentPrepared { get; set; }
}