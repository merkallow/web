namespace Merkallow.Web.ViewModels
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }

    public class ProjectCreateRequest
    {
        public string Name { get; set; }
    }

    public class GenerateTreeRequest
    {
        public int ProjectId { get; set; }
    }

    public class ProjectRoot
    { 
        public string Root { get; set; }
        public string Cid { get; set; }
    }
}
