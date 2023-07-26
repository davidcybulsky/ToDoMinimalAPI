using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ToDoMinimalAPI.DTOs;

namespace ToDoMinimalAPI.Auth
{
    public static class AuthRequests
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapPost("/login", AuthRequests.Login);
            app.MapPost("/signup", AuthRequests.SignUp);
        }

        public static IResult Login(IAuthService authService,
                                   [FromBody] LoginDto loginDto,
                                   IValidator<LoginDto> validator)
        {
            var result = validator.Validate(loginDto);
            if (!result.IsValid)
            {
                return Results.BadRequest(result.Errors);
            }
            var token = authService.Login(loginDto);
            if (token is null)
            {
                return Results.BadRequest("Bad login or password");
            }
            return Results.Ok(token);
        }

        public static IResult SignUp(IAuthService authService,
                                     [FromBody] SignUpDto signUpDto,
                                     IValidator<SignUpDto> validator)
        {
            var result = validator.Validate(signUpDto);
            if (!result.IsValid)
            {
                return Results.BadRequest(result.Errors);
            }
            var id = authService.SignUp(signUpDto);
            return Results.Created($"/user/{id}", id);
        }
    }
}
