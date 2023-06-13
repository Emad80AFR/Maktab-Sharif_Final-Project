using AM._Application.Contracts.Auction.DTO_s;
using FrameWork.Domain;

namespace AM._Domain.AuctionAgg;

public interface IAuctionRepository:IBaseRepository<long,Auction>
{
    Task<EditAuction> GetDetails(long id, CancellationToken cancellationToken);
    Task<List<AuctionViewModel>> Search(AuctionSearchModel searchModel, CancellationToken cancellationToken);
}