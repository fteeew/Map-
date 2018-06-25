using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WebApplication1.Models;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using System.Diagnostics;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()

        {
            return View();
        }
        public String GetDataFromUrl(string url)
        {
            WebClient client = new WebClient();
            string data = client.DownloadString(url);
            return data;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        [WebMethod]
        public ActionResult getData(string url, string blockName, string projection, string bound, int Layer_site)
        {


            var cnnString = ConfigurationManager.ConnectionStrings["mytestdbEntities1"].ConnectionString;
            var efConnectionStringBuilder = new EntityConnectionStringBuilder(cnnString);
            string sqlConnectionString = efConnectionStringBuilder.ProviderConnectionString;

            // var cmd = "insert into Layer values(@LayerName,@LayerProjection,@LayerBounds)";
            var cmd2 = "insert into Layers(LayerName,LayerProjection,LayerBounds,Layer_site)values(@LayerName,@LayerProjection,@LayerBounds,@Layer_site)";
            using (SqlConnection cnn = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand c = new SqlCommand())

                {
                    c.Connection = cnn;
                    c.CommandType = CommandType.Text;
                    c.CommandText = cmd2;
                    c.Parameters.AddWithValue("@LayerName", blockName);
                    c.Parameters.AddWithValue("@LayerProjection", projection);
                    c.Parameters.AddWithValue("@LayerBounds", bound);
                    c.Parameters.AddWithValue("@Layer_site", Layer_site);


                    cnn.Open();
                    c.ExecuteNonQuery();




                }
            }

            return View();

        }

        public ActionResult Reload()
        {
            var cnnString = ConfigurationManager.ConnectionStrings["mytestdbEntities1"].ConnectionString;
            var efConnectionStringBuilder = new EntityConnectionStringBuilder(cnnString);
            string sqlConnectionString = efConnectionStringBuilder.ProviderConnectionString;
            // SqlConnection cnn = new SqlConnection(sqlConnectionString);

            //String sql = "Select LayerName,LayerBounds from Layers";
            //SqlCommand cmd = new SqlCommand("select LayerName from Layers where LayerName ='Aya:blocks'", cnn);
            List<Layer> layer = new List<Layer>();
            using (SqlConnection cnn = new SqlConnection(sqlConnectionString))
            {

                string query = "SELECT LayerName, LayerBounds, LayerProjection,Layer_site FROM Layers";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = cnn;
                    cnn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            layer.Add(new Layer
                            {
                                LayerName = sdr["LayerName"].ToString(),
                                Layer_site = Convert.ToInt32(sdr["Layer_site"]),
                                LayerBounds = sdr["LayerBounds"].ToString(),
                                LayerProjection = sdr["LayerProjection"].ToString()

                            });


                        }
                    }
                    cnn.Close();
                }
            }
            return View(layer);

        }


        public ActionResult Map()
        {


            return View();
        }
        public ActionResult Refresh()
        {
            var cnnString = ConfigurationManager.ConnectionStrings["mytestdbEntities"].ConnectionString;
            var efConnectionStringBuilder = new EntityConnectionStringBuilder(cnnString);
            string sqlConnectionString = efConnectionStringBuilder.ProviderConnectionString;
            // SqlConnection cnn = new SqlConnection(sqlConnectionString);

            //String sql = "Select LayerName,LayerBounds from Layers";
            //SqlCommand cmd = new SqlCommand("select LayerName from Layers where LayerName ='Aya:blocks'", cnn);
            List<Layer> layer = new List<Layer>();
            using (SqlConnection cnn = new SqlConnection(sqlConnectionString))
            {

                string query = "SELECT LayerName, LayerBounds, LayerProjection,Layer_site FROM Layers";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = cnn;
                    cnn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            layer.Add(new Layer
                            {
                                LayerName = sdr["LayerName"].ToString(),
                                Layer_site = Convert.ToInt32(sdr["Layer_site"]),
                                LayerBounds = sdr["LayerBounds"].ToString(),
                                LayerProjection = sdr["LayerProjection"].ToString()

                            });


                        }
                    }
                    cnn.Close();
                }
            }
            return View(layer);


        }
        public ActionResult delete()
        {
            var cnnString = ConfigurationManager.ConnectionStrings["mytestdbEntities"].ConnectionString;
            var efConnectionStringBuilder = new EntityConnectionStringBuilder(cnnString);
            string sqlConnectionString = efConnectionStringBuilder.ProviderConnectionString;
            string sqlStatement = "DELETE FROM Layers WHERE LayerName = @LayerName";
            SqlConnection cnn = new SqlConnection(sqlConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlStatement, cnn);
                cmd.Parameters.AddWithValue("@LayerName", "Aya:urban_master_plan");
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            finally
            {
                cnn.Close();
            }
            return View();
        }
        [WebMethod]
        public void update(string url, string blockName, string projection, string bound, int Layer_site)
        {
            var cnnString = ConfigurationManager.ConnectionStrings["mytestdbEntities"].ConnectionString;
            var efConnectionStringBuilder = new EntityConnectionStringBuilder(cnnString);
            string sqlConnectionString = efConnectionStringBuilder.ProviderConnectionString;
            using (SqlConnection cnn = new SqlConnection(sqlConnectionString))
            {
                string query = "UPDATE Layers SET LayerName =@LayerName, LayerProjection=@LayerProjection,LayerBounds=@LayerBounds,Layer_site=@Layer_site WHERE (LayerName =@LayerName)";
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("@LayerName", blockName);
                    cmd.Parameters.AddWithValue("@LayerProjection", projection);
                    cmd.Parameters.AddWithValue("@LayerBounds", bound);
                    cmd.Parameters.AddWithValue("@Layer_site ", Layer_site);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
    }
}