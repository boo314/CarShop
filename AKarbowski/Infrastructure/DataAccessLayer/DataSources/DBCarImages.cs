using AKarbowski.DataAccessLayer;
using AKarbowski.Infrastructure.Constans;
using AKarbowski.Infrastructure.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AKarbowski.Infrastructure.DataAccessLayer.DataSources
{
    public class DBCarImages
    {
        public ResultTypes RemoveCarWithImages(int carId)
        {
            using (var dbConnection = new DBConnection())
            {
                try
                {
                    dbConnection.PrepearProcedureCall("RemoveCar");
                    dbConnection.AddParameter("carId", MySqlDbType.Int32, carId);
                    dbConnection.AddParameter("result", MySqlDbType.Int32, null,ParameterDirection.Output);
                    var carResult = dbConnection.ExecuteCommandInsert();

                    dbConnection.PrepearProcedureCall("RemoveImage");
                    dbConnection.AddParameter("carId", MySqlDbType.Int32, carId);
                    dbConnection.AddParameter("result", MySqlDbType.Int32, null, ParameterDirection.Output);
                    var imageResult = dbConnection.ExecuteCommandInsert();

                    dbConnection.PrepearProcedureCall("RemoveCarsImages");
                    dbConnection.AddParameter("carId", MySqlDbType.Int32, carId);
                    dbConnection.AddParameter("result", MySqlDbType.Int32, null, ParameterDirection.Output);
                    var carImageResult = dbConnection.ExecuteCommandInsert();

                    dbConnection.Commit();
                }
                catch (Exception ex)
                {
                    dbConnection.RollBack();
                    return ResultTypes.Failed;
                }
            }
            return ResultTypes.Ok;
        }

        public ResultTypes AddCarWithImages(CarModel carModel, List<ImageModel> imageModels)
        {
            using (var dbConnection = new DBConnection())
            {
                try
                {
                    dbConnection.PrepearProcedureCall("InsertCar");
                    var carRequestParameters = GetSqlParametersFromCarModel(carModel);
                    dbConnection.AddParameters(carRequestParameters);
                    var carID = dbConnection.ExecuteCommandInsert();

                    var imageResponses = new List<int>();
                    foreach (var item in imageModels)
                    {
                        dbConnection.PrepearProcedureCall("InsertImage");
                        var imageRequestParameter = GetSqlParametersFromImageModel(item);
                        dbConnection.AddParameters(imageRequestParameter);
                        var imageResponse = dbConnection.ExecuteCommandInsert();
                        imageResponses.Add(imageResponse);
                    }

                    foreach (var item in imageResponses)
                    {
                        dbConnection.PrepearProcedureCall("InsertCarImage");
                        dbConnection.AddParameter("carId",MySqlDbType.Int32, carID);
                        dbConnection.AddParameter("imageId", MySqlDbType.Int32, item);
                        dbConnection.AddParameter("rCode", MySqlDbType.VarChar, Guid.NewGuid().ToString());
                        dbConnection.AddParameter("result", MySqlDbType.Int32, null,ParameterDirection.Output);
                        var imageResponse = dbConnection.ExecuteCommandInsert();
                    }

                    dbConnection.Commit();
                }
                catch (Exception ex)
                {
                    dbConnection.RollBack();
                    return ResultTypes.Failed;
                }
            }

            return ResultTypes.Ok;
        }

        private List<MySqlParameter> GetSqlParametersFromCarModel(CarModel model)
        {
            var result = new List<MySqlParameter>();

            result.AddRange(new[] {
                new MySqlParameter("@title", MySqlDbType.VarChar) { Value = model.Title, Size = 200, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@description", MySqlDbType.VarChar) { Value = model.Description , Size = 200, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@brand", MySqlDbType.VarChar) { Value = model.Brand , Size = 45, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@model", MySqlDbType.VarChar) { Value = model.Model , Size = 45, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@categoryID", MySqlDbType.VarChar) { Value = model.CarCategory.CarCategoryId , Size = 11 , Direction = ParameterDirection.Input}
                ,new MySqlParameter("@version", MySqlDbType.VarChar) { Value = model.Version, Size = 45, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@yearOfProduction", MySqlDbType.VarChar) { Value = model.YearOfProduction, Size = 4, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@mileage", MySqlDbType.VarChar) { Value = model.Mileage, Size = 10, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@engineCapacity", MySqlDbType.VarChar) { Value = model.EngineCapacity, Size = 10, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@fuelType", MySqlDbType.VarChar) { Value = model.FuelType, Size = 10, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@amountOfDoors", MySqlDbType.VarChar) { Value = model.AmountOfDoors, Size = 10, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@amountOfSeats", MySqlDbType.VarChar) { Value = model.AmountOfSeats, Size = 10, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@enginePower", MySqlDbType.VarChar) { Value = model.EnginePower, Size = 10, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@transmission", MySqlDbType.VarChar) { Value = model.Transmission, Size = 10, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@carType", MySqlDbType.VarChar) { Value = model.CarType, Size = 10, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@driveType", MySqlDbType.VarChar) { Value = model.DriveType, Size = 10, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@color", MySqlDbType.VarChar) { Value = model.Color, Size = 50, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@carCode", MySqlDbType.VarChar) { Value = model.CarCode, Size = 20, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@result", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            });

            return result;
        }

        private List<MySqlParameter> GetSqlParametersFromImageModel(ImageModel model)
        {
            var result = new List<MySqlParameter>();

            result.AddRange(new[] 
            {
                new MySqlParameter("@imageName", MySqlDbType.VarChar) { Value = model.ImageName, Size = 300, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@typeId", MySqlDbType.VarChar) { Value = model.Type.ImageTypeId, Size = 11, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@imagePath", MySqlDbType.VarChar) { Value = model.ImagePath, Size = 500, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@mimeType", MySqlDbType.VarChar) { Value = model.MimeType, Size = 100, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@imageCode", MySqlDbType.VarChar) { Value = model.ImageCode, Size = 20, Direction = ParameterDirection.Input }
                ,new MySqlParameter("@result", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            });

            return result;
        }
        
    }
}