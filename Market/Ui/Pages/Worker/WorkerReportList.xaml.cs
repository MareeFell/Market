using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Market.Models;
using Market.Service;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Market.Ui.Pages.Worker;

public partial class WorkerReportList : Page
{
    public List<Nomenclature> Nomenclatures { get; set; } = new();
    private DatabaseService _databaseService;

    public WorkerReportList(DatabaseService databaseService)
    {
        InitializeComponent();
        DataContext = this;
        _databaseService = databaseService;
        Nomenclatures.AddRange(_databaseService.Nomenclatures.Include((e) => e.Category).ToList()
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

        var listData = _databaseService.Nomenclatures.Include((order) => order.Category).ToList();
        var list = package.Workbook.Worksheets.Add($"Номенклатура");

        for(int i = 0; i < listData.Count; i++)
        {
            var nomenclature = listData[i];

            list.Cells[1, 1].Value = "Имя номенклатуры";
            list.Cells[1, 2].Value = "PLU";
            list.Cells[1, 3].Value = "Количество";
            list.Cells[1, 4].Value = "Категория";
            list.Cells[1, 5].Value = "Цена закупки";
            list.Cells[1, 6].Value = "Цена продажи";

            list.Cells[2 + i, 1].Value = nomenclature.Name;
            list.Cells[2 + i, 2].Value = nomenclature.Id.ToString();
            list.Cells[2 + i, 3].Value = nomenclature.Count.ToString();
            list.Cells[2 + i, 4].Value = nomenclature.Category.Name;
            list.Cells[2 + i, 5].Value = nomenclature.BuyPrice.ToString();
            list.Cells[2 + i, 6].Value = nomenclature.SellPrice.ToString();
        }


        File.WriteAllBytes(dialog.FileName, package.GetAsByteArray());
    }
}