using AM._Application.Contracts.Auction;
using AM._Application.Contracts.Auction.DTO_s;
using AM._Domain.AccountAgg;
using AM._Domain.AuctionAgg;
using FrameWork.Application;
using FrameWork.Application.Authentication;
using FrameWork.Application.Messages;
using Microsoft.Extensions.Logging;
using SM._Application.Contracts.Product;
using SM._Domain.ProductAgg;

namespace AM._Application.Implementation
{
    
    public class AuctionApplication:IAuctionApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly ILogger<AuctionApplication> _logger;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProductRepository _productRepository;

        public AuctionApplication(IAuctionRepository auctionRepository, ILogger<AuctionApplication> logger, IAccountRepository accountRepository, IAuthHelper authHelper, IProductRepository productRepository)
        {
            _logger = logger;
            _authHelper = authHelper;
            _productRepository = productRepository;
            _auctionRepository = auctionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<OperationResult> Define(DefineAuction command, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            var product = await _productRepository.Get(command.ProductId, cancellationToken);
            if (await _auctionRepository.Exist(x => x.ProductId == command.ProductId, cancellationToken))
            {
                _logger.LogWarning("Duplicate record found for product ID: {ProductId}", command.ProductId);
                return result.Failed(ApplicationMessages.DuplicatedRecord);
            }
            product?.DeActive();
            var currentAccountId=_authHelper.CurrentAccountId();
            var endDate = command.EndDate.ToGeorgianDateTime();
            var auction = new Auction(command.ProductId, endDate, command.BasePrice,currentAccountId);

            try
            {
                await _auctionRepository.Create(auction, cancellationToken);
                await _auctionRepository.SaveChanges(cancellationToken);
                await _productRepository.SaveChanges(cancellationToken);
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

        public async Task<List<AuctionProcessModel>> GetEndedAuctionsProducts(CancellationToken cancellationToken)
        {
            return await _auctionRepository.GetEndedAuctionsProducts(cancellationToken);
        }

        public async Task<List<long>> GetAuctionsProductId(CancellationToken cancellationToken)
        {
            return await _auctionRepository.GetAuctionsProductId(cancellationToken);
        }

        public async Task SetMaxOffer(long productId, Bid bid, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAuctionBy(productId, cancellationToken);
            auction.AddBid(bid.SuggestionPrice,bid.CustomerId);
            await _auctionRepository.SaveChanges(cancellationToken);
        }

        public async Task EndAuctionStatus(AuctionProcessModel command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.Get(command.ProductId, cancellationToken);
            product?.Activate();
            var auction =await _auctionRepository.Get(command.ProductId, cancellationToken);
            var accountName = await _accountRepository.GetAccountName(command.CustomerId, cancellationToken);
            auction?.EndAuction(accountName);
            await _productRepository.SaveChanges(cancellationToken);
            await _auctionRepository.SaveChanges(cancellationToken);

        }
    }
}