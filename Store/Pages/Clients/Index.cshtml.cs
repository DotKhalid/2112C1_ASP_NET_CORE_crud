using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;


namespace Store.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> CLientList = new List<ClientInfo>();
        public void OnGet()
        {    
            try
            {
                string connectionString = "Data Source=.;Initial Catalog=mystore;User ID=sa;Password=aptech";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    String Sql = "Select * from Clients";
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(Sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id ="" + reader.GetInt32(0);
                                clientInfo.Name = reader.GetString(1);
                                clientInfo.Email = reader.GetString(2);
                                clientInfo.Phone = reader.GetString(3);
                                clientInfo.Address = reader.GetString(4);
                                clientInfo.Created_at = reader.GetDateTime(5).ToString();
                                CLientList.Add(clientInfo);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex);
            }
        }
    }

    public class ClientInfo
    {
        public string id;
        public string Name;
        public string Email;
        public string Phone;
        public string Address;
        public string Created_at;

    }
}
