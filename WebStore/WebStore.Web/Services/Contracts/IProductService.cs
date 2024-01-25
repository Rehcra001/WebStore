using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IProductService
    {
        Task<ProductDTO> AddProductAsync(ProductDTO productDTO);
        Task<IEnumerable<ProductCategoryDTO>> GetProductCategories();
        Task<IEnumerable<UnitPerDTO>> GetUnitPers();
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> UpdateProduct(ProductDTO productDTO);
    }
}
