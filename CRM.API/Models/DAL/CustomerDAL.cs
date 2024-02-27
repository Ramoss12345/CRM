﻿    //Importa los espacios de nombres necesarios
using CRM.API.Models.EN;
using Microsoft.EntityFrameworkCore;

//Define la clase CustomerDAL que se utiliza para interactuar con los datos de los clientes den la base de datos.
namespace CRM.API.Models.DAL
{
    public class CustomerDAL
    {
        readonly CMRContext _context;
        //Constructor que reciba un objeto CRMContext para interactuar con la base de datos.
        public CustomerDAL(CMRContext cRMContext)
        {
            _context = cRMContext;
        }
        //Metodo para crear un nuevo cliente en la base de datos.
        public async Task<int> Create(Customer customer)
        {
            _context.Add(customer);
            return await _context.SaveChangesAsync();
        }
        //Metodo para obtener un cliente por su ID.
        public async Task<Customer> GetById(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(s => s.Id == id);
            return customer != null ? customer : new Customer();
        }
        //Metodo para editar un cliente existente en la base de datos.
        public async Task<int> Edit(Customer customer)
        {
            int result = 0;
            var customerUpdate = await GetById(customer.Id);
            if (customer.Id != 0)
            {
                //Actualiza los datos del cliente.
                customerUpdate.Name = customer.Name;
                customerUpdate.LastName = customer.LastName;
                customerUpdate.Address = customer.Address;
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        //Metod para eliminar un cliente de la base de datos por su ID.
        public async Task<int> Delete(int id)
        {
            int result = 0;
            var customerDelete = await GetById(id);
            if (customerDelete.Id != 0)
            {
                _context.Customers.Remove(customerDelete);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        //Metodo privado para construir una consulata IQueryable para buscar clientes con filtros.
        private IQueryable<Customer> Query(Customer customer)
        {
            var query = _context.Customers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(customer.Name))
                query = query.Where(s => s.Name.Contains(customer.Name));
            if (!string.IsNullOrWhiteSpace(customer.LastName))
                query = query.Where(s => s.LastName.Contains(customer.LastName));
            return query;
        }
        //Metodo para contar la cantida de resultado de busqueda con filtros.
        public async Task<int> CountSearch(Customer customer)
        {
            return await Query(customer).CountAsync();
        }
        //Metodo para buscar clientes con filtros , paginacion y ordenamiento.
        public async Task<List<Customer>> Search(Customer customer, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;
            var query = Query(customer);
            query = query.OrderByDescending(s => s.Id).Skip(skip).Take(take);
            return await query.ToListAsync();
        }
    }
}

