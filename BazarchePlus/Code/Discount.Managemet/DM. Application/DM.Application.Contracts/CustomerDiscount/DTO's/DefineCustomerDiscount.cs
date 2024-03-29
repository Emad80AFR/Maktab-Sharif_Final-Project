﻿using System.ComponentModel.DataAnnotations;
using FrameWork.Application.Messages;
using SM._Application.Contracts.Product.DTO_s;

namespace DM._Application.Contracts.CustomerDiscount.DTO_s
{
    public class DefineCustomerDiscount
    {
        [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
        public long ProductId { get; set; }
        public long SellerId { get; set; }

        [Range(1, 99, ErrorMessage = ValidationMessages.IsRequired)]
        public int DiscountRate { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string StartDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string EndDate { get; set; }

        public string Reason { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
