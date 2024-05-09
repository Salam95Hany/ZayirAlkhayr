using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Interface.Common;

namespace ZayirAlkhayr.Service
{
    public class PhotoService : IPhotoService
    {
        private readonly ZADbContext _Context;
        private readonly IManageFileService _manageFileService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private string ApiLocalUrl;
        public PhotoService(ZADbContext Context, IManageFileService manageFileService, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _Context = Context;
            _manageFileService = manageFileService;
            _configuration = configuration;
            _environment = environment;
            ApiLocalUrl = _configuration["ApiUrlLocal"];
        }

        public List<Photos> GetAllPhotos()
        {
            var results = _Context.Photos.Select(i => new Photos
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Image = Path.Combine(ApiLocalUrl, ImageFiles.PhotoImages.ToString(), i.Image)
            }).ToList();
            return results;
        }

        public List<PhotoDetails> GetPhotoDetails(int PhotoId)
        {
            var results = _Context.PhotoDetails.Where(i => i.PhotoId == PhotoId).Select(i => new PhotoDetails
            {
                Id = i.Id,
                Image = Path.Combine(ApiLocalUrl, ImageFiles.PhotoDetailImages.ToString(), i.Image)
            }).ToList();
            return results;
        }

        public PhotoModel GetPhotoWithDetailsById(int PhotoId)
        {
            var Photo = _Context.Photos.FirstOrDefault(i => i.Id == PhotoId);
            if (Photo == null) { return new PhotoModel(); }
            var PhotoDetalImages = _Context.PhotoDetails.Where(i => i.PhotoId == PhotoId).Select(i => Path.Combine(ApiLocalUrl, ImageFiles.PhotoDetailImages.ToString(), i.Image)).ToList();

            var PhotoModel = new PhotoModel
            {
                Id = Photo.Id,
                Title = Photo.Title,
                Description = Photo.Description,
                Image = Path.Combine(ApiLocalUrl, ImageFiles.PhotoImages.ToString(), Photo.Image),
                DetailImages = PhotoDetalImages
            };
            return PhotoModel;
        }

        public async Task<HandleErrorResponseModel> AddNewPhoto(Photos Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var PhotoObj = new Photos();
                PhotoObj.Title = Model.Title;
                PhotoObj.Description = Model.Description;
                PhotoObj.IsVisible = Model.IsVisible;
                PhotoObj.InsertUser = Model.InsertUser;
                PhotoObj.InsertDate = DateTime.Now;

                var FileName = await _manageFileService.UploadFile(Model.File, "", ImageFiles.PhotoImages);
                if (FileName.Done)
                    PhotoObj.Image = FileName.StringValue;
                else
                    return FileName;

                _Context.Photos.Add(PhotoObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة صورة جديدة بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public async Task<HandleErrorResponseModel> UpdatePhoto(Photos Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var PhotoObj = _Context.Photos.FirstOrDefault(x => x.Id == Model.Id);
                PhotoObj.Title = Model.Title;
                PhotoObj.Description = Model.Description;
                PhotoObj.IsVisible = Model.IsVisible;
                PhotoObj.UpdateUser = Model.UpdateUser;
                PhotoObj.UpdateDate = DateTime.Now;

                if (Model.File != null)
                {
                    var FileName = await _manageFileService.UploadFile(Model.File, Model.OldFileName, ImageFiles.PhotoDetailImages);
                    if (FileName.Done)
                        PhotoObj.Image = FileName.StringValue;
                    else
                        return FileName;
                }

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل الصورة بنجاح";
                return Response;
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel DeletePhoto(int PhotoId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Photo = _Context.Photos.FirstOrDefault(i => i.Id == PhotoId);
                if (Photo != null)
                {
                    var DetailImages = _Context.PhotoDetails.Where(i => i.PhotoId == PhotoId).ToList();
                    if (DetailImages.Count > 0)
                        _Context.PhotoDetails.RemoveRange(DetailImages);

                    _Context.Photos.Remove(Photo);
                    var PhotoDetailImageNames = DetailImages.Select(i => i.Image).ToList();
                    DeletePhotoFiles(Photo.Image, PhotoDetailImageNames);
                    _Context.SaveChanges();
                    Response.Done = true;
                    Response.Message = "تم حذف الصورة بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "هذه الصورة غير موجود";
                    return Response;
                }

            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public async Task<HandleErrorResponseModel> AddPhotoDetailsImage(IFormFile File, int PhotoId)
        {
            var FileName = await _manageFileService.UploadFile(File, "", ImageFiles.PhotoDetailImages);
            if (FileName.Done)
            {
                var Photo = new PhotoDetails();
                Photo.PhotoId = PhotoId;
                Photo.Image = FileName.StringValue;
                _Context.PhotoDetails.Add(Photo);
                _Context.SaveChanges();
                return FileName;
            }
            else
                return FileName;
        }

        public HandleErrorResponseModel DeletePhotoDetailsImage(string FileName, int Id)
        {
            var Photo = _Context.PhotoDetails.FirstOrDefault(a => a.Id == Id);
            if (Photo != null)
            {
                var File = _manageFileService.DeleteFile(FileName, ImageFiles.PhotoDetailImages);
                if (File.Done)
                {
                    _Context.PhotoDetails.Remove(Photo);
                    _Context.SaveChanges();
                    return File;
                }
            }

            return new HandleErrorResponseModel() { Done = false, Message = "لقد حدث خطا" };
        }

        private void DeletePhotoFiles(string PhotoImageName, List<string> PhotoDetailImageNames)
        {
            var PhotoImagePaths = Directory.GetFiles(Path.Combine(_environment.WebRootPath, ImageFiles.PhotoImages.ToString()));
            var PhotoDetailImagePaths = Directory.GetFiles(Path.Combine(_environment.WebRootPath, ImageFiles.PhotoDetailImages.ToString()));

            if (PhotoImagePaths.Count() > 0)
            {
                var File = PhotoImagePaths.FirstOrDefault(i => i.Contains(PhotoImageName));
                if (File != null)
                    System.IO.File.Delete(File);
            }

            if (PhotoDetailImagePaths.Count() > 0)
            {
                var Files = PhotoDetailImagePaths.Where(i => PhotoDetailImageNames.Any(x => i.Contains(x))).ToList();
                if (Files.Count() > 0)
                {
                    Files.ForEach(i => System.IO.File.Delete(i));
                }
            }
        }
    }
}
