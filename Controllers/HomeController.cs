using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KoolApplicationMain.Models;
using MySql.Data.MySqlClient;
using System.Data;


namespace KoolApplicationMain.Controllers
{
    public class HomeController : Controller
    {
        private IProductInformation _productInformation;
        public HomeController(IProductInformation ProductInformation)
        {
            _productInformation = ProductInformation;
        }
        public IActionResult Index()
        {
            Search search = new Search();
            var model = new List<Product>();
            DataTable dt = new DataTable();
            MySqlDataAdapter mda;
            using (MySqlConnection conn = search.GetConnection())
            {

                conn.Open();
                string str = "select XXIBM_PRODUCT_SKU.Item_number,XXIBM_PRODUCT_SKU.description,XXIBM_PRODUCT_PRICING.List_price,XXIBM_PRODUCT_PRICING.In_stock from XXIBM_PRODUCT_SKU JOIN XXIBM_PRODUCT_PRICING ON XXIBM_PRODUCT_SKU.Item_number=XXIBM_PRODUCT_PRICING.Item_number ";

                //MySqlCommand cmd = new MySqlCommand(, conn);
                mda = new MySqlDataAdapter(str, search.GetConnection());
                mda.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    model.Add(new Product()
                    {
                        ItemNumber = Convert.ToInt32(dt.Rows[i]["Item_number"]),
                        Description = dt.Rows[i]["description"].ToString(),
                        Price = Convert.ToDouble(dt.Rows[i]["List_price"]),
                        Stock = dt.Rows[i]["In_stock"].ToString()

                    });
                }

            }
            return View(model);

            //return View();
        }
        public IActionResult ProductDetail(int p)
        {
            Search search = new Search();
            var model = new List<Product>();
            DataTable dt = new DataTable();
            MySqlDataAdapter mda;
            using (MySqlConnection conn = search.GetConnection())
            {
                conn.Open();
                string str = "select XXIBM_PRODUCT_SKU.Item_number,XXIBM_PRODUCT_SKU.description,XXIBM_PRODUCT_PRICING.List_price,XXIBM_PRODUCT_PRICING.In_stock from XXIBM_PRODUCT_SKU JOIN XXIBM_PRODUCT_PRICING ON XXIBM_PRODUCT_SKU.Item_number=XXIBM_PRODUCT_PRICING.Item_number where XXIBM_PRODUCT_SKU.Item_number=" + p + " ";
                mda = new MySqlDataAdapter(str, search.GetConnection());
                mda.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    model.Add(new Product()
                    {
                        ItemNumber = Convert.ToInt32(dt.Rows[i]["Item_number"]),
                        Description = dt.Rows[i]["description"].ToString(),
                        Price = Convert.ToDouble(dt.Rows[i]["List_price"]),
                        Brand = dt.Rows[i]["Brand"].ToString()

                    });
                }
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Search(string search)
        {

            string s = search.ToUpper();
            var result = _productInformation.GetProductsInformation();
            result = result.Where(l => string.Compare(l.Brand, s, true) == 0 || l.ClassName.ToUpper().Contains(s) ||
            l.CommodityName.ToUpper().Contains(s) || l.FamilyName.ToUpper().Contains(s) ||
            l.LongDescription.ToUpper().Contains(s) || l.Color.ToUpper().Contains(s) || l.Size.ToUpper().Contains(s)).ToList();
            if (result.Count == 0)
            {
                return View("NoResults");
            }
            ViewBag.name = search;
            return View("EachProductDetails", result);

        }

        public IActionResult EachProductDetails()
        {
            var list = _productInformation.GetProductsInformation();
            ViewBag.name = "All products";
            return View(list);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
