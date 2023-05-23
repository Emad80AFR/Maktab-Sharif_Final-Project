namespace AM._Application.Contracts.Role.DTO_s;

public class CreateRole
{
    public string Name { get; set; }
    public List<int> Permissions { get; set; }

}