﻿namespace DM.Application.Contracts.ColleagueDiscount.DTO_s
{
    public class ColleagueDiscountViewModel
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string? Product { get; set; }
        public int DiscountRate { get; set; }
        public bool IsRemoved { get; set; }
        public string CreationDate { get; set; }
    }
}
