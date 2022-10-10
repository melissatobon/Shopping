using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shopping.Helpers
{
	public interface ICombosHelper
	{
        Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync();//Lista de categorias

        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();//Lista de países

        Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId);//Lista de departamentos de x país

        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId);//Lista de ciudades de x departamento

    }
}
