namespace BP._Query.Contracts.Slide
{
    public interface ISlideQuery
    {
        Task<List<SlideQueryModel>> GetSlides(CancellationToken cancellationToken);
    }
}
