using System.ComponentModel;
using System.Runtime.CompilerServices;
using Market.Models;

namespace Market.Service;

public class UserService: INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private DatabaseService _database;
    private StorageService _storageService;
    private User? _user;

    public User? User
    {
        get => _user;
        set
        {
            _user = value;
            OnPropertyChanged(nameof(User));
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsStoreWorker));
            OnPropertyChanged(nameof(IsWorker));
        }
    }
    public bool IsAdmin => User is { Role: Roles.Admin };
    public bool IsStoreWorker => User is { Role: Roles.StoreWorker };
    public bool IsWorker => User is { Role: Roles.Worker };
    
    public UserService(DatabaseService database, StorageService storageService)
    {
        _database = database;
        _storageService = storageService;
    }


    public void Logout()
    {
        _storageService.SetData(new StorageService.SettingsInfo());
        User = null;
    }

    public bool TryLoadUser()
    {
        var data = _storageService.GetData();
        
        if (data.UserId == null) return false;
        User = _database.Users.FirstOrDefault((user) => user.Id == data.UserId);
        if (User == null) return false;
        return true;
    }

    public (Boolean, String) Login(String login, String password)
    {
        var candidate = _database.Users.FirstOrDefault((user) => user.Login == login);

        if (candidate == null) return (false, "Пользователя с таким логином нет");

        if (candidate.Password != password) return (false, "Пароль не совпадает");
        
        User = candidate;

        return (true, "Успешный вход");
    }


    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (User != null)
        {
            _storageService.SetData(new StorageService.SettingsInfo()
            {
                UserId = User.Id
            });
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}