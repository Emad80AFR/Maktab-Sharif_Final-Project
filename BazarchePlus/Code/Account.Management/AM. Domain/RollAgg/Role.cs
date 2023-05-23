using AM._Domain.AccountAgg;
using FrameWork.Domain;

namespace AM._Domain.RollAgg;

public class Role:BaseClass<long>
{
    public string Name { get; private set; }
    public List<Permission> Permissions { get; private set; }
    public List<Account> Accounts { get; private set; }

    protected Role()
    {
    }

    public Role(string name, List<Permission> permissions)
    {
        Name = name;
        Permissions = permissions;
        Accounts = new List<Account>();
    }

    public void Edit(string name, List<Permission> permissions)
    {
        Name = name;
        Permissions = permissions;
    }

}