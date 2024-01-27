using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IProductService
    {
        Task<ProductDTO> AddProductAsync(ProductDTO productDTO);
        Task<ProductCategoryDTO> AddCategoryAsync(ProductCategoryDTO productCategoryDTO);
        Task<UnitPerDTO> AddUnitPerAsync(UnitPerDTO unitPerDTO);
        Task<IEnumerable<ProductCategoryDTO>> GetProductCategoriesAsync();
        Task<IEnumerable<UnitPerDTO>> GetUnitPersAsync();
        Task<IEnumerable<ProductDTO>> GetProductsAsync();
        Task<ProductDTO> UpdateProductAsync(ProductDTO productDTO);
        Task<ProductCategoryDTO> UpdateProductCategoryAsync(ProductCategoryDTO productCategoryDTO);
        Task<UnitPerDTO> UpdateUnitPerAsync(UnitPerDTO unitPerDTO);
    }
}
