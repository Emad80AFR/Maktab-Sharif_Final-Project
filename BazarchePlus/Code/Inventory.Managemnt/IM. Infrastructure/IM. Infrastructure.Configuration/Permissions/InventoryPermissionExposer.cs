using FrameWork.Infrastructure.Permission;

namespace IM._Infrastructure.Configuration.Permissions
{
    public class InventoryPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Inventory", new List<PermissionDto>
                    {
                        new(InventoryPermissions.ListInventory, "لیست انبار"),
                        new(InventoryPermissions.SearchInventory, "جستجو در انبار"),
                        new(InventoryPermissions.CreateInventory, "ساخت انبار جدید"),
                        new(InventoryPermissions.EditInventory, "ویرایش انبار"),
                        new(InventoryPermissions.Increase, "افزایش موجودی"),
                        new(InventoryPermissions.Reduce, "کاهش موجودی"),
                        new(InventoryPermissions.OperationLog, "مشاهده گردش انبار")
                    }
                },
                {
                    "InventoryMenu", new List<PermissionDto>
                    {
                        new(InventoryPermissions.Menu, "مشاهده منو")
                    }
                }
            };
        }
    }
}