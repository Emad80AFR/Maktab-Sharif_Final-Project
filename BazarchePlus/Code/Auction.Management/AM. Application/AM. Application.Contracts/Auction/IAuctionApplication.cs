using AM._Application.Contracts.Auction.DTO_s;
using FrameWork.Application;

namespace AM._Application.Contracts.Auction;

public interface IAuctionApplication
{
    Task<OperationResult> Define(DefineAuction command, CancellationToken cancellationToken);
    Task<OperationResult> Edit(EditAuction command, CancellationToken cancellationToken);
    Task<OperationResult> Activate( CancellationToken cancellationToken);
    Task<OperationResult> DeActive( CancellationToken cancellationToken);
    Task<EditAuction> GetDetails(long id, CancellationToken cancellationToken);
    Task<List<AuctionViewModel>> Search(AuctionSearchModel searchModel, CancellationToken cancellationToken);
}