CREATE PROCEDURE [dbo].[usp_GetProductCategory]
(
	@ProductCategoryId INT
)AS
BEGIN
	SET NOCOUNT ON;

	SELECT ProductCategoryId, CategoryName
	FROM dbo.ProductCategories
	WHERE ProductCategoryId = @ProductCategoryId;
END;
GO

