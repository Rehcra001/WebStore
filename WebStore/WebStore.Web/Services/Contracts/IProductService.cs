using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IProductService
    {
        Task<ProductDTO> AddProductAsync(ProductDTO productDTO);
    }
}
