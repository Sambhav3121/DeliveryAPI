public class Cart
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public double Price { get; set; } 
    public int Amount { get; set; } 
    public string Image { get; set; }
    public double TotalPrice {get; set;}
    public Guid dishId { get; set; }
    public string UserId { get; set; } 
    public string? OrderId { get; set; } 
}
