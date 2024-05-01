using System.Windows;
using System.Windows.Controls;
using Market.Models;
using Market.Service;
using Microsoft.EntityFrameworkCore;

namespace Market.Ui.Pages.StoreWorker;

public class OrderWithDetails
{
    public Order Order { get; set; }
    public bool isDelivered { get; set; }
}

public partial class OrdersList : Page
{
    private DatabaseService _databaseService;

    public List<OrderWithDetails> Orders { get; set; } = new();

    public OrdersList(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        InitializeComponent();
        DataContext = this;
        UpdateInfo();
    }


    private void UpdateInfo()
    {
        Orders.Clear();
        Orders.AddRange(_databaseService.Orders.Where((e) => e.OrderStatus == OrderStatus.Delivered)
            .Include((e) => e.OrderItems).ThenInclude(e => e.Nomenclature).ToList().Select(e => new OrderWithDetails
            {
                Order = e,
                isDelivered = false
            }).ToList());
    }

    private void UpdateDeliveryClick(object sender, RoutedEventArgs e)
    {
        foreach (var order in Orders.Where(order => order.isDelivered))
        {
            order.Order.OrderStatus = OrderStatus.Accept;
            foreach (var nomenclature in order.Order.OrderItems)
            {
                nomenclature.Nomenclature.Count += nomenclature.Count;
            }
        }

        _databaseService.SaveChanges();
        MessageBox.Show("Успешно обновлено");
        UpdateInfo();
    }
}