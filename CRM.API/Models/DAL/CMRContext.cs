
//Importa el espacio de nombres necesarios para DbContext.
using Microsoft.EntityFrameworkCore;
using CRM.API.Models.EN;

//Define la clase CRMContext que hereda de DbContext.
namespace CRM.API.Models.DAL
{
    public class CMRContext : DbContext
    {
        //Constructor que toma DbContextOptions como parametro para configurar la conexion a la base de datos.
        public CMRContext(DbContextOptions<CMRContext> options) : base(options)
        {}
        //Define un DbSet llamado Customer que representa una tabla de clientes en la base de datos.
        public DbSet<Customer> Customers { get; set; }

    }
}