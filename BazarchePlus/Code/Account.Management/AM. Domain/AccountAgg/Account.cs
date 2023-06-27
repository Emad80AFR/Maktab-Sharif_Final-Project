using System.Reflection.Metadata.Ecma335;
using AM._Domain.RollAgg;
using FrameWork.Domain;

namespace AM._Domain.AccountAgg;

public class Account:BaseClass<long>
{
    public string Fullname { get; private set; }
    public string Username { get; private set; }
    public string? ShopName { get; set; }
    public string Password { get; private set; }
    public string Mobile { get; private set; }
    public long RoleId { get; private set; }
    public Role Role { get; private set; }
    public string ProfilePhoto { get; private set; }
    public string? ShopPhoto { get; set; }
    public bool IsActive { get; set; }
    public double SalesAmount { get; set; }
    public int Medal { get; set; }

    protected Account()
    {
        
    }
    public Account(string fullname, string username, string password, string mobile,
        long roleId, string profilePhoto)
    {
        Fullname = fullname;
        Username = username;
        Password = password;
        Mobile = mobile;
        RoleId = roleId;
        IsActive=true;

        if (roleId is 0 or 3)
            IsActive = false;

        ProfilePhoto = profilePhoto;
    }

    public void Edit(string fullname, string username, string mobile,
        long roleId, string profilePhoto)
    {
        Fullname = fullname;
        Username = username;
        Mobile = mobile;
        RoleId = roleId;

        if (!string.IsNullOrWhiteSpace(profilePhoto))
            ProfilePhoto = profilePhoto;
    }

    public void EditSeller(string fullname, string shopName,string username, string mobile, string profilePhoto,string shopPhoto)
    {
        Fullname=fullname;
        Username=username;
        Mobile = mobile;

        if (!string.IsNullOrWhiteSpace(shopName))
            ShopName = shopName;

        if (!string.IsNullOrWhiteSpace(profilePhoto))
            ProfilePhoto = profilePhoto;

        if (!string.IsNullOrWhiteSpace(shopPhoto))
            ShopPhoto = shopPhoto;
    }

    public void ChangePassword(string password)
    {
        Password = password;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive=false;
    }

    public void UpdateSaleAmount(double saleAmount)
    {
        SalesAmount += saleAmount;
    }
}