﻿using Anabi.DataAccess.Ef;
using Anabi.DataAccess.Ef.DbModels;
using Anabi.DataAccess.Abstractions.Repositories;

namespace Anabi.DataAccess.Repositories
{
    public class InstitutionsRepository : GenericRepository<InstitutionDb>
    {
        public InstitutionsRepository(AnabiContext ctx) : base(ctx)
        {

        }

    }
}