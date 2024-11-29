using Amazon.DynamoDBv2.DataModel;
using Products.Api.Data;
using Products.Api.Dtos;

namespace Products.Api.Services;

public class ProductService(IDynamoDBContext ddb) : IProductService
{
    public async Task<Product> CreateProductAsync(CreateProductRequest request)
    {
        Product? product = await ddb.LoadAsync<Product>(request.Id);
        if (product != null) throw new($"Product with Id {request.Id} Already Exists");
        Product productToCreate = new()
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price,
        };
        await ddb.SaveAsync(productToCreate);
        return productToCreate;
    }

    public async Task DeleteProductAsync(string id)
    {
        Product? product = await ddb.LoadAsync<Product>(id);
        if (product == null) throw new($"Product with Id {id} Not Found");
        await ddb.DeleteAsync(product);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        List<Product>? products = await ddb.ScanAsync<Product>(default).GetRemainingAsync();
        return products;
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
        Product? product = await ddb.LoadAsync<Product>(id);
        if (product == null) throw new($"Product with Id {id} Not Found");
        return product;
    }

    public async Task<Product> UpdateProductAsync(UpdateProductRequest request)
    {
        Product? product = await ddb.LoadAsync<Product>(request.Id);
        if (product == null) throw new($"Product with Id {request.Id} Not Found");
        Product productToUpdate = new()
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price,
        };
        await ddb.SaveAsync(productToUpdate);
        return productToUpdate;
    }
}
