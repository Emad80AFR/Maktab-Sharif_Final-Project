using System.Security.AccessControl;
using FrameWork.Domain;

namespace CM._Domain.CommentAgg;

public class Comment:BaseClass<long>
{
    
    public string Name { get; private set; }
    public long Sender { get; private set; }
    public string Email { get; private set; }
    public string? Website { get; private set; }
    public string Message { get; private set; }
    public bool IsConfirmed { get; private set; }
    public bool IsCanceled { get; private set; }
    public long OwnerRecordId { get; private set; }
    public int Type { get; private set; }
    public long ParentId { get; private set; }
    public Comment Parent { get; private set; }
    public List<Comment> Comments { get; set; }
    public Comment(string name, string email, string website, string message, long ownerRecordId, int type, long parentId, long sender)
    {
        Name = name;
        Email = email;
        Website = website;
        Message = message;
        OwnerRecordId = ownerRecordId;
        Type = type;
        ParentId = parentId;
        Sender = sender;
    }

    public void Confirm()
    {
        IsConfirmed = true;
    }

    public void Cancel()
    {
        IsCanceled = true;
    }
}