using AM._Application.Contracts.Role.DTO_s;
using FrameWork.Domain;

namespace AM._Domain.RollAgg;

public interface IRoleRepository:IBaseRepository<long,Role>
{
    Task<List<RoleViewModel>> List();
    Task<EditRole> GetDetails(long id);

}