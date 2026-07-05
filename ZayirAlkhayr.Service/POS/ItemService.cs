using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Entities.Specifications.Items;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.POS;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.POS
{
    public class ItemService: IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppSettings _appSettings;
        private readonly IManageFileService _manageFileService;
        private readonly IWebHostEnvironment _environment;
        private string ApiLocalUrl;

        public ItemService(IUnitOfWork unitOfWork, IManageFileService manageFileService, IAppSettings appSettings, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings;
            _manageFileService = manageFileService;
            ApiLocalUrl = _appSettings.ApiUrlLocal;
            _environment = environment;
        }

        public async Task<ApiResponseModel<List<ProductsResponseDto>>> GetAllProducts(PagingFilterModel Model)
        {
            var DataSpec = new ItemSpecification(Model);
            var CountSpec = new ItemSpecification(Model, false);
            var results = await _unitOfWork.Repository<Item>().GetAllWithSpecAsync(DataSpec);
            var Count = await _unitOfWork.Repository<Item>().GetCountAsync(CountSpec);
            var data = results.Select(i => new ProductsResponseDto
            {
                ProductId = i.ItemId,
                CategoryId = i.CategoryId,
                ProductName = i.Name,
                ProductNameEn = i.NameEn,
                CategoryName = i.Category.Name,
                Image = Path.Combine(ApiLocalUrl, "Images", ImageFiles.Items.ToString(), i.Image ?? string.Empty),
                Price = i.Price,
                CostPrice = i.CostPrice
            }).ToList();
            return ApiResponseModel<List<ProductsResponseDto>>.Success(GenericErrors.GetSuccess, data, Count);
        }

        public async Task<ApiResponseModel<string>> AddNewProduct(Item Model)
        {
            try
            {
                if (Model.Files != null)
                {
                    var FileName = await _manageFileService.UploadFile(Model.Files, "", ImageFiles.Items);
                    if (FileName.IsSuccess)
                        Model.Image = FileName.Results;
                    else
                        return FileName;
                }
                Model.InsertDate = DateTime.Now;
                await _unitOfWork.Repository<Item>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateProduct(Item Model)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<Item>().GetByIdAsync(Model.ItemId);
                if (Entity != null)
                {
                    Entity.Name = Model.Name;
                    Entity.NameEn = Model.NameEn;
                    Entity.Price = Model.Price;
                    Entity.CategoryId = Model.CategoryId;
                    Entity.CostPrice = Model.CostPrice;
                    Entity.UpdateUser = Model.InsertUser;
                    Entity.UpdateDate = DateTime.Now;
                    if (Model.Files != null)
                    {
                        var FileName = await _manageFileService.UploadFile(Model.Files, Model.OldFileName, ImageFiles.Items);
                        if (FileName.IsSuccess)
                            Entity.Image = FileName.Results;
                        else
                            return FileName;
                    }
                    await _unitOfWork.CompleteAsync();
                    return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
                }

                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ApiResponseModel<string>> DeleteProduct(int ProductId)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<Item>().GetByIdAsync(ProductId);
                if (Entity != null)
                {
                    var hasRecipe = await _unitOfWork.Repository<ItemRecipe>()
                        .AnyAsync(i => i.ItemId == ProductId);

                    if (hasRecipe)
                        return ApiResponseModel<string>.Failure(GenericErrors.DeleteRelationRow);

                    _unitOfWork.Repository<Item>().Delete(Entity);
                    await _unitOfWork.CompleteAsync();
                    DeleteCategoryFile(Entity.Image);
                    return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
                }

                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ApiResponseModel<List<Item>>> GetProductsByCategoryId(int CategoryId)
        {
            var results = await _unitOfWork.Repository<Item>().FindAsync(p => p.CategoryId == CategoryId);
            var data = results.Select(i => new Item
            {
                ItemId = i.ItemId,
                CategoryId = i.CategoryId,
                Name = i.Name,
                NameEn = i.NameEn,
                Price = i.Price,
                Image = Path.Combine(ApiLocalUrl, "Images", ImageFiles.Items.ToString(), i.Image ?? string.Empty)
            }).ToList();
            return ApiResponseModel<List<Item>>.Success(GenericErrors.GetSuccess, data);
        }

        private void DeleteCategoryFile(string ItemImageName)
        {
            var ItemImagePaths = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "Images", ImageFiles.Items.ToString()));

            if (ItemImagePaths.Count() > 0)
            {
                var File = ItemImagePaths.FirstOrDefault(i => i.Contains(ItemImageName));
                if (File != null)
                    System.IO.File.Delete(File);
            }
        }
    }
}
