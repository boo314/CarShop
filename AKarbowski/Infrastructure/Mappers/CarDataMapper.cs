using AKarbowski.Infrastructure.Models;
using AKarbowski.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace AKarbowski.Infrastructure.Mappers
{
    public class CarDataMapper
    {
        private readonly string _filesPath;
        public CarDataMapper()
        {
            _filesPath = HttpContext.Current.Server.MapPath("~/App_Data/Car/Images");

        }

        public CarDetailsViewModel GetCarDetailsViewModelFromDBResponse(DataSet response)
        {
            var images = new List<ImageViewModel>();
            var data = response.Tables[0].Rows[0];
            var title = (string)data["Title"];
            var description = (string)data["Description"];
            var brand = (string)data["Brand"];
            var model = (string)data["Model"];
            var version = (string)data["Version"];
            var yearOfProduction = Convert.ToInt32(data["YearOfProduction"]);
            var mileage = Convert.ToInt32(data["Mileage"]);
            var engineCapacity = Convert.ToInt32(data["EngineCapacity"]);
            var enginePower = Convert.ToInt32(data["EnginePower"]);
            var fuelType = (string)data["FuelType"];
            var amountOfDoors = Convert.ToInt32(data["AmountOfDoors"]);
            var amountOfSeats = Convert.ToInt32(data["AmountOfSeats"]);
            var color = (string)data["Color"];
           // var type = (string)data["Type"];
            var carId = Convert.ToInt32(data["CarID"]);
            var carCode = (string)data["CarCode"];


            foreach (DataRow item in response.Tables[0].Rows)
            {
                var imageBase64 = GetImageAsBase64((string)item["MIMEType"], (string)item["ImagePath"]);
                var alt = brand + " " + model;
                var id = Convert.ToInt32(item["ImageId"]);
                var imageTitle = brand + " " + model;
                var tempModel = new ImageViewModel
                {
                    ImageBase64 = imageBase64,
                    Alt = alt,
                    ImageId = id,
                    Title = imageTitle
                };
                images.Add(tempModel);
            }


            var result = new CarDetailsViewModel
            {
                Images = images,
                AmountOfDoors = amountOfDoors,
                AmountOfSeats = amountOfSeats,
                Brand = brand,
                CarCode = carCode,
                CarId = carId,
                Category = "",
                Color = color,
                Description = description,
                EngineCapacity = engineCapacity,
                EnginePower = enginePower,
                FuelType = fuelType,
                Mileage = mileage,
                Model = model,
                Title = title,
                Version = version,
                YearOfProduction = yearOfProduction

            };

            return result;
        }

        public List<CarListItemViewModel> GetCarListItemViewModelsFromDBResponse(DataSet response)
        {
            var result = new List<CarListItemViewModel>();

            foreach (DataRow item in response.Tables[0].Rows)
            {
                var model = GetCarListItemFromDataRow(item);
                result.Add(model);
            }

            return result;
        }

        private CarListItemViewModel GetCarListItemFromDataRow(DataRow data)
        {
            try
            {
                var fileBase64 = GetImageAsBase64((string)data["MIMEType"], (string)data["ImagePath"]);
                var result = new CarListItemViewModel
                {
                    Brand = (string)data["Brand"],
                    CarCode = (string)data["CarCode"],
                    Description = (string)data["Description"],
                    Model = (string)data["Model"],
                    CarId = Convert.ToInt32(data["CarID"]),
                    Title = (string)data["Title"],
                    ThumbnailBase64 = fileBase64
                };
                return result;
            }
            catch (Exception ex)
            {

                return new CarListItemViewModel();
            }
        }

        public CarModel GetCarModelFromCarViewModel(CarAddViewModel viewModel)
        {
            var result = new CarModel
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                AmountOfDoors = viewModel.AmountOfDoors,
                AmountOfSeats = viewModel.AmountOfSeats,
                Brand = viewModel.Brand,
                CarCategory = new CarCategoryModel { CarCategoryId = viewModel.CategoryID },
                CarCode = Guid.NewGuid().ToString(),
                CarType = "",
                Color = viewModel.Color,
                DriveType = "",
                EngineCapacity = viewModel.EngineCapacity,
                EnginePower = viewModel.EnginePower,
                FuelType = viewModel.FuelType,
                Mileage = viewModel.Mileage,
                Model = viewModel.Model,
                Transmission = "",
                Version = viewModel.Version,
                YearOfProduction = viewModel.YearOfProduction
            };
            return result;
        }

        public List<ImageModel> GetImageModelsFromHttpPostedFileBases(string carCode, HttpPostedFileBase thumb, IEnumerable<HttpPostedFileBase> images)
        {
            var result = new List<ImageModel>();

            result.Add(GetImageModelFromHttpPostedFileBase(carCode, thumb, 2));

            foreach (var item in images)
            {
                result.Add(GetImageModelFromHttpPostedFileBase(carCode, item, 1));
            }

            return result;
        }

        public ImageModel GetImageModelFromHttpPostedFileBase(string carCode, HttpPostedFileBase image, int imageType)
        {
            var thumbCode = Guid.NewGuid().ToString();
            var extension = image.FileName.Split('.');
            var result = new ImageModel
            {
                ImageName = image.FileName,
                ImageCode = thumbCode,
                Type = new ImageTypeModel { ImageTypeId = imageType },
                MimeType = image.ContentType,
                ImagePath = carCode + "\\" + thumbCode + "." + extension.Last()
            };
            return result;
        }

        private string GetImageAsBase64(string mimeType, string imagePath)
        {
            var path = Path.Combine(_filesPath, imagePath);
            var file = File.ReadAllBytes(path);
            var fileBase64 = "data:" + mimeType + ";base64," + Convert.ToBase64String(file);
            return fileBase64;
        }
    }
}