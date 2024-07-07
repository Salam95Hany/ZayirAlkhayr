using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Service;

namespace ZayirAlkhayr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("GetAllPhotos")]
        public DataTable GetAllPhotos(PagingFilterModel PagingFilter)
        {
            var results = _photoService.GetAllPhotos(PagingFilter);
            return results;
        }

        [HttpGet("GetPhotoDetails")]
        public List<PhotoDetails> GetPhotoDetails(int PhotoId)
        {
            var results = _photoService.GetPhotoDetails(PhotoId);
            return results;
        }

        [HttpGet("GetPhotoWithDetailsById")]
        public PhotoModel GetPhotoWithDetailsById(int PhotoId)
        {
            var results = _photoService.GetPhotoWithDetailsById(PhotoId);
            return results;
        }

        [HttpPost("AddNewPhoto")]
        public async Task<HandleErrorResponseModel> AddNewPhoto([FromForm] Photos Model)
        {
            var results = await _photoService.AddNewPhoto(Model);
            return results;
        }

        [HttpPost("UpdatePhoto")]
        public async Task<HandleErrorResponseModel> UpdatePhoto([FromForm] Photos Model)
        {
            var results = await _photoService.UpdatePhoto(Model);
            return results;
        }

        [HttpGet("DeletePhoto")]
        public HandleErrorResponseModel DeletePhoto(int PhotoId)
        {
            var results = _photoService.DeletePhoto(PhotoId);
            return results;
        }

        [HttpPost("AddPhotoDetailsImage")]
        public async Task<HandleErrorResponseModel> AddPhotoDetailsImage([FromForm] UploadFileModel Model)
        {
            var results = await _photoService.AddPhotoDetailsImage(Model);
            return results;
        }

        [HttpGet("DeletePhotoDetailsImage")]
        public HandleErrorResponseModel DeletePhotoDetailsImage(string FileName, int Id)
        {
            var results = _photoService.DeletePhotoDetailsImage(FileName, Id);
            return results;
        }
    }
}
