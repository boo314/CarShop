using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Web;
using AKarbowski.Infrastructure.Constans;

namespace AKarbowski.DataAccessLayer
{
    public class DBCars
    {
        public DataSet GetCars()
        {
            var result = new DataSet();
            try
            {
                using (var dbConnection = new DBConnection())
                {
                    dbConnection.PrepearProcedureCall("GetCars");
                    dbConnection.AddParameter("imageTypeID", MySqlDbType.Int32, 2);
                    result = dbConnection.ExecuteCommandFill();
                }
            }
            catch (Exception ex)
            {

                return new DataSet();
            }
            

            return result;
        }

        public DataSet GetCarById(int carId)
        {
            var result = new DataSet();
            try
            {
                using (var dbConnection = new DBConnection())
                {
                    dbConnection.PrepearProcedureCall("GetCarById");
                    dbConnection.AddParameter("carId", MySqlDbType.Int32, carId);
                    result = dbConnection.ExecuteCommandFill();

                }
            }
            catch (Exception ex)
            {

                return new DataSet();
            }
            return result;
        }
    }
}