using Application.Faturas.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators
{
    public class AtualizarClienteDTOValidator : AbstractValidator<AtualizarClienteDTO>
    {
        public AtualizarClienteDTOValidator() {
            RuleFor(x => x.NomeCliente)
                .NotEmpty().WithMessage("Nome do cliente é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do cliente deve ter no máximo 100 caracteres.");
        }
    }
}
