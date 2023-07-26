using Microsoft.AspNetCore.Mvc;
using ToDoMinimalAPI.DTOs;

namespace ToDoMinimalAPI.ToDo
{
    public static class ToDoRequests
    {
        public static WebApplication RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/todo", ToDoRequests.Get)
                .RequireAuthorization();
            app.MapGet("/todo/{id}", ToDoRequests.GetById)
                .RequireAuthorization();
            app.MapPost("/todo", ToDoRequests.Post)
                .RequireAuthorization();
            app.MapPut("/todo{id}", ToDoRequests.Put)
                .RequireAuthorization();
            app.MapDelete("/todo/{id}", ToDoRequests.Delete)
                .RequireAuthorization();
            return app;
        }

        public static IResult GetById(IToDoService service,
                                      [FromRoute] Guid id)
        {
            var todo = service.Get(id);
            if (todo is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(todo);
        }

        public static IResult Get(IToDoService service)
        {
            var todos = service.Get();
            return Results.Ok(todos);
        }

        public static IResult Post(IToDoService service,
                                   [FromBody] CreateToDoDto createToDoDto)
        {
            var Todo = service.Post(createToDoDto);
            return Results.Created($"/todo/{Todo.Id}", Todo);
        }

        public static IResult Put(IToDoService service,
                                  [FromRoute] Guid id,
                                  [FromBody] EditToDoDto editToDoDto)
        {
            var todo = service.Get(id);
            if (todo is null)
            {
                return Results.NotFound();
            }
            service.Put(id, editToDoDto);
            return Results.NoContent();
        }

        public static IResult Delete(IToDoService service,
                                     [FromRoute] Guid id)
        {
            var todo = service.Get(id);
            if (todo is null)
            {
                return Results.NotFound();
            }
            service.Delete(id);
            return Results.NoContent();
        }
    }
}
