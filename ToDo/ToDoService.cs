using ToDoMinimalAPI.Auth;
using ToDoMinimalAPI.Context;
using ToDoMinimalAPI.DTOs;
namespace ToDoMinimalAPI.ToDo
{
    public interface IToDoService
    {
        IEnumerable<ToDoDto> Get();
        ToDoDto Get(Guid id);
        ToDoDto Post(CreateToDoDto createToDoDto);
        void Put(Guid id, EditToDoDto editToDoDto);
        void Delete(Guid id);
    }

    public class ToDoService : IToDoService
    {

        private readonly ApiContext _db;
        private readonly IUserContextService _userContextService;

        public ToDoService(ApiContext db, IUserContextService userContextService)
        {
            _db = db;
            _userContextService = userContextService;
        }

        public void Delete(Guid id)
        {
            var todo = _db.ToDos.FirstOrDefault(t => t.Id == id);
            if (todo is null)
            {
                return;
            }
            _db.ToDos.Remove(todo);
            _db.SaveChanges();
        }

        public IEnumerable<ToDoDto> Get()
        {
            IEnumerable<Context.ToDo> toDos = _db.ToDos.Where(t => t.UserId == _userContextService.GetUserId);
            List<ToDoDto> toDoDtos = new();
            foreach (var toDo in toDos)
            {
                toDoDtos.Add(new ToDoDto
                {
                    Id = toDo.Id,
                    Name = toDo.Name,
                    Description = toDo.Description,
                    IsCompleted = toDo.IsCompleted
                });
            }
            return toDoDtos;
        }

        public ToDoDto Get(Guid id)
        {
            var toDo = _db.ToDos.FirstOrDefault(t => t.Id == id);
            if (toDo is null)
            {
                return null;
            }
            var toDoDto = new ToDoDto
            {
                Id = toDo.Id,
                Name = toDo.Name,
                Description = toDo.Description,
                IsCompleted = toDo.IsCompleted,
            };
            return toDoDto;
        }

        public ToDoDto Post(CreateToDoDto createToDoDto)
        {

            var todo = new Context.ToDo
            {
                Name = createToDoDto.Name,
                Description = createToDoDto.Description,
                IsCompleted = createToDoDto.IsCompleted,
                UserId = (int)_userContextService.GetUserId,
            };

            _db.ToDos.Add(todo);
            _db.SaveChanges();

            var todoDto = new ToDoDto
            {
                Id = todo.Id,
                Name = todo.Name,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
            };

            return todoDto;
        }

        public void Put(Guid id, EditToDoDto editToDoDto)
        {
            var todo = _db.ToDos.FirstOrDefault(t => t.Id == id);
            if (todo is null)
            {
                return;
            }
            todo.Name = editToDoDto.Name;
            todo.Description = editToDoDto.Description;
            todo.IsCompleted = editToDoDto.IsCompleted;

            _db.SaveChanges();
        }
    }
}
