using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace TeZenNis.Controllers
{
    public class DeleteProductController : Controller
    {
        private string connString = "Server=HP-RIC\\SQLEXPRESS; Initial Catalog=TeZenNis; Integrated Security=true; TrustServerCertificate=True";
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete([FromRoute] int id)
        {
            var conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                var command = new SqlCommand($@"
                    DELETE from ProductData
                    WHERE ProductId = {id}", conn);
                //command.Parameters.AddWithValue("@ProductId", id);

                var rows = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               return RedirectToAction("Index", "Home");
            }
            finally { conn.Close(); }
            return RedirectToAction("Index", "Home");
        }
    }
}
