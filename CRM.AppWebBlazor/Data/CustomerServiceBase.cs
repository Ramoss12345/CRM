using CRM.DTOs.CustomerDTOs;
//Importacion del espacio de nombres para los DTOs relacionados con los clientes
namespace CRM.AppWebBlazor.Data
{
    public class CustomerServiceBase
    {
        //Metodo para obtener un clientepor su ID utilizando una solicitud HTTP GET
        public async Task<GetIdResultCustomerDTO> GetById(int id)
        {
            var response = await _httpClientCRMAPI.GetAsync("/customer/" + id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();
                return result ?? new GetIdResultCustomerDTO();

            }
        }
    }
}