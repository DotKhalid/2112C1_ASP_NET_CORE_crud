using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Store.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            string connectionString = "Data Source=.;Initial Catalog=mystore;User ID=sa;Password=aptech";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                String Sql = "Select * from Clients where id=@id";
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(Sql, conn))           
                {

                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            
                            clientInfo.id =""+ reader.GetInt32(0);
                            clientInfo.Name = reader.GetString(1);
                            clientInfo.Email = reader.GetString(2);
                            clientInfo.Phone = reader.GetString(3);
                            clientInfo.Address = reader.GetString(4);
                           
                        }
                    }
                }
            }
        }

         public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.Name = Request.Form["name"];
            clientInfo.Email = Request.Form["email"];
            clientInfo.Phone = Request.Form["phone"];
            clientInfo.Address = Request.Form["address"];

            if (clientInfo.Name.Length == 0 || clientInfo.Email.Length == 0 || clientInfo.Phone.Length == 0 ||
               clientInfo.Address.Length == 0)
            {
                errorMessage = "All the Fields are Required";
                return;
            }

            try
            {
                string connectionString = "Data Source=.;Initial Catalog=mystore;User ID=sa;Password=aptech";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Create the SQL insert statement
                    string sql = "UPDATE clients SET name=@name, email=@email, phone=@phone, address=@address WHERE id=@id";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add parameter values to the command
                        cmd.Parameters.AddWithValue("@name", clientInfo.Name);
                        cmd.Parameters.AddWithValue("@email", clientInfo.Email);
                        cmd.Parameters.AddWithValue("@phone", clientInfo.Phone);
                        cmd.Parameters.AddWithValue("@address", clientInfo.Address);
                        cmd.Parameters.AddWithValue("@id", clientInfo.id);

                        // Execute the command
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "New client Added Successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to add new client";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                errorMessage = "An error occurred while adding the new client";
            }
            Response.Redirect("/Clients/Index");

        }

    }
}
