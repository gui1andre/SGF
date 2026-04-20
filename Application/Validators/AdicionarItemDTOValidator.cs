using Application.Faturas.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators
{
    public class AdicionarItemDTOValidator : AbstractValidator<AdicionarItemDTO>
    {
        public AdicionarItemDTOValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição do item é obrigatória.")
                .MinimumLength(5).WithMessage("A descrição do item deve ter no mínimo 5 caracteres.")
                .MaximumLength(200).WithMessage("A descrição do item deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

            RuleFor(x => x.ValorUnitario)
                .GreaterThan(0).WithMessage("O valor unitário deve ser maior que zero.");

            RuleFor(x => x.Justificativa)
                .MaximumLength(500).WithMessage("A justificativa deve ter no máximo 500 caracteres.");

            RuleFor(x => x.Justificativa)
                .NotEmpty().WithMessage("Justificativa é obrigatória quando o valor total do item supera R$ 1.000,00.")
                .When(x => x.Quantidade * x.ValorUnitario > 1000m);

        }
    }
}