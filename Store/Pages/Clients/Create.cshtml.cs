using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Store.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";


        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.Name = Request.Form["name"];
            clientInfo.Email = Request.Form["email"];
            clientInfo.Phone = Request.Form["phone"];
            clientInfo.Address = Request.Form["address"];


            if (clientInfo.Name.Length == 0 || clientInfo.Email.Length == 0 || clientInfo.Phone.Length == 0 | clientInfo.Address.Length == 0)
            {
                errorMessage = "All Field mut be required";
                return;
            }
            try
            {

                string connectionString = "Data Source=.;Initial Catalog=mystore;User ID=sa;Password=aptech";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    String Sql = "insert into Clients Values(@name,@email,@phone,@address,@created_at)";
                    using (SqlCommand cmd = new SqlCommand(Sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", clientInfo.Name);
                        cmd.Parameters.AddWithValue("@email", clientInfo.Email);
                        cmd.Parameters.AddWithValue("@phone", clientInfo.Phone);
                        cmd.Parameters.AddWithValue("@address", clientInfo.Address);
                        cmd.Parameters.AddWithValue("@created_at", DateTime.Now);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if(rowsAffected > 0)
                        {
                            successMessage = "Client Added succcefully";
                        }
                        else
                        {
                            errorMessage = "Failed to add client";

                        }

                    }




                        conn.Close();

                }


            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : "+ ex);
                errorMessage = "An error occured";
            }
            Response.Redirect("/Clients/index");


        }
    }
}
