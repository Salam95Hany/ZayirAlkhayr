using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.GeneralServices;

namespace ZayirAlkhayr.Service.GeneralServices
{
    public class OrphansService : IOrphansService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        public OrphansService(ZADbContext context, ISQLHelper sQLHelper)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
        }

        public List<OrphansDetailsModel> GetAllFamilyStatusOrphansType(string SearchText)
        {
            var Params = new SqlParameter[1];
            Params[0] = new SqlParameter("@SearchText", SearchText);
            var Model = _sQLHelper.SQLQuery<OrphansMappingModel>("admin.SP_GetAllFamilyStatusOrphansType", Params);
            var Data = Model.GroupBy(i => i.FamilyStatusId).Select(i => new OrphansDetailsModel
            {
                FamilyStatusId = i.Key,
                FamilyStatusName = i.FirstOrDefault().FamilyStatusName,
                OrphansDetails = i.Select(x => new OrphansDetails
                {
                    FamilyDetailsId = x.FamilyDetailsId,
                    FamilyDetailsName = x.FamilyDetailsName
                }).ToList()
            }).ToList();

            return Data;
        }

        public DataTable GetAllOrphansData(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllOrphansDataWithFilters", Params);
            return dt;
        }

        public List<FilterModel> GetAllOrphansFilters(PagingFilterModel PagingFilter)
        {
            var FilterDt = _sQLHelper.ConvertFilterModelToDataTable(PagingFilter.FilterList);
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            var dt = _sQLHelper.ExecuteDataTable("admin.SP_GetAllOrphansDataWithFilters", Params);
            var Filters = _sQLHelper.GroupingFilters(dt);
            return Filters;
        }

        public HandleErrorResponseModel AddNewOrphans(Orphans Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Lookups = GetFamilyOrphanLookups(Model.FamilyStatusId, Model.FamilyDetailsId);
                var OrphansObj = new Orphans();
                OrphansObj.FamilyStatusId = Model.FamilyStatusId;
                OrphansObj.FamilyDetailsId = Model.FamilyDetailsId;
                OrphansObj.Name = Lookups.OrphanDetails.Name;
                OrphansObj.DateOfBirth = Model.DateOfBirth;
                OrphansObj.FamilyName = Lookups.FamilyStatus.Name;
                OrphansObj.Jop = Lookups.OrphanDetails.Jop;
                OrphansObj.NationalId = Lookups.OrphanDetails.NationalId;
                OrphansObj.Phone = Lookups.FamilyStatus.Phone;
                OrphansObj.Address = Lookups.FamilyStatus.Address;
                OrphansObj.Income = Lookups.FamilyIncome.TotalFamilyIncome.ToString();
                OrphansObj.AcademicStage = Lookups.OrphanDetails.Education;
                OrphansObj.FamilyMembersCount = Lookups.FamilyMembersCount.ToString();
                OrphansObj.RankingBrothers = Model.RankingBrothers;
                OrphansObj.HealthStatus = Lookups.FamilyPatient?.Specialization;
                OrphansObj.NationalityId = Lookups.FamilyStatus.NationalityId;
                OrphansObj.Notes = Model.Notes;
                OrphansObj.IsGuaranteed = false;
                OrphansObj.InsertUser = Model.InsertUser;
                OrphansObj.InsertDate = DateTime.Now.AddHours(1);

                _Context.Orphans.Add(OrphansObj);
                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم اضافة يتيم جديد بنجاح";
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

        public HandleErrorResponseModel UpdateOrphans(Orphans Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Lookups = GetFamilyOrphanLookups(Model.FamilyStatusId, Model.FamilyDetailsId);
                var OrphansObj = _Context.Orphans.FirstOrDefault(x => x.Id == Model.Id);
                OrphansObj.FamilyStatusId = Model.FamilyStatusId;
                OrphansObj.FamilyDetailsId = Model.FamilyDetailsId;
                OrphansObj.Name = Lookups.OrphanDetails.Name;
                OrphansObj.DateOfBirth = Model.DateOfBirth;
                OrphansObj.FamilyName = Lookups.FamilyStatus.Name;
                OrphansObj.Jop = Lookups.OrphanDetails.Jop;
                OrphansObj.NationalId = Lookups.OrphanDetails.NationalId;
                OrphansObj.Phone = Lookups.FamilyStatus.Phone;
                OrphansObj.Address = Lookups.FamilyStatus.Address;
                OrphansObj.Income = Lookups.FamilyIncome.TotalFamilyIncome.ToString();
                OrphansObj.AcademicStage = Lookups.OrphanDetails.Education;
                OrphansObj.FamilyMembersCount = Lookups.FamilyMembersCount.ToString();
                OrphansObj.RankingBrothers = Model.RankingBrothers;
                OrphansObj.HealthStatus = Lookups.FamilyPatient?.Specialization;
                OrphansObj.NationalityId = Lookups.FamilyStatus.NationalityId;
                OrphansObj.Notes = Model.Notes;
                OrphansObj.IsGuaranteed = Model.IsGuaranteed;
                OrphansObj.UpdateUser = Model.InsertUser;
                OrphansObj.UpdateDate = DateTime.Now.AddHours(1);

                _Context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم تعديل العنصر بنجاح";
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

        public HandleErrorResponseModel DeleteOrphans(int OrphansId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Orphans = _Context.Orphans.FirstOrDefault(i => i.Id == OrphansId);

                _Context.Orphans.Remove(Orphans);
                _Context.SaveChanges();
                Response.Done = true;
                Response.Message = "تم حذف العنصر بنجاح";
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

        public HandleErrorResponseModel AddUpdateBenefactorOrphans(BeneFactorOrphansModel Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Orphans = _Context.Orphans.FirstOrDefault(i => i.Id == Model.OrphansId);
                if (Orphans != null)
                {
                    Orphans.BenefactorId = Model.BenefactorId;
                    Orphans.BenefactorAddress = Model.BenefactorAddress;
                    Orphans.BenefactorPhone = Model.BenefactorPhone;
                    Orphans.BenefactorType = Model.BenefactorType;
                    Orphans.IsGuaranteed = true;

                    _Context.SaveChanges();

                    if (!Model.IsGuaranteed)
                    {
                        Response.Done = true;
                        Response.Message = "تم اضافة كفيل جديد بنجاح";
                        return Response;
                    }
                    else
                    {
                        Response.Done = true;
                        Response.Message = "تم تعديل الكفيل بنجاح";
                        return Response;
                    }
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "لقد حدث خطا";
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

        public HandleErrorResponseModel DeleteBenefactorOrphans(int OrphansId)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var Orphans = _Context.Orphans.FirstOrDefault(i => i.Id == OrphansId);
                if (Orphans != null)
                {
                    Orphans.BenefactorId = null;
                    Orphans.BenefactorAddress = null;
                    Orphans.BenefactorPhone = null;
                    Orphans.BenefactorType = null;
                    Orphans.IsGuaranteed = false;

                    _Context.SaveChanges();
                    Response.Done = true;
                    Response.Message = "تم حذف الكفيل بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "لقد حدث خطا";
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

        public List<OrphansDetails> GetOrphanDetailsByFamilyId(int FamilyStatusId)
        {
            var results = _Context.FamilyDetails.Where(i => i.FamilyStatusId == FamilyStatusId).Select(i => new OrphansDetails
            {
                FamilyDetailsId = i.Id,
                FamilyDetailsName = i.Name
            }).ToList();

            return results;
        }

        public FamilyOrphanLookups GetFamilyOrphanLookups(int FamilyStatusId,int FamilyDetailsId)
        {
            var PersonTypes = new List<string> { "ابنة", "إبنة", "ابنه", "ابن", "إبنه", "إبن" };
            var FamilyStatusTask = _Context.FamilyStatus.FirstOrDefault(i => i.Id == FamilyStatusId);
            var FamilyDetailsTask = _Context.FamilyDetails.Where(i => i.FamilyStatusId == FamilyStatusId).ToList();
            var FamilyIncomeTask = _Context.FamilyIncome.FirstOrDefault(i => i.FamilyStatusId == FamilyStatusId);
            var FamilyMembersCount = FamilyDetailsTask.Where(i => PersonTypes.Contains(i.Relevance)).Count() + 1;
            var OrphanObj = FamilyDetailsTask.FirstOrDefault(i => i.Id == FamilyDetailsId);
            var FamilyPatientObj = _Context.FamilyPatient.FirstOrDefault(i => i.Name == OrphanObj.Name);

            return new FamilyOrphanLookups
            {
                FamilyStatus = FamilyStatusTask,
                FamilyDetails = FamilyDetailsTask,
                FamilyIncome = FamilyIncomeTask,
                OrphanDetails = OrphanObj,
                FamilyPatient = FamilyPatientObj,
                FamilyMembersCount = FamilyMembersCount
            };
        }
    }
}
