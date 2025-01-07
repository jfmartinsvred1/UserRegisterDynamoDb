using Amazon.DynamoDBv2.DataModel;
using UserRegisterDynamo.Models;
using UserRegisterDynamo.Services;

namespace UserRegisterDynamo.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly ILogger<UserRepository> _logger;
        private readonly EmailService _emailService;

        public UserRepository(IDynamoDBContext context, ILogger<UserRepository> logger, EmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Add(CreateUser dto)
        {
            try
            {
                var code = Guid.NewGuid().ToString().Substring(0, 6);
                await _emailService.SendEmailAsync
                    (
                        dto.Email,
                        dto.FirstName,
                        "Confirmação de usuário",
                        $"Código de confirmação: {code}"
                    );
                User user = new User(dto,code);
                await _context.SaveAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar user");
            }
        }

        public async Task<bool> ConfirmCode(string cpf, string username, string password, string code)
        {
            var user = await GetByCpf(cpf,username,password);
            if (user != null && user.Code == code && user.Confirmation == false)
            {
                user.Confirmation = true;
                user.Code = "";
                await Update(user);
                return true;
            }
            return false;
        }

        public Task Delete(string cpf, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByCpf(string cpf, string username, string password)
        {
            try
            {
                var itens = await _context.QueryAsync<User>(cpf, Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, new object[] { username })
                .GetRemainingAsync();

                var user = itens.FirstOrDefault();
                if (user != null && user.Password == password)
                {
                    return user;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao buscar usuario");
                
            }
            return null;
        }


        public async Task Update(User user)
        {
            try
            {
                await _context.SaveAsync(user);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex,$"{ex.Message}");
            }
        }
    }
}
