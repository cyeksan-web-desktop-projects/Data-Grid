using Data_Grid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.SqlClient;

namespace Data_Grid.Controllers
{
    public class HomeController : Controller
    {

        SqlCommand command = new SqlCommand();
        SqlDataReader dataReader;
        SqlConnection connection = new SqlConnection(Properties.Resources.ConnectionString);
        List<Address> addresses = new List<Address>();

        public IActionResult Index()
        {
            FetchData();
            return View(addresses); //addresses obje list'ini View'a gönderiyoruz.
        }

        private void FetchData()
        {
            if (addresses.Count > 0)
            {
                //Duplication'ı engellemek için fetch'in başında eski kayıtları temizliyoruz.
                addresses.Clear();
            }
            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT TOP (1000) [AddressID], [AddressLine1], [City], [StateProvinceID], [PostalCode], [SpatialLocation], [rowguid], [ModifiedDate] FROM [AdventureWorks2019].[Person].[Address]";
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    addresses.Add(new Address() {
                        AddressID = dataReader["AddressID"].ToString(),
                        AddressLine = dataReader["AddressLine1"].ToString(),
                        City = dataReader["City"].ToString(),
                        StateProvinceID = dataReader["StateProvinceID"].ToString(),
                        PostalCode = dataReader["PostalCode"].ToString(),
                        SpatialLocation = dataReader["SpatialLocation"].ToString(),
                        RowID = dataReader["rowguid"].ToString(),
                        ModifiedDate = dataReader["ModifiedDate"].ToString(),
                    });
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
