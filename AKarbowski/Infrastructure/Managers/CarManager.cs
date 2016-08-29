using AKarbowski.DataAccessLayer;
using AKarbowski.Infrastructure.Constans;
using AKarbowski.Infrastructure.DataAccessLayer.DataSources;
using AKarbowski.Infrastructure.Mappers;
using AKarbowski.Infrastructure.Models;
using AKarbowski.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AKarbowski.Infrastructure.Managers
{
    public class CarManager
    {
        private readonly CarDataMapper _mapper;
        private readonly string _filesPath;

        public CarManager()
        {
            _mapper = new CarDataMapper();
            _filesPath = HttpContext.Current.Server.MapPath("~/App_Data/Car/Images");
        }

        public IEnumerable<CarListItemViewModel> GetCars()
        {
            var dbCars = new DBCars();
            var requestResult = dbCars.GetCars();

            var result = _mapper.GetCarListItemViewModelsFromDBResponse(requestResult);

            return result;
        }

        public CarDetailsViewModel GetCarById(int carId)
        {
            var dbCars = new DBCars();
            var response = dbCars.GetCarById(carId);

            var result = _mapper.GetCarDetailsViewModelFromDBResponse(response);

            return result;
        }

        public ResultTypes RemoveCar(int carId)
        {
            var dbCarImages = new DBCarImages();
            var result = dbCarImages.RemoveCarWithImages(carId);
            
            //Add images deleting

            return result;
        }

        public ResultTypes AddCar(CarAddViewModel viewModel, HttpPostedFileBase thumb, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                viewModel.CarCode = Guid.NewGuid().ToString();
                viewModel.CategoryID = 1;

                var carModel = _mapper.GetCarModelFromCarViewModel(viewModel);
                var images = _mapper.GetImageModelsFromHttpPostedFileBases(carModel.CarCode, thumb, files);



                var dbCarImage = new DBCarImages();

                var result = dbCarImage.AddCarWithImages(carModel, images);


                switch (result)
                {
                    case ResultTypes.Ok:
                        SaveFiles(thumb, files, images);
                        return ResultTypes.Ok;
                    case ResultTypes.Failed:
                    default:
                        return ResultTypes.Failed;
                }
            }
            catch (Exception ex)
            {
                return ResultTypes.Failed;
            }
        }

        private ResultTypes SaveFiles(HttpPostedFileBase thumb, IEnumerable<HttpPostedFileBase> files, List<ImageModel> images)
        {
            try
            {
                var thumbImage = images.FirstOrDefault(x => x.Type.ImageTypeId == 2);
                var thumbPath = Path.Combine(_filesPath, thumbImage.ImagePath);
                CreatDirectoryIfNotExist(thumbImage.ImagePath);
                thumb.SaveAs(thumbPath);

                foreach (var item in files)
                {
                    var image = images.FirstOrDefault(z => z.ImageName == item.FileName);
                    var path = Path.Combine(_filesPath, image.ImagePath);
                    CreatDirectoryIfNotExist(image.ImagePath);
                    item.SaveAs(path);
                }
            }
            catch (Exception ex)
            {
                return ResultTypes.Failed;
            }
            return ResultTypes.Ok;
        }

        private void CreatDirectoryIfNotExist(string path)
        {
            var directory = path.Split('\\')[0];
            var tempPath = Path.Combine(_filesPath, directory);
            var isDirectoryExists = Directory.Exists(tempPath);
            if (!isDirectoryExists)
            {
                Directory.CreateDirectory(tempPath);
            }
        }
    }
}