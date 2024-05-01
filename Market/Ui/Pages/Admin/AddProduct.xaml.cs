using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Market.Models;
using Market.Service;

namespace Market.Ui.Pages.Admin;

public partial class AddProduct : Window
{
    public List<Category> Categories { get; set; }
    private DatabaseService _databaseService;
    
    public Category CurrentCategory { get; set; }
    public String NomenclatureName { get; set; }
    public String Description { get; set; }
    public int SellPrice { get; set; }
    public int BuyPrice { get; set; }
    public int Count { get; set; }

    public AddProduct(DatabaseService databaseService)
    {
        InitializeComponent();
        DataContext = this;
        _databaseService = databaseService;
        Categories = databaseService.Categories.ToList();
        CurrentCategory = Categories.First();
    }

    private void AddProductClick(object sender, RoutedEventArgs e)
    {
        _databaseService.Nomenclatures.Add(new Nomenclature
        {
            Name = NomenclatureName,
            Count = Count,
            Description = Description,
            SellPrice = SellPrice,
            BuyPrice = BuyPrice,
            Category = CurrentCategory
        });
        _databaseService.SaveChanges();
        MessageBox.Show("Успешно сохранено");
        Close();
    }

    private void NumberPViewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !int.TryParse(e.Text, out int _);
    }
}