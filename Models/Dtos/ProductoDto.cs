using System;

namespace webApi.Models.Dtos;

public class ProductoDto
{

    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description {get; set;}
    public decimal Price { get; set;}
    public string ImageUrl { get; set;} = string.Empty;
    public string SKU { get; set;} = string.Empty;
    public int Stock { get; set;}
    public DateTime CreationDate { get; set;} = DateTime.Now;
    public DateTime? UpdateDate { get; set; }
    public int CategoryId {get; set;}

}
