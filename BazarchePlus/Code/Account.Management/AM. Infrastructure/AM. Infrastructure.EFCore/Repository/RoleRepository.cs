using AM._Application.Contracts.Role.DTO_s;
using AM._Domain.RollAgg;
using FrameWork.Application;
using FrameWork.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AM._Infrastructure.EFCore.Repository;

public class RoleRepository:BaseRepository<long,Role>,IRoleRepository
{
    private readonly ILogger<RoleRepository> _logger;
    private readonly AccountContext _context;
    public RoleRepository(AccountContext context, ILogger<RoleRepository> logger) : base(context, logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<RoleViewModel>> List(CancellationToken cancellationToken)
    {
        var roles = await _context.Roles
            .Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CreationDate = x.CreationDate.ToFarsi()
            })
            .ToListAsync(cancellationToken: cancellationToken);

        _logger.LogInformation("Retrieved roles successfully");

        return roles;
    }

    public async Task<EditRole> GetDetails(long id, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .Select(x => new EditRole
            {
                Id = x.Id,
                Name = x.Name,
                MappedPermissions = MapPermissions(x.Permissions)
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (role != null)
        {
            role.Permissions = role.MappedPermissions.Select(x => x.Code).ToList();
            _logger.LogInformation("Retrieved role with ID {RoleId} successfully", id);
        }
        else
        {
            _logger.LogWarning("Role with ID {RoleId} not found", id);
        }

        return role!;
    }
    private static List<PermissionDto> MapPermissions(IEnumerable<Permission> permissions)
    {
        return permissions.Select(x => new PermissionDto(x.Code, x.Name)).ToList();
    }
}