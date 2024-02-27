//Importacion del espacio de nombres para los DTOs relacionados con los clientes
using CRM.DTOs.CustomerDTOs;

namespace CRM.AppWebBlazor.Data
{
    public class CustomerService
    {
        readonly HttpClient _httpClientCRMAPI;

        //Constructor que recibe una instania de IHttpClientFactory para crear el clientevHTTP
        public CustomerService(IHttpClientFactory httpClientFactory)
        {
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");
        }
        //Metodo para buscar clientes utilanzando una solicitud HTTP POST
        public async Task<SearchResultCustomerDTO> Search(SearchQueryCustomerDTO searchQueryCustomerDTO)
        {
            var response = await _httpClientCRMAPI.PostAsJsonAsync("/customer/search", searchQueryCustomerDTO);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<SearchResultCustomerDTO>();
                return result ?? new SearchResultCustomerDTO();
            }
            return new SearchResultCustomerDTO(); //Devolver un objeto vacio en caso de error o respuesta no exitosa
        }
        //Metodo para obtener un clientepor su ID utilizando una solicitud HTTP GET
        public async Task<GetIdResultCustomerDTO> GetById(int id)
        {
            var response = await _httpClientCRMAPI.GetAsync("/customer/" + id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();
                return result ?? new GetIdResultCustomerDTO();

            }
            return new GetIdResultCustomerDTO(); // Devolver un objeto vacio en caso de error o respuesta no exitosa
        }
        // Metodo para crear un cliente utilizando una solicitud HTTP POST
        public async Task<int> Create(CreateCustomerDTO createCustomerDTO)
        {
            int result = 0;
            var response = await _httpClientCRMAPI.PostAsJsonAsync("/customer", createCustomerDTO);
            if (response.IsSuccessStatusCode)
            {
                var reponseBody = await response.Content.ReadAsStringAsync();
                if (int.TryParse(reponseBody, out result) == false)
                    result = 0;

            }
            return result;
        }
        // Metodo para editar un cliente utilizando una solicitud HTTP POST
        public async Task<int> Edit(EditCustomerDTO editCustomerDTO)
        {
            int result = 0;
            var response = await _httpClientCRMAPI.PostAsJsonAsync("/customer", editCustomerDTO);
            if (response.IsSuccessStatusCode)
            {
                var reponseBody = await response.Content.ReadAsStringAsync();
                if (int.TryParse(reponseBody, out result) == false)
                    result = 0;

            }
            return result;
        }
        // Metodo para eliminar un cliente utilizando una solicitud HTTP POST
        public async Task<int> Delete(int id)
        {
            int result = 0;
            var response = await _httpClientCRMAPI.DeleteAsync("/customer/" + id);
            if (response.IsSuccessStatusCode)
            {
                var reponseBody = await response.Content.ReadAsStringAsync();
                if (int.TryParse(reponseBody, out result) == false)
                    result = 0;

            }
            return result;
        }
    }
}

