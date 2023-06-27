using AM._Application.Contracts.Auction.DTO_s;
using FrameWork.Application;

namespace AM._Application.Contracts.Auction;

public interface IAuctionApplication
{
    Task<OperationResult> Define(DefineAuction command, CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditAuction command, CancellationToken cancellationToken);
    Task<OperationResult> Activate(long id, CancellationToken cancellationToken);
    Task<OperationResult> DeActive(long id, CancellationToken cancellationToken);
    Task<EditAuction> GetDetails(long id, CancellationToken cancellationToken);
    Task<List<AuctionViewModel>> Search(AuctionSearchModel searchModel, CancellationToken cancellationToken);
    Task<List<AuctionProcessModel>> GetEndedAuctionsProducts(CancellationToken cancellationToken);
    Task<List<long>> GetAuctionsProductId(CancellationToken cancellationToken);
    Task SetMaxOffer(long productId, Bid bid, CancellationToken cancellationToken);
    Task EndAuctionStatus(AuctionProcessModel command, CancellationToken cancellationToken);

}