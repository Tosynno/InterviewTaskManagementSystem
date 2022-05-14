using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class OnbardingRequestValidator : AbstractValidator<OnbardingRequest>
    {
        public OnbardingRequestValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
  .NotEmpty().WithMessage("Enter a valid value")
  .EmailAddress().WithMessage("Invalid Email Address");
            RuleFor(x => x.PhoneNumber)
 .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Otp)
 .NotEmpty().WithMessage("Enter a valid value");

            RuleFor(x => x.ConfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
    .NotEmpty().WithMessage("Enter a valid value")
    .When(x => x.Password != x.ConfirmPassword)
    .WithMessage("Password does not match");
        }
    
    }
}
