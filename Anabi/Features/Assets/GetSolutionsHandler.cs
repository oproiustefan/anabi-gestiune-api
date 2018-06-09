﻿using Anabi.Common.ViewModels;
using Anabi.Domain;
using Anabi.Features.Assets.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anabi.Features.Assets
{
    public class GetSolutionsHandler : BaseHandler, IAsyncRequestHandler<GetSolutions, List<SolutionViewModel>>
    {
        public GetSolutionsHandler(BaseHandlerNeeds needs) : base(needs)
        {

        }

        public async Task<List<SolutionViewModel>> Handle(GetSolutions message)
        {

            var result = await context.HistoricalStages
                .Where(p => p.AssetId == message.AssetId)
                .Select(h => new SolutionViewModel
                {
                    DecisionDate = h.DecisionDate,
                    DecisionId = h.DecizieId,
                    DecisionNumber = h.DecisionNumber,
                    Id = h.Id,
                    InstitutionId = h.InstitutionId,
                    StageId = h.StageId,
                    RecoveryBeneficiaryId = h.RecoveryBeneficiaryId,
                    RecoveryDetails = new RecoveryDetailsViewModel (h.ActualValue, h.EstimatedAmount, h.EstimatedAmountCurrency, h.ActualValueCurrency, h.RecoveryState, 
                                                 new EvaluationCommitteeViewModel(h.EvaluationCommitteeDesignationDate, h.EvaluationCommitteePresident, h.EvaluationCommittee),
                                                    new RecoveryCommitteeViewModel(h.RecoveryCommitteeDesignationDate, h.RecoveryCommitteePresident, h.RecoveryCommittee),
                    h.LastActivity, h.PersonResponsible),
                    SolutionDetails = new SolutionDetailsViewModel(h.Source, h.SentOnEmail, h.FileNumber, h.FileNumberParquet, h.ReceivingDate, h.IsDefinitive, h.DefinitiveDate, h.SendToAuthoritiesDate, h.CrimeTypeId, h.LegalBasis),
                    Journal = new JournalViewModel
                    {
                        AddedDate = h.AddedDate,
                        UserCodeAdd = h.UserCodeAdd,
                        LastChangeDate = h.LastChangeDate,
                        UserCodeLastChange = h.UserCodeLastChange
                    }
                }).
                ToListAsync();

            return result;

        }
    }
}
