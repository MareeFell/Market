using System.Windows.Controls;
using Market.Models;
using Market.Service;
using Microsoft.EntityFrameworkCore;

namespace Market.Ui.Pages.Worker;

public partial class WorkerProductsList : Page
{
    public List<Nomenclature> Nomenclatures { get; set; }

    public WorkerProductsList(DatabaseService databaseService)
    {
        InitializeComponent();
        DataContext = this;
        Nomenclatures = databaseService.Nomenclatures.Include(e => e.Category).ToList();
    }
}