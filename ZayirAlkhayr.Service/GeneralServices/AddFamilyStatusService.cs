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
    public class AddFamilyStatusService : IAddFamilyStatusService
    {
        private readonly ZADbContext _Context;
        private readonly ISQLHelper _sQLHelper;
        public AddFamilyStatusService(ZADbContext context, ISQLHelper sQLHelper)
        {
            _Context = context;
            _sQLHelper = sQLHelper;
        }

        public HandleErrorResponseModel AddNewFamilyStatus(AddFamilyStatusModel Model)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                var FamilyObj = new FamilyStatus();
                var Code = _Context.FamilyStatus.Max(i => i.Id);
                FamilyObj.StatusTypeId = Model.FamilyStatus.StatusTypeId;
                FamilyObj.CategoryId = Model.FamilyStatus.CategoryId;
                FamilyObj.NationalityId = Model.FamilyStatus.NationalityId;
                FamilyObj.Code = Code + 1;
                FamilyObj.Name = Model.FamilyStatus.Name;
                FamilyObj.Fname = Model.FamilyStatus.Fname;
                FamilyObj.Address = Model.FamilyStatus.Address;
                FamilyObj.Village = Model.FamilyStatus.Village;
                FamilyObj.Center = Model.FamilyStatus.Center;
                FamilyObj.Governorate = Model.FamilyStatus.Governorate;
                FamilyObj.Phone = Model.FamilyStatus.Phone;
                FamilyObj.Phone1 = Model.FamilyStatus.Phone1;
                FamilyObj.SupportingParty = Model.FamilyStatus.SupportingParty;
                FamilyObj.ReasonOfRefuse = Model.FamilyStatus.ReasonOfRefuse;
                FamilyObj.InsertUser = Model.FamilyStatus.InsertUser;
                FamilyObj.InsertDate = DateTime.Now.AddHours(1);
                FamilyObj.AddedDate = Model.FamilyStatus.AddedDate;

                _Context.FamilyStatus.Add(FamilyObj);
                _Context.SaveChanges();


                AddNewFamilyIncome(Model.FamilyIncome, FamilyObj.Id);
                AddNewFamilyExpenses(Model.FamilyExpenses, FamilyObj.Id);
                AddNewFamilyExtraDetails(Model.FamilyExtraDetails, FamilyObj.Id);
                AddNewFamilyDetails(Model.FamilyDetails, FamilyObj.Id);
                AddNewFamilyPatient(Model.FamilyPatient, FamilyObj.Id);
                AddNewFamilyNeeds(Model.FamilyNeeds, FamilyObj.Id);
                _Context.SaveChanges();
                Response.Done = true;
                Response.Message = "تم اضافة حالة جديدة بنجاح";
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

        private void AddNewFamilyIncome(FamilyIncome Model, int FamilyStatusId)
        {
            var IncomeObj = new FamilyIncome();
            IncomeObj.FamilyStatusId = FamilyStatusId;
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
            _Context.FamilyIncome.Add(IncomeObj);
        }

        private void AddNewFamilyExpenses(FamilyExpenses Model, int FamilyStatusId)
        {
            var ExpensesObj = new FamilyExpenses();
            ExpensesObj.FamilyStatusId = FamilyStatusId;
            ExpensesObj.Rent_Electricity_Water_Gas_Sewage = Model.Rent_Electricity_Water_Gas_Sewage;
            ExpensesObj.MedicalExamination_Treatment = Model.MedicalExamination_Treatment;
            ExpensesObj.SchoolExpenses = Model.SchoolExpenses;
            ExpensesObj.Installment_debts = Model.Installment_debts;
            ExpensesObj.PhysiotherapySessions = Model.PhysiotherapySessions;
            ExpensesObj.Analysis = Model.Analysis;
            ExpensesObj.SatisfactoryTransfers = Model.SatisfactoryTransfers;
            ExpensesObj.MedicalXRays = Model.MedicalXRays;
            ExpensesObj.IsMinisterialSupply = Model.IsMinisterialSupply;
            ExpensesObj.IsFoodBank = Model.IsFoodBank;
            ExpensesObj.TotalFamilyExpenses = Model.TotalFamilyExpenses;
            ExpensesObj.NetFamilyIncome = Model.NetFamilyIncome;
            ExpensesObj.FamilyCount = Model.FamilyCount;
            ExpensesObj.AvgPersonIncome = Model.AvgPersonIncome;
            _Context.FamilyExpenses.Add(ExpensesObj);
        }

        private void AddNewFamilyExtraDetails(FamilyExtraDetails Model, int FamilyStatusId)
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

        private void AddNewFamilyDetails(List<FamilyDetails> Model, int FamilyStatusId)
        {
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

        private void AddNewFamilyPatient(List<FamilyPatient> Model, int FamilyStatusId)
        {
            foreach (var FamilyPatient in Model)
            {
                var FamilyPatientObj = new FamilyPatient();
                FamilyPatientObj.FamilyStatusId = FamilyStatusId;
                FamilyPatientObj.Name = FamilyPatient.Name;
                FamilyPatientObj.PatientTypeId = FamilyPatient.PatientTypeId;
                FamilyPatientObj.PatientDate = FamilyPatient.PatientDate;
                FamilyPatientObj.Specialization = FamilyPatient.Specialization;
                FamilyPatientObj.IsMedicalReport = FamilyPatient.IsMedicalReport;
                _Context.FamilyPatient.Add(FamilyPatientObj);
            }
        }

        private void AddNewFamilyNeeds(List<FamilyNeeds> Model, int FamilyStatusId)
        {
            foreach (var FamilyNeed in Model)
            {
                var FamilyNeedsObj = new FamilyNeeds();
                FamilyNeedsObj.StatusId = FamilyStatusId;
                FamilyNeedsObj.NeedTypeId = FamilyNeed.NeedTypeId;
                _Context.FamilyNeeds.Add(FamilyNeedsObj);
            }
        }
    }
}
