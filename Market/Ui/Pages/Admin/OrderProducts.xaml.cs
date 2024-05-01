using System.Windows;
using System.Windows.Controls;
using Market.Models;
using Market.Service;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Market.Ui.Pages.Admin;

public class MappedOrderItem
{
    public int Count { get; set; }
    public Nomenclature Nomenclature { get; set; }
}
public partial class OrderProducts : Page
{
    public List<MappedOrderItem> OrderItems { get; set; }
    private readonly DatabaseService _databaseService;

    public OrderProducts(DatabaseService databaseService)
    {
        InitializeComponent();
        DataContext = this;
        _databaseService = databaseService;
        UpdateOrderInfo();
    }

    private void UpdateOrderInfo()
    {
        OrderItems = _databaseService.Nomenclatures.ToList().Select(nomenclature => new MappedOrderItem
        {
            Count = 0,
            Nomenclature = nomenclature,
        }).ToList();
    }

    private void OnOrderCreatedClick(object sender, RoutedEventArgs e)
    {
        Order order = new Order()
        {
            OrderItems = OrderItems.Where((item) => item.Count > 0).Select((item) => new OrderItem()
            {
                Count = item.Count,
                Nomenclature = item.Nomenclature
            }).ToList()
        };

        _databaseService.Orders.Add(order);
        _databaseService.SaveChanges();
        MessageBox.Show("Заказ добавлен");
        UpdateOrderInfo();
    }
}