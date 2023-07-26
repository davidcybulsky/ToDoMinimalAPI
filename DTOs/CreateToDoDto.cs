namespace ToDoMinimalAPI.DTOs
{
    public class CreateToDoDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
