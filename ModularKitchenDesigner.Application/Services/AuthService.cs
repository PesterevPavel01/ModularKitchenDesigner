
namespace ModularKitchenDesigner.Application.Services
{
    public class AuthService
    {
        public AuthService()
        {
            Users = new List<string>{"Иванов","Смирнов","Соболев"};
        }

        private readonly List<String> Users = [];

        public bool? Login(string name)
        {
            return Users.Contains(name);
        }
    }
}
