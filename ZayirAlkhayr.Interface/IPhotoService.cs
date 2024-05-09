using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface
{
    public interface IPhotoService
    {
        List<Photos> GetAllPhotos();
        List<PhotoDetails> GetPhotoDetails(int PhotoId);
        PhotoModel GetPhotoWithDetailsById(int PhotoId);
        Task<HandleErrorResponseModel> AddNewPhoto(Photos Model);
        Task<HandleErrorResponseModel> UpdatePhoto(Photos Model);
        HandleErrorResponseModel DeletePhoto(int PhotoId);
        Task<HandleErrorResponseModel> AddPhotoDetailsImage(IFormFile File, int PhotoId);
        HandleErrorResponseModel DeletePhotoDetailsImage(string FileName, int Id);
    }
}
