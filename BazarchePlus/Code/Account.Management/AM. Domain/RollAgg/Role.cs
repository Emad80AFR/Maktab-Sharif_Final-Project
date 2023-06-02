using AM._Domain.AccountAgg;
using FrameWork.Domain;

namespace AM._Domain.RollAgg;

public class Role:BaseClass<long>
{
    public string Name { get; private set; } 
    public List<Account> Accounts { get; private set; }

    protected Role()
    {
    }

    public Role(string name)
    {
        Name = name;
        Accounts = new List<Account>();
    }

    public void Edit(string name)
    {
        Name = name;
    }

}