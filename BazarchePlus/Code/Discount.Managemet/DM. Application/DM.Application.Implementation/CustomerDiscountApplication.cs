using DM._Domain.CustomerDiscountAgg;
using DM.Application.Contracts.CustomerDiscount;
using DM.Application.Contracts.CustomerDiscount.DTO_s;
using FrameWork.Application;
using Microsoft.Extensions.Logging;

namespace DM.Application.Implementation
{
    public class CustomerDiscountApplication:ICustomerDiscountApplication
    {
        private readonly ICustomerDiscountRepository _customerDiscountRepository;
        private readonly ILogger<CustomerDiscountApplication> _logger;

        public CustomerDiscountApplication(ICustomerDiscountRepository customerDiscountRepository, ILogger<CustomerDiscountApplication> logger)
        {
            _customerDiscountRepository = customerDiscountRepository;
            _logger = logger;
        }

        public async Task<OperationResult> Define(DefineCustomerDiscount command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Defining customer discount...");

                var operation = new OperationResult();
                if (await _customerDiscountRepository.Exist(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate, cancellationToken))
                    return operation.Failed(ApplicationMessages.DuplicatedRecord);

                var startDate = command.StartDate.ToGeorgianDateTime();
                var endDate = command.EndDate.ToGeorgianDateTime();
                var customerDiscount = new CustomerDiscount(command.ProductId, command.DiscountRate,
                    startDate, endDate, command.Reason);
                await _customerDiscountRepository.Create(customerDiscount,cancellationToken);
                await _customerDiscountRepository.SaveChanges(cancellationToken);

                _logger.LogInformation("Customer discount defined successfully.");

                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while defining customer discount.");
                throw; 
            }
        }

        public async Task<OperationResult> Edit(EditCustomerDiscount command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Editing customer discount...");

                var operation = new OperationResult();
                var customerDiscount = await _customerDiscountRepository.Get(command.Id, cancellationToken);

                if (customerDiscount == null)
                    return operation.Failed(ApplicationMessages.RecordNotFound);

                if (await _customerDiscountRepository.Exist(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate && x.Id != command.Id, cancellationToken))
                    return operation.Failed(ApplicationMessages.DuplicatedRecord);

                var startDate = command.StartDate.ToGeorgianDateTime();
                var endDate = command.EndDate.ToGeorgianDateTime();
                customerDiscount.Edit(command.ProductId, command.DiscountRate, startDate, endDate, command.Reason);
                await _customerDiscountRepository.SaveChanges(cancellationToken);

                _logger.LogInformation("Customer discount edited successfully.");

                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing customer discount.");
                throw; 
            }
        }

        public async Task<EditCustomerDiscount> GetDetails(long id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching customer discount details...");

                var customerDiscount = await _customerDiscountRepository.GetDetails(id,cancellationToken);

                _logger.LogInformation("Customer discount details retrieved successfully. Customer Discount ID: {Id}", customerDiscount.Id);

                return customerDiscount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customer discount details.");
                throw; 
            }
        }

        public async Task<List<CustomerDiscountViewModel>> Search(CustomerDiscountSearchModel searchModel, CancellationToken cancellationToken)
        {
            try
            {
                // Log a message at the information level
                _logger.LogInformation("Performing customer discount search...");

                var discounts = await _customerDiscountRepository.Search(searchModel,cancellationToken);

                if (discounts != null && discounts.Any())
                    _logger.LogInformation("Customer discount search completed successfully. Total discounts found: {DiscountCount}", discounts.Count);
                else
                    _logger.LogWarning("No customer discounts found matching the search criteria.");

                return discounts!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while performing customer discount search.");
                throw; 
            }
        }
    }
}