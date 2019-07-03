using System.Threading.Tasks;
using WebAdvert.Web.ServiceClients.Models;

namespace WebAdvert.Web.ServiceClients
{
    public interface IAdvertApiClient
    {
        Task<CreateAdvertResponseModel> CreateAsync(AdvertModel model);
        Task<bool> ConfirmAsync(ConfirmAdvertModel model);
    }
}
