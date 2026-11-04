namespace CodingPortfolio.Models
{
    public class ProjectTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty; 
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public bool IsCompleted { get; set; } = false; 
        public DateTime? DueDate { get; set; } 


        // Foreign Key للمشروع
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}

