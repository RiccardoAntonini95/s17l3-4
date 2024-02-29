using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TeZenNis.Models;

namespace TeZenNis.Controllers
{
    public class EditProductController : Controller
    {
        private string connString = "Server=HP-RIC\\SQLEXPRESS; Initial Catalog=TeZenNis; Integrated Security=true; TrustServerCertificate=True";
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
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

        [HttpPost]
        public IActionResult Edit([FromRoute]int id, string name, double price, string description, IFormFile coverImg)
        {
            var conn = new SqlConnection(connString);
            try
            {
                // validare i dati

                conn.Open();

                // salviamo il file che ci è stato inviato
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                string fileName = Path.GetFileName(coverImg.FileName);
                string fullFilePath = Path.Combine(path, fileName);
                // TODO: generare un nome univoco
                //string fullFilePath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\uploads\\{Path.GetFileName(image.FileName)}";
                FileStream stream = new FileStream(fullFilePath, FileMode.Create);
                coverImg.CopyTo(stream);


                // creare il comando
                var command = new SqlCommand(@"
                    UPDATE ProductData
                    SET ProductName = @name, ProductPrice = @price, ProductDescription = @description, ProductImg = @image
                    WHERE ProductId = @ProductId", conn);
                command.Parameters.AddWithValue("@ProductId", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@description", description);
                // questo valore lo ricaviamo dopo aver salvato l'immagine nel disco
                command.Parameters.AddWithValue("@image", fileName);

                // eseguire il comando
                var nRows = command.ExecuteNonQuery();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View("error");
            }
            finally { conn.Close(); }
        }
    }
}
