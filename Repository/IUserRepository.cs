using UserRegisterDynamo.Models;

namespace UserRegisterDynamo.Repository
{
    public interface IUserRepository
    {
        Task Add(CreateUser user);
        Task Update(User user);
        Task Delete(string cpf, string username);
        Task<User> GetByCpf(string cpf, string username, string password);
        Task<bool> ConfirmCode(string cpf, string username, string password, string code);
    }
}
