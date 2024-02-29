using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeZenNis.Models;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace TeZenNis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        private string connString = "Server=HP-RIC\\SQLEXPRESS; Initial Catalog=TeZenNis; Integrated Security=true; TrustServerCertificate=True";

        [HttpGet]
        public IActionResult Index()
        {
            var conn = new SqlConnection(connString);
            List<Product> products = [];

            try
            {
                conn.Open();
                //Creare comando per selezionare tutto il contenuto del db con una query
                var command = new SqlCommand("SELECT * from ProductData", conn);
                //Eseguire il comando
                var reader = command.ExecuteReader();

                //Usare i dati
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var product = new Product() //ciclo i record del database e creo un nuovo prodotto usando il modello esistente
                        {
                            Id = (int)reader["ProductId"],
                            Name = (string)reader["ProductName"],
                            Price = Convert.ToDouble(reader["ProductPrice"]),
                            Description = (string)reader["ProductDescription"],
                            CoverImg = (string)reader["ProductImg"]
                        };
                        products.Add(product);
                    }
                }
            }
            catch(Exception ex) 
            { 
              return View("Error"); 
               
            }
            finally { conn.Close(); }
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
