

using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Classes.Models
{
    // Абстракная фабрика
    public class User : IdentityUser // базовая модель
    {
        public string Role { get; set; }


    }

    //  Создание конкретных классов пользователей
    public class AdminUser : User
    {
        public AdminUser()
        {
            Role = "Admin";
        }
    }

    public class RegularUser : User
    {
        public RegularUser()
        {
            Role = "Regular";
        }
    }

    // Создание интерфейса фабрики
    public interface IUserFactory
    {
        User CreateUser(string username, string password, string email);
    }

    // Создание конкретных фабрик для каждого типа пользователя
    public class AdminUserFactory : IUserFactory
    {
        public User CreateUser(string username, string password, string email)
        {
            return new AdminUser { UserName = username, PasswordHash = password, Email = email };
        }
    }

        public class RegularUserFactory : IUserFactory
        {
            public User CreateUser(string username, string password, string email)
            {
                return new RegularUser { UserName = username, PasswordHash = password, Email = email };
            }
        }
        // Использование абстрактной фабрики
        public class UserService
        {
            private readonly IUserFactory _userFactory;

            public UserService(IUserFactory userFactory)
            {
                _userFactory = userFactory;
            }

            public User RegisterUser(string username, string password, string email)
            {
                return _userFactory.CreateUser(username, password, email);
            }
        }
    }

