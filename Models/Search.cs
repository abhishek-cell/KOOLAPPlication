using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace KoolApplicationMain.Models
{
    public class Search
    {
		public string ConnectionString { get; set; }



		public MySqlConnection GetConnection()
		{
			//database connection 
			ConnectionString = @"server=custom-mysql.gamification.svc.cluster.local; port=3306; database=sampledb;uid=xxuser;pwd=welcome1";
			return new MySqlConnection(ConnectionString);
		}



		public List<Product> GetProducts(string search)
		{
			List<Product> list = new List<Product>();

			using (MySqlConnection conn = GetConnection())
			{
				conn.Open();
				MySqlCommand cmd = new MySqlCommand("select XXIBM_PRODUCT_SKU.Item_number,XXIBM_PRODUCT_SKU.description,XXIBM_PRODUCT_PRICING.List_price,XXIBM_PRODUCT_PRICING.In_stock from XXIBM_PRODUCT_SKU JOIN XXIBM_PRODUCT_PRICING ON XXIBM_PRODUCT_SKU.Item_number=XXIBM_PRODUCT_PRICING.Item_number AND XXIBM_PRODUCT_SKU.description LIKE '%" + search + "%'", conn);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new Product()
						{
							ItemNumber = Convert.ToInt32(reader["Item_number"]),
							Description = reader["description"].ToString(),
							Price = Convert.ToDouble(reader["List_price"]),
							Stock = reader["In_stock"].ToString()

						});
					}
				}
			}
			return list;
		}
	}
}
