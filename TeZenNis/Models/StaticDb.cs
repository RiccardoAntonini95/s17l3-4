namespace TeZenNis.Models
{
    public class StaticDb
    {
        private static int _maxId = 3;
        private static List<Product> _productsList = [
            new Product(){Id = 1, Name = "Prodotto1", Price = 49.99, Description = "We wagliò bello sto prodotto1", CoverImg = "https://cdn.pixabay.com/photo/2023/05/03/22/43/tennis-7968714_1280.png" },
            new Product(){Id = 2, Name = "Prodotto2", Price = 60.50, Description = "We wagliò bello sto prodotto2", CoverImg = "https://cdn.pixabay.com/photo/2022/06/06/21/03/footwear-7246982_640.png" },
            new Product(){Id = 3, Name = "Prodotto3", Price = 59.99, Description = "We wagliò bello sto prodotto3", CoverImg = "https://cdn.pixabay.com/photo/2014/04/02/10/19/tennis-shoes-303451_640.png" }
        ];

        public static List<Product> GetAll()
        {
            return _productsList;
        }

        public static Product Add(string name, double price, string description, string coverImg)
        {
            _maxId++;
            var product = new Product() {Id = _maxId, Name = name, Price = price, Description = description, CoverImg = coverImg};
            _productsList.Add(product);
            return product;
        }
    }
}
