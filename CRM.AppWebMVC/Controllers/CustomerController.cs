using Microsoft.AspNetCore.Mvc;
using CRM.DTOs.CustomerDTOs;

namespace CRM.AppWebMVC.Controllers
{
    public class CustomerController : Controller
    {
       private readonly HttpClient _httpClientCRMAPI;     
        //Costructor que rice una instancia de IHttpClientFactory para crear el cliente HTTP
        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");
        }
        //Metodo para mostrar la lista de clientes
        public async Task<IActionResult> Index(SearchQueryCustomerDTO searchQueryCustomerDTO, int CountRow = 0)
        {
            //Configuracion de valores por defecto para la busqueda
            if (searchQueryCustomerDTO.SendRowCount == 0)
                searchQueryCustomerDTO.SendRowCount = 2;
            if (searchQueryCustomerDTO.Take == 0)
                searchQueryCustomerDTO.Take = 10;

            var result = new SearchResultCustomerDTO();

            try
            {
                //Realizar una solicitud HTTP POST para buscar clientes en el servidor web
                var response = await _httpClientCRMAPI.PostAsJsonAsync("/customer/search", searchQueryCustomerDTO);

                if (response.IsSuccessStatusCode)
                    result = await response.Content.ReadFromJsonAsync<SearchResultCustomerDTO>();

                result = result != null ? result : new SearchResultCustomerDTO();

                //configuracion de valores para la vista
                if (result.CountRow == 0 && searchQueryCustomerDTO.SendRowCount == 1)
                    result.CountRow = CountRow;

                ViewBag.CountRow = result.CountRow;
                searchQueryCustomerDTO.SendRowCount = 0;
                ViewBag.SearchQuery = searchQueryCustomerDTO;
                return View(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(new SearchResultCustomerDTO());
                throw;
            }

        }
        //Metodo para mostrar los detalles de un cliente
        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultCustomerDTO();

            //Realizar una solicitud HTTP GET para obtener los detalles del cliente por ID
            var response = await _httpClientCRMAPI.GetAsync("/customer" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();

            return View(result ?? new GetIdResultCustomerDTO());
        }
        //Metodo para mostrar el formulario de creacion de un cliente
        public ActionResult Creating()
        {
            return View();
        } 

        
    
        //Metodo para procesar la creacion de un cliente
        [HttPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerDTO createCustomerDTO)
        {
            try
            {
                //Realizar una solicitud HTTP POST para crear un nuevo cliente
                var response = await _httpClientCRMAPI.PostAsJsonAsync("/customer", createCustomerDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = "Error al intentar guardar el registro";
                return View();
            }
            catch(Exception ex)                         
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        
        //Metodo para mostrar el frmulario de edicion de un cliente
        public async Task<IActionResult> Edit(int id)
        {
            var result = new GetIdResultCustomerDTO();
            var response = await _httpClientCRMAPI.GetAsync("/customer/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();
            return View(new EditCustomerDTO(result ?? new GetIdResultCustomerDTO()));

        }
        //Metodo para procesar la edicion de un cliente
        [HttPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCustomerDTO editCustomerDTO)
        {
            try
            {
                //Realizar una solicitud HTTP PUT para editar el cliente
                var response = await _httpClientCRMAPI.PutAsJsonAsync("/customer", editCustomerDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "Error al intentar editar el registro";
                return View();
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        //Metodo para mostrar la pagina de configuracion de eliminacion de un cliente
        public async Task<IActionResult> Delete(int id)
        {
            var result = new GetIdResultCustomerDTO();
            var response = await _httpClientCRMAPI.GetAsync("/customer/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();

            return View(result ?? new GetIdResultCustomerDTO());
        }

        //Metodo para procesar la eliminacion de un cliente
        [HttPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GetIdResultCustomerDTO getIdResultCustomerDTO)
        {
            try
            {
                //Realiza una solicitud HTTP DELETE para eliminar el cliente por ID
                var response = await _httpClientCRMAPI.DeleteAsync("/customer/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = "Error al intentar eliminar el registro";
                return View(getIdResultCustomerDTO);
            }
            catch(Exception ex)
            {
                ViewBag.Erroe = ex.Message;
                return View(getIdResultCustomerDTO);
            }
        }
    }
}
