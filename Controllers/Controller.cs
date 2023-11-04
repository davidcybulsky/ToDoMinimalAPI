using Microsoft.AspNetCore.Mvc;
using ToDoMinimalAPI.Auth;
using ToDoMinimalAPI.DTOs;
using ToDoMinimalAPI.ToDo;

namespace ToDoMinimalAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class Controller : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IToDoService _toDo;

        public Controller(IAuthService auth, IToDoService toDo)
        {
            _auth = auth;
            _toDo = toDo;
        }

        [HttpPost("login")]
        public ActionResult<string> login(LoginDto loginDto)
        {
            var response = _auth.Login(loginDto);
            return Ok(response);
        }

        [HttpPost("signup")]
        public ActionResult<int> signup(SignUpDto signUpDto)
        {
            var response = _auth.SignUp(signUpDto);
            return Ok(response);
        }

        [HttpGet("todo")]
        public ActionResult<List<ToDoDto>> get()
        {
            var response = _toDo.Get();
            return Ok(response.ToList());
        }

        [HttpGet("todo/{id}")]
        public ActionResult<ToDoDto> get(Guid id)
        {
            var response = _toDo.Get(id);
            return Ok(response);
        }

        [HttpPost("todo")]
        public ActionResult<ToDoDto> post(CreateToDoDto createToDoDto)
        {
            var response = _toDo.Post(createToDoDto);
            return Ok(response);
        }

        [HttpPut("todo/{id}")]
        public ActionResult update(Guid id, EditToDoDto editToDoDto)
        {
            _toDo.Put(id, editToDoDto);
            return NoContent();
        }

        [HttpDelete("todo/{id}")]
        public ActionResult delete(Guid id)
        {
            _toDo.Delete(id);
            return NoContent();
        }
    }
}
