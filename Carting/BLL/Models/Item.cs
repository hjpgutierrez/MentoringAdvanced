﻿namespace Carting.BLL.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
