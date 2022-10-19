using Shopping.Common;
using Shopping.Models;

namespace Shopping.Helpers
{
    public interface IOrdersHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);
    }
}
