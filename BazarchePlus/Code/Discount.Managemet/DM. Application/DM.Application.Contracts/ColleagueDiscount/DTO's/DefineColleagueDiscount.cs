using System.ComponentModel.DataAnnotations;
using FrameWork.Application;
using SM._Application.Contracts.Product.DTO_s;

namespace DM.Application.Contracts.ColleagueDiscount.DTO_s
{
    public class DefineColleagueDiscount
    {
        [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
        public long ProductId { get; set; }

        [Range(1, 99, ErrorMessage = ValidationMessages.IsRequired)]
        public int DiscountRate { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
