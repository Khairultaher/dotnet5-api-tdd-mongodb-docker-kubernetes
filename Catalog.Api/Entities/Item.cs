using System;

namespace Catalog.Api.Entities
{
    //public record Item
    //{
    //    public Guid Id { get; init; }
    //    public string Name { get; init; }
    //    public decimal Price { get; init; }
    //    public DateTimeOffset CreatedDate { get; init; }
    //}

    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}