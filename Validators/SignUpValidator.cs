using FluentValidation;
using ToDoMinimalAPI.Context;
using ToDoMinimalAPI.DTOs;

namespace ToDoMinimalAPI.Validators
{
    public class SignUpValidator : AbstractValidator<SignUpDto>
    {
        public SignUpValidator(ApiContext db)
        {
            RuleFor(signUpDto => signUpDto.Email)
                .NotEmpty()
                .EmailAddress()
                .Must((signUpDto, context) =>
                {
                    var isEmailInDb = db.Users.Any(u => u.Email == signUpDto.Email);
                    return !isEmailInDb;
                }).WithMessage("This email has already been taken.");

            RuleFor(signUpDto => signUpDto.Password)
                .MinimumLength(8)
                .MaximumLength(16)
                .Equal(signUpDto => signUpDto.RepeatPassword);
        }
    }
}
