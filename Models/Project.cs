namespace CodingPortfolio.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Technologies { get; set; } 
        public string? GitHubUrl { get; set; }
        public string? LiveDemoUrl { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;

        // Foreign Key للمطور
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; } = null!;


        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}

