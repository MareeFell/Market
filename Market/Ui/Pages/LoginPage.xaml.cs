using System.Windows;
using System.Windows.Controls;
using Market.Service;

namespace Market.Ui.Pages;

public partial class LoginPage : Page
{
    private readonly UserService _userService;
    public String Login { get; set; } = "";
    public String Password { get; set; } = "";

    public LoginPage(UserService userService)
    {
        InitializeComponent();
        DataContext = this;
        _userService = userService;
    }

    private void OnLoginClick(object sender, RoutedEventArgs e)
    {
        var (isSuccess, message) = _userService.Login(Login, Password);
        MessageBox.Show(message);

        if (!isSuccess) return;
    }
}