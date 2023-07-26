namespace ToDoMinimalAPI.Context
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<ToDo> ToDos { get; set; }
    }
}
