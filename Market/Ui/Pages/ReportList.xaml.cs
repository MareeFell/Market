using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Market.Models;
using Market.Service;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Market.Ui.Pages;

public partial class ReportList : Page
{
    public List<Order> Orders { get; set; } = new();
    private DatabaseService _databaseService;

    public ReportList(DatabaseService daDatabaseService)
    {
        _databaseService = daDatabaseService;
        InitializeComponent();
        DataContext = this;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        Orders.Clear();
        Orders.AddRange(_databaseService.Orders.Include((e) => e.OrderItems).ThenInclude(e => e.Nomenclature).ToList()
            .ToList());
    }

    private void CreateOrderClick(object sender, RoutedEventArgs e)
    {
        SaveFileDialog dialog = new SaveFileDialog()
        {
            Filter = "Excel Files(*.xlsx)|*.xlsx"
        };

        var res = dialog.ShowDialog();

        if (res != DialogResult.OK) return;

        var package = new ExcelPackage();

        foreach (var order in _databaseService.Orders.Include((order) => order.OrderItems)
                     .ThenInclude(order => order.Nomenclature)
                     .ToList())
        {
            var list = package.Workbook.Worksheets.Add($"Заказ № {order.Id}");

            list.Cells[1, 1].Value = "Номер заказа";
            list.Cells[1, 2].Value = "Статус";

            list.Cells[2, 1].Value = order.Id.ToString();
            list.Cells[2, 2].Value = order.OrderStatus.ToString();

            list.Cells[1, 4].Value = "Имя номенклатуры";
            list.Cells[1, 5].Value = "PLU";
            list.Cells[1, 6].Value = "Количество";

            for(int i = 0; i < order.OrderItems.Count; i++)
            {
                var nomenclature = order.OrderItems[i];

                list.Cells[2 + i, 4].Value = nomenclature.Nomenclature.Name;
                list.Cells[2 + i, 5].Value = nomenclature.Nomenclature.Id.ToString();
                list.Cells[2 + i, 6].Value = nomenclature.Count.ToString();
            }
        }
        
        
        File.WriteAllBytes(dialog.FileName, package.GetAsByteArray());
    }
}