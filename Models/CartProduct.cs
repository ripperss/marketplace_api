﻿namespace marketplace_api.Models;

public class CartProduct
{
    public int Id { get; set; }

    public int CartId { get; set; } 

    public int ProductId { get; set; } 

    public int Quantity { get; set; } 
}
