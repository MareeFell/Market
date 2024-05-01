using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Market.Service;
using Market.Ui.Pages;
using Market.Ui.Pages.Admin;
using Market.Ui.Pages.StoreWorker;
using Market.Ui.Pages.Worker;
using Microsoft.EntityFrameworkCore;

namespace Market
{
    public partial class App : Application
    {
        private readonly IHost _appHost;

        public App()
        {
            _appHost = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    // Тут менять имя базы
                    services.AddDbContext<DatabaseService>((options) =>
                        options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Market1;Trusted_Connection=True;"));

                    // Вид - Терминал
                    // Комментируешь 
                    // dotnet tool install --global dotnet-ef
                    // cd Market
                    // dotnet ef database update
                    // Расскоментируешь


                    // Начало коммента
                    services.AddSingleton<MainWindow>();
                    
                    
                    services.AddTransient<LoginPage>();
                    services.AddTransient<OrderProducts>();
                    services.AddTransient<ProductsList>();
                    services.AddTransient<ReportList>();
                    services.AddTransient<OrdersList>();
                    services.AddTransient<WorkerProductsList>();
                    services.AddTransient<WorkerReportList>();

                    services.AddTransient<AddProduct>();


                    services.AddSingleton<UserService>();
                    services.AddSingleton<StorageService>();
                    // Конец коммента

                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _appHost.StartAsync();


            var form = _appHost.Services.GetRequiredService<MainWindow>();
            form.ShowDialog();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _appHost.StopAsync();
            base.OnExit(e);
        }
    }
}