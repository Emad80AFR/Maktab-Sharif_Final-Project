using AM._Application.Contracts.Role.DTO_s;
using FrameWork.Application;

namespace AM._Application.Contracts.Role;

public interface IRoleApplication
{
    Task<OperationResult> Create(CreateRole command,CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditRole command, CancellationToken cancellationToken);
    Task<List<RoleViewModel>> List(CancellationToken cancellationToken);
    Task<EditRole> GetDetails(long id, CancellationToken cancellationToken);

}