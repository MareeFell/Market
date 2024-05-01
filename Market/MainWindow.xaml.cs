using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Market.Models;
using Market.Service;
using Market.Ui.Pages;
using Market.Ui.Pages.Admin;
using Market.Ui.Pages.StoreWorker;
using Market.Ui.Pages.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Market
{
    public partial class MainWindow : Window
    {
        public UserService UserService { get; }
        private IHost _host;
        private StorageService _storageService;

        private Dictionary<Roles, List<Type>> _screens = new()
        {
            {
                Roles.Admin,
                [typeof(OrderProducts), typeof(ProductsList), typeof(ReportList)]
            },
            {
                Roles.Worker,
                [typeof(WorkerProductsList), typeof(WorkerReportList)]
            },
            {
                Roles.StoreWorker,
                [typeof(OrdersList), typeof(ReportList)]
            }
        };

        public MainWindow(LoginPage loginPage, UserService userService, IHost host, StorageService storageService)
        {
            InitializeComponent();
            _storageService = storageService;
            UserService = userService;
            _host = host;
            DataContext = this;
            userService.PropertyChanged += (_, _1) =>
            {
                if (userService.User != null)
                    GoToScreen(0);
                else
                    Frame.Navigate(_host.Services.GetRequiredService<LoginPage>());
            };

            Frame.Navigated += (sender, args) =>
            {
                if (args.Content is Page)
                {
                    Title = ((Page)args.Content).Title;
                }            
            };

            if (!userService.TryLoadUser())
            {
                Frame.Navigate(loginPage);
                GoToScreen(0);
            }
        }


        private void GoToScreen(int index)
        {
            if (UserService.User == null) return;

            var page = _host.Services.GetRequiredService(_screens[UserService.User.Role][index]);
            Frame.Navigate(page);
        }


        private void OnOrderProductListClick(object sender, MouseButtonEventArgs e) => GoToScreen(0);
        private void OnProductsListClick(object sender, MouseButtonEventArgs e) => GoToScreen(1);
        private void OnReportListClick(object sender, MouseButtonEventArgs e) => GoToScreen(2);

        private void Logout(object sender, MouseButtonEventArgs e)
        {
            UserService.Logout();
        }
    }
}