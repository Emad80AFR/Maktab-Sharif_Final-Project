using AM._Application.Contracts.Auction;
using AM._Application.Contracts.Auction.DTO_s;
using AM._Domain.AuctionAgg;
using FrameWork.Application;
using FrameWork.Application.Messages;
using Microsoft.Extensions.Logging;

namespace AM._Application.Implementation
{
    
    public class AuctionApplication:IAuctionApplication
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly ILogger<AuctionApplication> _logger;

        public AuctionApplication(IAuctionRepository auctionRepository, ILogger<AuctionApplication> logger)
        {
            _auctionRepository = auctionRepository;
            _logger = logger;
        }

        public async Task<OperationResult> Define(DefineAuction command, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            if (await _auctionRepository.Exist(x => x.ProductId == command.ProductId, cancellationToken))
            {
                _logger.LogWarning("Duplicate record found for product ID: {ProductId}", command.ProductId);
                return result.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var endDate = command.EndDate.ToGeorgianDateTime();
            var auction = new Auction(command.ProductId, endDate, command.BasePrice);

            try
            {
                await _auctionRepository.Create(auction, cancellationToken);
                await _auctionRepository.SaveChanges(cancellationToken);
                _logger.LogInformation("Auction defined successfully for product ID: {ProductId}", command.ProductId);
                return result.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while defining auction for product ID: {ProductId}", command.ProductId);
                return result.Failed(ApplicationMessages.ErrorOccurred);
            }
        }

        public async Task<OperationResult> Edit(EditAuction command, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            var auction = await _auctionRepository.Get(command.Id, cancellationToken);

            if (auction == null)
            {
                _logger.LogWarning("Auction not found for ID: {AuctionId}", command.Id);
                return result.Failed(ApplicationMessages.RecordNotFound);
            }

            if (await _auctionRepository.Exist(x => x.ProductId == auction.ProductId && x.Id != auction.Id, cancellationToken))
            {
                _logger.LogWarning("Duplicate record found for product ID: {ProductId}", auction.ProductId);
                return result.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var endDate = command.EndDate.ToGeorgianDateTime();
            auction.Edit(command.ProductId, endDate, command.BasePrice);

            try
            {
                await _auctionRepository.SaveChanges(cancellationToken);
                _logger.LogWarning("Auction edited successfully for ID: {AuctionId}", command.Id);
                return result.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occurred while editing auction for ID: {AuctionId}", command.Id);
                return result.Failed(ApplicationMessages.ErrorOccurred);
            }
        }

        public async Task<OperationResult> Activate(long id,CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            var auction = await _auctionRepository.Get(id, cancellationToken);

            if (auction == null)
            {
                _logger.LogWarning("Auction not found for ID: {AuctionId}", id);
                return result.Failed(ApplicationMessages.RecordNotFound);
            }

            auction.Activate();

            try
            {
                await _auctionRepository.SaveChanges(cancellationToken);
                _logger.LogWarning("Auction activated successfully for ID: {AuctionId}", id);
                return result.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occurred while activating auction for ID: {AuctionId}", id);
                return result.Failed(ApplicationMessages.ErrorOccurred);
            }
        }

        public async Task<OperationResult> DeActive(long id,CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            var auction = await _auctionRepository.Get(id, cancellationToken);

            if (auction == null)
            {
                _logger.LogWarning("Auction not found for ID: {AuctionId}", id);
                return result.Failed(ApplicationMessages.RecordNotFound);
            }

            auction.DeActive();

            try
            {
                await _auctionRepository.SaveChanges(cancellationToken);
                _logger.LogWarning("Auction deactivated successfully for ID: {AuctionId}", id);
                return result.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occurred while deactivating auction for ID: {AuctionId}", id);
                return result.Failed(ApplicationMessages.ErrorOccurred);
            }
        }

        public async Task<EditAuction> GetDetails(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await _auctionRepository.GetDetails(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occurred while retrieving auction details for ID: {AuctionId}", id);
                throw; 
            }
        }

        public async Task<List<AuctionViewModel>> Search(AuctionSearchModel searchModel, CancellationToken cancellationToken)
        {
            try
            {
                return await _auctionRepository.Search(searchModel, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occurred while searching auctions with search model: {@SearchModel}", searchModel);
                throw; 
            }
        }
    }
}