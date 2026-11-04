namespace CodingPortfolio.Models
{
    public class Developer
    {
        public int DeveloperId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;  
        public string? Bio { get; set; } 
        public string? GitHubUrl { get; set; }

    
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}

