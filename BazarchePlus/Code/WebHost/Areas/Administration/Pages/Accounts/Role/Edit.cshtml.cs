using AM._Application.Contracts.Role;
using AM._Application.Contracts.Role.DTO_s;
using FrameWork.Infrastructure.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebHost.Areas.Administration.Pages.Accounts.Role
{
    public class EditModel : PageModel
    {
        public EditRole Command;
        public List<SelectListItem> Permissions = new();
        private readonly IRoleApplication _roleApplication;
        private readonly IEnumerable<IPermissionExposer> _exposers;

        public EditModel(IRoleApplication roleApplication, IEnumerable<IPermissionExposer> exposers)
        {
            _roleApplication = roleApplication;
            _exposers = exposers;
        }

        public async Task OnGet(long id,CancellationToken cancellationToken)
        {
            Command = await _roleApplication.GetDetails(id,cancellationToken);
            foreach (var exposer in _exposers)
            {
                var exposedPermissions = exposer.Expose();
                foreach (var (key, value) in exposedPermissions)
                {
                    var group = new SelectListGroup {Name = key};
                    foreach (var permission in value)
                    {
                        var item = new SelectListItem(permission.Name, permission.Code.ToString())
                        {
                            Group = group
                        };

                        if (Command.MappedPermissions.Any(x => x.Code == permission.Code))
                            item.Selected = true;

                        Permissions.Add(item);
                    }
                }
            }
        }

        public async Task<IActionResult> OnPost(EditRole command,CancellationToken cancellationToken)
        {
            var result = await _roleApplication.Edit(command,cancellationToken);
            return RedirectToPage("Index");
        }
    }
}