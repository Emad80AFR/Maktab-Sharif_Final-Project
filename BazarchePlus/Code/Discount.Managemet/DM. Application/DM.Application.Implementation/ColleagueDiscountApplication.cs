using DM._Domain.ColleagueDiscountAgg;
using DM.Application.Contracts.ColleagueDiscount;
using DM.Application.Contracts.ColleagueDiscount.DTO_s;
using FrameWork.Application;
using FrameWork.Application.Messages;
using Microsoft.Extensions.Logging;

namespace DM.Application.Implementation;

public class ColleagueDiscountApplication:IColleagueDiscountApplication
{
    private readonly IColleagueDiscountRepository _colleagueDiscountRepository;
    private readonly ILogger<ColleagueDiscountApplication> _logger;

    public ColleagueDiscountApplication(IColleagueDiscountRepository colleagueDiscountRepository, ILogger<ColleagueDiscountApplication> logger)
    {
        _colleagueDiscountRepository = colleagueDiscountRepository;
        _logger = logger;
    }

    public async Task<OperationResult> Define(DefineColleagueDiscount command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Defining new colleague discount...");

            var operation = new OperationResult();

            if (await _colleagueDiscountRepository.Exist(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate,cancellationToken))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var colleagueDiscount = new ColleagueDiscount(command.ProductId, command.DiscountRate);

            await _colleagueDiscountRepository.Create(colleagueDiscount,cancellationToken);
            await _colleagueDiscountRepository.SaveChanges(cancellationToken);

            _logger.LogInformation("New colleague discount defined successfully.");

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while defining new colleague discount.");
            throw; 
        }
    }

    public async Task<OperationResult> Edit(EditColleagueDiscount command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Editing colleague discount...");

            var operation = new OperationResult();
            var colleagueDiscount =await _colleagueDiscountRepository.Get(command.Id,cancellationToken);

            if (colleagueDiscount == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (await _colleagueDiscountRepository.Exist(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate && x.Id != command.Id,cancellationToken))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            colleagueDiscount.Edit(command.ProductId, command.DiscountRate);

            await _colleagueDiscountRepository.SaveChanges(cancellationToken);

            _logger.LogInformation("Colleague discount edited successfully.");

            return operation.Succeeded();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while editing colleague discount.");
            throw;
        }
    }

    public async Task<OperationResult> Remove(long id, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var colleagueDiscount = await _colleagueDiscountRepository.Get(id,cancellationToken);

        if (colleagueDiscount == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        colleagueDiscount.Remove();

        try
        {
            _logger.LogInformation("Saving changes after removing colleague discount...");

            await _colleagueDiscountRepository.SaveChanges(cancellationToken);

            _logger.LogInformation("Colleague discount removed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while removing colleague discount.");
            throw;
        }

        return operation.Succeeded();
    }

    public async Task<OperationResult> Restore(long id, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var colleagueDiscount = await _colleagueDiscountRepository.Get(id,cancellationToken);

        colleagueDiscount!.Restore();

        try
        {
            _logger.LogInformation("Saving changes after restoring colleague discount...");

            await _colleagueDiscountRepository.SaveChanges(cancellationToken);

            _logger.LogInformation("Colleague discount restored successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while restoring colleague discount.");
            throw; 
        }

        return operation.Succeeded();
    }

    public async Task<EditColleagueDiscount> GetDetails(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching colleague discount details...");

            var details = await _colleagueDiscountRepository.GetDetails(id, cancellationToken);

            _logger.LogInformation("Colleague discount details retrieved successfully.");

            return details;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching colleague discount details.");
            throw;
        }
    }

    public async Task<List<ColleagueDiscountViewModel>> Search(ColleagueDiscountSearchModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Performing colleague discount search...");

            var discounts = await _colleagueDiscountRepository.Search(searchModel, cancellationToken);

            if (discounts != null && discounts.Any())
                _logger.LogInformation("Customer discount search completed successfully. Total discounts found: {DiscountCount}", discounts.Count);
            else
                _logger.LogWarning("No customer discounts found matching the search criteria.");

            return discounts!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while performing colleague discount search.");
            throw;
        }
    }
}