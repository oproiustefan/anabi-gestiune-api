﻿using Anabi.Domain;
using Anabi.Features.Assets.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Anabi.Features.Assets
{
    public class GetStagesHandler : BaseHandler, IRequestHandler<GetStages, List<StageViewModel>>
    {
        public GetStagesHandler(BaseHandlerNeeds needs) : base(needs)
        {

        }

        public async Task<List<StageViewModel>> Handle(GetStages message, CancellationToken cancellationToken)
        {
            var models = await (context.Stages.OrderBy(s => s.Name)
                         .Select(s => new StageViewModel {
                             Id = s.Id,
                             IsFinal = s.IsFinal,
                             IsRecovery = s.StageCategory == Anabi.Common.Enums.StageCategory.Recovery,
                             Name = s.Name,
                         })
                         ).ToListAsync(cancellationToken);

            return models;
        }
    }
}
