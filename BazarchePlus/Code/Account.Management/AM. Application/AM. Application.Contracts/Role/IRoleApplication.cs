using AM._Application.Contracts.Role.DTO_s;
using FrameWork.Application;

namespace AM._Application.Contracts.Role;

public interface IRoleApplication
{
    Task<OperationResult> Create(CreateRole command);
    Task<OperationResult> Edit(EditRole command);
    Task<List<RoleViewModel>> List();
    Task<EditRole> GetDetails(long id);

}