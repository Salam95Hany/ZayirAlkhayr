using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.WebSite
{
    public interface IPhotoService
    {
        DataTable GetAllPhotos(PagingFilterModel PagingFilter);
        List<PhotoDetails> GetPhotoDetails(int PhotoId);
        PhotoModel GetPhotoWithDetailsById(int PhotoId);
        Task<HandleErrorResponseModel> AddNewPhoto(Photos Model);
        Task<HandleErrorResponseModel> UpdatePhoto(Photos Model);
        HandleErrorResponseModel DeletePhoto(int PhotoId);
        Task<HandleErrorResponseModel> AddPhotoDetailsImage(UploadFileModel Model);
        HandleErrorResponseModel DeletePhotoDetailsImage(string FileName, int Id);
        HandleErrorResponseModel ApplyPhotoFilesSorting(List<FileSortingModel> Model, int PhotoId);
    }
}
