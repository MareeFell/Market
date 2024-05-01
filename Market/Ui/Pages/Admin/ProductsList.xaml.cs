using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Market.Models;
using Market.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MessageBox = System.Windows.MessageBox;

namespace Market.Ui.Pages.Admin;

public partial class ProductsList : Page
{
    public List<Nomenclature> Nomenclatures { get; set; }
    private DatabaseService _databaseService { get; }
    private IHost _host;

    public ProductsList(DatabaseService databaseService, IHost host)
    {
        InitializeComponent();
        DataContext = this;
        _databaseService = databaseService;
        _host = host;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        Nomenclatures = _databaseService.Nomenclatures.Include(e => e.Category).ToList();
    }

    private void OnSaveClick(object sender, RoutedEventArgs e)
    {
        _databaseService.SaveChanges();
        MessageBox.Show("Изменения сохранены");
        UpdateInfo();
    }

    private void AddProductClick(object sender, RoutedEventArgs e)
    {
        _host.Services.GetRequiredService<AddProduct>().ShowDialog();
    }
}