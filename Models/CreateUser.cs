using System.ComponentModel.DataAnnotations;

namespace UserRegisterDynamo.Models
{
    public class CreateUser
    {
        [Required]
        [MaxLength(11)]
        public string Cpf { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
