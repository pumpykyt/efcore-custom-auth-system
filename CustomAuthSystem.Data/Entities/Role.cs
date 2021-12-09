namespace CustomAuthSystem.Data.Entities;

public class Role
{
    public string Id { get; set; }
    public string Value { get; set; }
    public virtual ICollection<User> Users { get; set; } 
}