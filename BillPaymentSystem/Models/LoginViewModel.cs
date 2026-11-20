using System.ComponentModel.DataAnnotations;

namespace BillPaymentSystem.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunlu.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunlu.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
