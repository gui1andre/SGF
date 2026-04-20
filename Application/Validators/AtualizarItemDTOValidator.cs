using Application.Faturas.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators
{
    public class AtualizarItemDTOValidator : AbstractValidator<AtualizarItemDTO>
    {
        public AtualizarItemDTOValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatória.")
                .MinimumLength(5).WithMessage("Descrição deve ter no mínimo 5 caracteres.")
                .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres.");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.");

            RuleFor(x => x.ValorUnitario)
                .GreaterThan(0).WithMessage("ValorUnitario deve ser maior que zero.");

            RuleFor(x => x.Justificativa)
                .NotEmpty().WithMessage("Justificativa é obrigatória quando o valor total do item supera R$ 1.000,00.")
                .When(x => x.Quantidade * x.ValorUnitario > 1000m);
        }
    }
}
