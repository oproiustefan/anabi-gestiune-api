﻿using Anabi.Common.Enums;
using Anabi.Common.ViewModels;
using Anabi.Domain.Asset.Commands;
using AutoFixture;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Anabi.Integration.Tests.Assets
{
    public class AddSolutionTests : ApiTests
    {
        public AddSolutionTests(AnabiApplicationFactory<Startup> factory) : base(factory)
        {
            Context = new AnabiDbContextBuilder()
                .CreateInMemorySqliteDbContext()
                .WithAllDictionaries()
                .Build();

            Client = factory.CreateClient();
            
        }

        [Fact]
        public async Task AddSolution_HasRecoveryFields()
        {
            var assetId = await AddMinimalAsset();

            var applicationNr = "aa232";
            var applicationDate = DateTime.Today;
            var recoveryDocType = RecoveryDocumentType.Ordonanta;
            var issuingInstitution = "institution A";
            var request = CreateSolutionRequest(applicationNr, applicationDate, recoveryDocType, issuingInstitution);

            var json = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            await Client.PostAsync($"api/assets/{assetId}/solutions", stringContent);

            var solution = Context.HistoricalStages.Single();

            Assert.Equal(applicationNr, solution.RecoveryApplicationNumber);
            Assert.Equal(applicationDate, solution.RecoveryApplicationDate);
            Assert.Equal(recoveryDocType, solution.RecoveryDocumentType);
            Assert.Equal(issuingInstitution, solution.RecoveryIssuingInstitution);
        }

        private async Task<int> AddMinimalAsset()
        {
            var stage = Context.Stages.First();
            var category = Context.Categories.First();

            var message = new AddMinimalAsset
            {
                Description = "Description",
                EstimatedAmount = 100,
                EstimatedAmountCurrency = "EUR",
                Identifier = "AA",
                MeasureUnit = "kg",
                Name = "Asset 1",
                Quantity = 23,
                Remarks = "remarksss",
                StageId = stage.Id,
                SubcategoryId = category.Id
            };


            var response = await Client.PostAsJsonAsync($"api/assets/addminimalasset", message);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var assetViewModel = JsonConvert.DeserializeObject<MinimalAssetViewModel>(content);
            return assetViewModel.Id;
        }

        private AddSolutionRequest CreateSolutionRequest(string applicationNr, DateTime applicationDate, RecoveryDocumentType recoveryDocumentType, string issuingInstitution)
        {
            var r = new Fixture();
            var request = r.Build<AddSolutionRequest>()
                .With(x => x.RecoveryDetails.RecoveryApplicationNumber, applicationNr)
                .With(x => x.RecoveryDetails.RecoveryApplicationDate, applicationDate)
                .With(x => x.RecoveryDetails.RecoveryDocumentType, recoveryDocumentType)
                .With(x => x.RecoveryDetails.RecoveryIssuingInstitution, issuingInstitution)
                .Create();

            return request;

            //return new AddSolutionRequest(
            //    stageId: 0,
            //    decisionId: 0,
            //    institutionId: 0,
            //    decisionDate: DateTime.Today,
            //    decisionNumber: "aa123",
            //    confiscationDetails: null,
            //    recoveryDetails: new Domain.Asset.Commands.Models.RecoveryDetails(actualAmount: 1000,
            //        estimatedAmount: 2000, estimatedAmountCurrency: "RON", actualAmountCurrency: "RON", recoveryState: "ok",
            //        evaluationCommittee: new Domain.Asset.Commands.Models.EvaluationCommittee(evaluationCommitteeDesignationDate: DateTime.Today, evaluationCommitteePresident: "P", evaluationCommitteeMembers:"A,B"),
            //        recoveryCommittee: new Domain.Asset.Commands.Models.RecoveryCommittee(recoveryCommitteeDesignationDate: DateTime.Now, recoveryCommitteePresident: "P", recoveryCommitteeMembers: "A,B"),
            //        lastActivity: DateTime.Now, personResponsible: "Alex", 
            //        recoveryApplicationNumber: "aa123", 
            //        recoveryApplicationDate: DateTime.Now, 
            //        recoveryDocumentType: Common.Enums.RecoveryDocumentType.Ordonanta,
            //        recoveryIssuingInstitution: "Institutie"),
            //    solutionDetails: null,
            //    sequesterDetails: null
            //    );
        }
    }
}