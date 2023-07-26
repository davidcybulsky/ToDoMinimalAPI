using FluentValidation;
using ToDoMinimalAPI.DTOs;

namespace ToDoMinimalAPI.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(loginDto => loginDto.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(loginDto => loginDto.Password)
                .MinimumLength(8)
                .MaximumLength(16);
        }
    }
}
