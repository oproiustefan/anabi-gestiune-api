﻿using System.Threading;
using System.Threading.Tasks;
using Anabi.DataAccess.Ef;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Anabi.Domain.Category.Commands
{
    public class AddCategory : IRequest<int>
    {
        
        
        public string Code { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }

        public string ForEntity { get; set; }
    }


    public class AddCategoryValidator : AbstractValidator<AddCategory>
    {
        private readonly AnabiContext context;

        public AddCategoryValidator(AnabiContext ctx)
        {
            //TODO Aici ar fi trebuit sa se foloseasca resources la nivel de domeniu
            context = ctx;
            
            RuleFor(c => c.Code).NotEmpty().WithMessage("CODE_MANDATORY"); //"Id-ul nu a fost specificat!"
            RuleFor(c => c.Code).Length(1, 100).WithMessage("CODE_LENGTH_1_TO_100"); //"Codul trebuie sa aiba lungimea de 1 pana la 100 de caractere!"
            RuleFor(c => c.Code).MustAsync(HaveUniqueCode).WithMessage("CODE_EXISTS_ON_DIFFERENT_CATEGORY"); //"Codul exista la o alta categorie!"

            RuleFor(c => c.Description).Length(0, 4000).WithMessage("DESCRIPTION_MAX_LENGTH_4000");//"Descrierea nu poate avea mai mult de 4000 de caractere!"

            RuleFor(c => c.ForEntity).NotEmpty().WithMessage("FORENTITY_MUST_BE_SPECIFIED"); //"ForEntity nu a fost specificat"
            RuleFor(c => c.ForEntity).Length(1, 40).WithMessage("FORENTITY_MAX_LENGTH_40");//"\"Pentru entitate\" trebuie sa contina o valoare de maxim 40 de caractere!"


        }

        private async Task<bool> HaveUniqueCode(string arg1, CancellationToken arg2)
        {
            var codeExists = await context.Categorii.AnyAsync(c => c.Code == arg1);
            return !codeExists;

        }
    }
}
