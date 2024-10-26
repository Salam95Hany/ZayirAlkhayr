using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.GeneralServices;

namespace ZayirAlkhayr.Service.GeneralServices
{
    public class UpdateFamilyStatusService : IUpdateFamilyStatusService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        public UpdateFamilyStatusService(ZADbContext context, ISQLHelper sQLHelper)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
        }

        public HandleErrorResponseModel UpdateFamilyStatus(AddFamilyStatusModel Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var FamilyObj = _Context.FamilyStatus.FirstOrDefault(i => i.Id == Model.FamilyStatus.Id);
                FamilyObj.StatusTypeId = Model.FamilyStatus.StatusTypeId;
                FamilyObj.CategoryId = Model.FamilyStatus.CategoryId;
                FamilyObj.NationalityId = Model.FamilyStatus.NationalityId;
                FamilyObj.Name = Model.FamilyStatus.Name;
                FamilyObj.Fname = Model.FamilyStatus.Fname;
                FamilyObj.Address = Model.FamilyStatus.Address;
                FamilyObj.NationalId = Model.FamilyStatus.NationalId;
                FamilyObj.Village = Model.FamilyStatus.Village;
                FamilyObj.Center = Model.FamilyStatus.Center;
                FamilyObj.Governorate = Model.FamilyStatus.Governorate;
                FamilyObj.Phone = Model.FamilyStatus.Phone;
                FamilyObj.Phone1 = Model.FamilyStatus.Phone1;
                FamilyObj.SupportingParty = Model.FamilyStatus.SupportingParty;
                FamilyObj.ReasonOfRefuse = Model.FamilyStatus.ReasonOfRefuse;
                FamilyObj.Relevance = Model.FamilyStatus.Relevance;
                FamilyObj.Age = Model.FamilyStatus.Age;
                FamilyObj.MaritalStatus = Model.FamilyStatus.MaritalStatus;
                FamilyObj.Education = Model.FamilyStatus.Education;
                FamilyObj.Jop = Model.FamilyStatus.Jop;
                FamilyObj.UpdateUser = Model.FamilyStatus.InsertUser;
                FamilyObj.UpdateDate = DateTime.Now.AddHours(1);

                UpdateFamilyIncome(Model.FamilyIncome, Model.FamilyStatus.Id);
                UpdateFamilyExpenses(Model.FamilyExpenses, Model.FamilyStatus.Id);
                UpdateFamilyExtraDetails(Model.FamilyExtraDetails, Model.FamilyStatus.Id);
                UpdateFamilyDetails(Model.FamilyDetails, Model.FamilyStatus.Id);
                UpdateFamilyPatient(Model.FamilyPatient, Model.FamilyStatus.Id);
                UpdateFamilyNeeds(Model.FamilyNeeds, Model.FamilyStatus.Id);
                _Context.SaveChanges();
                Response.Done = true;
                Response.Message = "تم تعديل الحالة بنجاح";
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

        private void UpdateFamilyIncome(FamilyIncome Model, int FamilyStatusId)
        {
            var IncomeObj = _Context.FamilyIncome.FirstOrDefault(i => i.FamilyStatusId == FamilyStatusId);
            IncomeObj.FatherJop = Model.FatherJop;
            IncomeObj.MotherJop = Model.MotherJop;
            IncomeObj.ChildernsJop = Model.ChildernsJop;
            IncomeObj.AffairSpension_SocialSolidarity = Model.AffairSpension_SocialSolidarity;
            IncomeObj.Project = Model.Project;
            IncomeObj.LiveStock_Lands = Model.LiveStock_Lands;
            IncomeObj.Organization_ZakatCommittee = Model.Organization_ZakatCommittee;
            IncomeObj.InsurancePension = Model.InsurancePension;
            IncomeObj.Comments = Model.Comments;
            IncomeObj.Other = Model.Other;
            IncomeObj.TotalFamilyIncome = Model.TotalFamilyIncome;
        }

        private void UpdateFamilyExpenses(FamilyExpenses Model, int FamilyStatusId)
        {
            var ExpensesObj = _Context.FamilyExpenses.FirstOrDefault(i => i.FamilyStatusId == FamilyStatusId);
            ExpensesObj.Rent_Electricity_Water_Gas_Sewage = Model.Rent_Electricity_Water_Gas_Sewage;
            ExpensesObj.MedicalExamination_Treatment = Model.MedicalExamination_Treatment;
            ExpensesObj.SchoolExpenses = Model.SchoolExpenses;
            ExpensesObj.Installment_debts = Model.Installment_debts;
            ExpensesObj.PhysiotherapySessions = Model.PhysiotherapySessions;
            ExpensesObj.Analysis = Model.Analysis;
            ExpensesObj.SatisfactoryTransfers = Model.SatisfactoryTransfers;
            ExpensesObj.MedicalXRays = Model.MedicalXRays;
            ExpensesObj.IsMinisterialSupply = Model.IsMinisterialSupply.GetValueOrDefault(false);
            ExpensesObj.IsFoodBank = Model.IsFoodBank.GetValueOrDefault(false);
            ExpensesObj.TotalFamilyExpenses = Model.TotalFamilyExpenses;
            ExpensesObj.NetFamilyIncome = Model.NetFamilyIncome;
            ExpensesObj.FamilyCount = Model.FamilyCount;
            ExpensesObj.AvgPersonIncome = Model.AvgPersonIncome;
        }

        private void UpdateFamilyExtraDetails(FamilyExtraDetails Model, int FamilyStatusId)
        {
            bool isEmpty = Model.AreAllPropertiesDefault();
            var ExtraDetails = _Context.FamilyExtraDetails.FirstOrDefault(i => i.FamilyStatusId == FamilyStatusId);
            if (ExtraDetails != null)
                _Context.FamilyExtraDetails.Remove(ExtraDetails);

            if (!isEmpty)
            {
                var ExtraDetailsObj = new FamilyExtraDetails();
                ExtraDetailsObj.FamilyStatusId = FamilyStatusId;
                ExtraDetailsObj.StatusDescription = Model.StatusDescription;
                ExtraDetailsObj.HousingNeedsAndStatus = Model.HousingNeedsAndStatus;
                ExtraDetailsObj.ResearcherNotes = Model.ResearcherNotes;
                ExtraDetailsObj.ReferencesNotes = Model.ReferencesNotes;
                ExtraDetailsObj.LastVisitDate = Model.LastVisitDate;
                ExtraDetailsObj.PersonalPapers = Model.PersonalPapers;
                _Context.FamilyExtraDetails.Add(ExtraDetailsObj);
            }

        }

        private void UpdateFamilyDetails(List<FamilyDetails> Model, int FamilyStatusId)
        {
            var FamilyDetailsDb = _Context.FamilyDetails.Where(I => I.FamilyStatusId == FamilyStatusId).ToList();
            if (FamilyDetailsDb.Count > 0)
                _Context.FamilyDetails.RemoveRange(FamilyDetailsDb);

            foreach (var FamilyDetails in Model)
            {
                var FamilyDetailsObj = new FamilyDetails();
                FamilyDetailsObj.FamilyStatusId = FamilyStatusId;
                FamilyDetailsObj.Name = FamilyDetails.Name;
                FamilyDetailsObj.Relevance = FamilyDetails.Relevance;
                FamilyDetailsObj.Age = FamilyDetails.Age;
                FamilyDetailsObj.MaritalStatus = FamilyDetails.MaritalStatus;
                FamilyDetailsObj.Education = FamilyDetails.Education;
                FamilyDetailsObj.Jop = FamilyDetails.Jop;
                FamilyDetailsObj.NationalId = FamilyDetails.NationalId;
                FamilyDetailsObj.ChildernsCount = FamilyDetails.ChildernsCount;
                FamilyDetailsObj.FamilyMembersCount = FamilyDetails.FamilyMembersCount;
                _Context.FamilyDetails.Add(FamilyDetailsObj);
            }
        }

        private void UpdateFamilyPatient(List<FamilyPatientGroup> Model, int FamilyStatusId)
        {
            var FamilyPatients = _Context.FamilyPatient.Where(I => I.FamilyStatusId == FamilyStatusId).ToList();
            if (FamilyPatients.Count > 0)
                _Context.FamilyPatient.RemoveRange(FamilyPatients);

            foreach (var FamilyPatient in Model)
            {
                foreach (var typeId in FamilyPatient.PatientTypeIds)
                {
                    var FamilyPatientObj = new FamilyPatient();
                    FamilyPatientObj.FamilyStatusId = FamilyStatusId;
                    FamilyPatientObj.Name = FamilyPatient.Name;
                    FamilyPatientObj.PatientTypeId = typeId;
                    FamilyPatientObj.PatientDate = FamilyPatient.PatientDate;
                    FamilyPatientObj.Specialization = FamilyPatient.Specialization;
                    FamilyPatientObj.IsMedicalReport = FamilyPatient.IsMedicalReport.GetValueOrDefault(false);
                    FamilyPatientObj.IsNeedProcess = FamilyPatient.IsNeedProcess.GetValueOrDefault(false);
                    _Context.FamilyPatient.Add(FamilyPatientObj);
                }
            }
        }

        private void UpdateFamilyNeeds(List<FamilyNeeds> Model, int FamilyStatusId)
        {
            var FamilyNeeds = _Context.FamilyNeeds.Where(I => I.StatusId == FamilyStatusId).ToList();
            if (FamilyNeeds.Count > 0)
                _Context.FamilyNeeds.RemoveRange(FamilyNeeds);

            foreach (var FamilyNeed in Model)
            {
                var FamilyNeedsObj = new FamilyNeeds();
                FamilyNeedsObj.StatusId = FamilyStatusId;
                FamilyNeedsObj.NeedTypeId = FamilyNeed.NeedTypeId;
                FamilyNeedsObj.IsWaiting = FamilyNeed.IsWaiting;
                FamilyNeedsObj.DeliveryDate = FamilyNeed.DeliveryDate;
                _Context.FamilyNeeds.Add(FamilyNeedsObj);
            }
        }
    }
}
