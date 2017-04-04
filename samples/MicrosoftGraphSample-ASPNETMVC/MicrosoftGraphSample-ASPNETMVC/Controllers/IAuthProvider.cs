using System.Threading.Tasks;

namespace MicrosoftGraphSample_ASPNETMVC.Controllers
{
    public interface IAuthProvider
    {
        Task<string> GetUserAccessTokenAsync();
    }

}