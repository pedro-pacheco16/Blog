using blogpessoal.model;
using FluentValidation;

namespace blogpessoal.validator
{
    public class TemaValidator : AbstractValidator<Tema>
    {
        public TemaValidator()
        {

            RuleFor(p => p.Descricao)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(255);
        }
    }
}
