namespace WebshopPractice.Server.Data.DTO;

public class ProductDTO
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}
