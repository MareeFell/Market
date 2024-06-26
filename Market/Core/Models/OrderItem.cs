﻿namespace Market.Models;

public class OrderItem
{
    public int Id { get; set; }
    
    public Order Order { get; set; }
    public Nomenclature Nomenclature { get; set; }
    
    public int Count { get; set; }
}