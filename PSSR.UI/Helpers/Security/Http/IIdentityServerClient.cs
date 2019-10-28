
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security.Http
{
    public interface IIdentityServerClient
    {
        Task<string> RequestClientCredentialsTokenAsync();
    }
}
