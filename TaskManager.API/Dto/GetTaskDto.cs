namespace TaskManager.API.Dto
{
    public class GetTaskDto
    {
        public string TaskControlId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
