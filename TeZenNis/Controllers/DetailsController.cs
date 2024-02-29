using Microsoft.AspNetCore.Mvc;
using TeZenNis.Models;
using Microsoft.Data.SqlClient;

namespace TeZenNis.Controllers
{
    public class DetailsController : Controller
    {
        private string connString = "Server=HP-RIC\\SQLEXPRESS; Initial Catalog=TeZenNis; Integrated Security=true; TrustServerCertificate=True";
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index([FromRoute] int id)
        {
            var prodDetail = new Product();
            var conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                var command = new SqlCommand("SELECT * from ProductData where ProductId=@ProductId", conn);
                command.Parameters.AddWithValue("@ProductId", id);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    prodDetail.Id = (int)reader["ProductId"];
                    prodDetail.Name = (string)reader["ProductName"];
                    prodDetail.Price = Convert.ToDouble(reader["ProductPrice"]);
                    prodDetail.Description = (string)reader["ProductDescription"];
                    prodDetail.CoverImg = (string)reader["ProductImg"];
                    return View(prodDetail);
                }
                return View(prodDetail);
            }
            catch (Exception ex)
            {
                return View("error");
            }
            finally { conn.Close(); }

        }
    }
}
