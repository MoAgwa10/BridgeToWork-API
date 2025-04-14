using B2W.Models.Authentication;

namespace B2W.UserService
{
    public interface IUserRepo
    {


        List<object> GetAllUsers(); 
        ApplicationUser GetUserById(string id);

        Task<List<string>> GetUserIdsOnly();






    }
}
