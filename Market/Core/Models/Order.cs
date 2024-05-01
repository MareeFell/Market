using System.ComponentModel;

namespace Market.Models;

public enum OrderStatus
{
    Created,
    Delivered,
    Accept,
}
public class Order
{
    public int Id { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    
    [DefaultValue(OrderStatus.Delivered)]
    public OrderStatus OrderStatus { get; set; }

    public string StringedOrderItems => string.Join(",", OrderItems.Select(e => $"{e.Nomenclature.Name} {e.Count}"));
}

