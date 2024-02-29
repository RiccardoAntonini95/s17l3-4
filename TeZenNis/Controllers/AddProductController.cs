﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TeZenNis.Models;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TeZenNis.Controllers
{
    public class AddProductController : Controller
    {
        private string connString = "Server=HP-RIC\\SQLEXPRESS; Initial Catalog=TeZenNis; Integrated Security=true; TrustServerCertificate=True";

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string name, double price, string description, IFormFile coverImg)
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
                    INSERT INTO ProductData
                    (ProductName, ProductPrice, ProductDescription, ProductImg) VALUES
                    (@name, @price, @description, @image)", conn);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@description", description);
                // questo valore lo ricaviamo dopo aver salvato l'immagine nel disco
                command.Parameters.AddWithValue("@image", fileName);

                // eseguire il comando
                var nRows = command.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("error");
            }
            finally { conn.Close(); }

            //var product = StaticDb.Add(name, price, description, coverImg);
            //return RedirectToAction("Index", "Home");
        }
    }
}
