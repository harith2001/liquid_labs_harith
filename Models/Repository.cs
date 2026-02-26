public class Repository
{
    public int ID { get; set; }
    public long GithubID { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string Descriprion { get; set; }
    public int StargazersCount { get; set; }
    public int ForksCount { get; set; }
    public int OpenIssuesCount { get; set; }
    public string Language { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Owner Owner { get; set; }

}