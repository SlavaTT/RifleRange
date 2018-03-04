using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RifleRange.DAL;

namespace RifleRange.Models
{
    public class LoginModel
    {
        [Required]
        [DisplayName("Логин")]
        public string LoginName { get; set; }

        [Required]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class RegisterModel
    {

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Имя")]
        [StringLength(30, MinimumLength = 5)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(30, MinimumLength = 5)]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        public string UserName => $"{FirstName}, {LastName}";

        [StringLength(30)]
        [DisplayName("Email")]
        public string Email{ get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 5)]
        [DisplayName("Логин")]
        public string LoginName { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 5)]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        [StringLength(15, MinimumLength = 5)]
        [DisplayName("Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public RegisterModel(rrUser User)
        {
            FirstName = User.FirstName;
            LastName = User.LastName;
            LoginName = User.LoginName;
            Password = User.Password;
            Email = User.Email;
        }
        public RegisterModel()
        {

        }
    }
}