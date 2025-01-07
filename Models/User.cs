using Amazon.DynamoDBv2.DataModel;

namespace UserRegisterDynamo.Models
{
    [DynamoDBTable("users-table")]
    public class User
    {
        [DynamoDBHashKey("pk")]
        public string Cpf { get; set; }
        [DynamoDBRangeKey("sk")]
        public string Username { get; set; }
        [DynamoDBProperty]
        public string FirstName { get; set; }
        [DynamoDBProperty]
        public string LastName { get; set; }
        [DynamoDBProperty]
        public string Email { get; set; }
        [DynamoDBProperty]
        public string Password { get; set; }
        [DynamoDBProperty]
        public string Code { get; set; }
        [DynamoDBProperty]
        public bool Confirmation { get; set; }

        public User(CreateUser dto, string code)
        {
            this.Cpf = dto.Cpf;
            this.Username = dto.Username;
            this.FirstName = dto.FirstName;
            this.LastName = dto.LastName;
            this.Email = dto.Email;
            this.Password = dto.Password;
            this.Confirmation = false;
            Code = code;
        }
        public User()
        {
            
        }
    }
}
