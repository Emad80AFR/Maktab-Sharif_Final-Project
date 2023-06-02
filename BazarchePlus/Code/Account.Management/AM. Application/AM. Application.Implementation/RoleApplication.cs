using AM._Application.Contracts.Role;
using AM._Application.Contracts.Role.DTO_s;
using AM._Domain.RollAgg;
using FrameWork.Application;
using FrameWork.Application.Messages;
using Microsoft.Extensions.Logging;

namespace AM._Application.Implementation;

public class RoleApplication:IRoleApplication
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<RoleApplication> _logger;

    public RoleApplication(IRoleRepository roleRepository, ILogger<RoleApplication> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<OperationResult> Create(CreateRole command, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        try
        {
            if (await _roleRepository.Exist(x => x.Name == command.Name, cancellationToken))
            {
                _logger.LogWarning(ApplicationMessages.DuplicatedRecord);
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var role = new Role(command.Name);
            await _roleRepository.Create(role, cancellationToken);
            await _roleRepository.SaveChanges(cancellationToken);

            _logger.LogInformation("Role created successfully.");
            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the role.");
            return operation.Failed("An error occurred while creating the role.");
        }
    }

    public async Task<OperationResult> Edit(EditRole command,CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        try
        {
            var role = await _roleRepository.Get(command.Id, cancellationToken);
            if (role == null)
            {
                _logger.LogWarning(ApplicationMessages.RecordNotFound);
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            if (await _roleRepository.Exist(x => x.Name == command.Name && x.Id != command.Id, cancellationToken))
            {
                _logger.LogWarning(ApplicationMessages.DuplicatedRecord);
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var permissions = new List<Permission>();
            command.Permissions.ForEach(code => permissions.Add(new Permission(code)));

            role.Edit(command.Name);
            await _roleRepository.SaveChanges(cancellationToken);

            _logger.LogInformation("Role updated successfully.");
            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the role.");
            return operation.Failed("An error occurred while updating the role.");
        }
    }

    public async Task<List<RoleViewModel>> List(CancellationToken cancellationToken)
    {

        try
        {
            var roles = await _roleRepository.List(cancellationToken);
            _logger.LogInformation("Roles listed successfully.");
            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while listing roles.");
            return null!; 
        }


    }

    public async Task<EditRole> GetDetails(long id,CancellationToken cancellationToken)
    {
        try
        {
            var details = await _roleRepository.GetDetails(id, cancellationToken);
            _logger.LogInformation("Role details retrieved successfully.");
            return details;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving role details.");
            return null!;
        }
    }
}