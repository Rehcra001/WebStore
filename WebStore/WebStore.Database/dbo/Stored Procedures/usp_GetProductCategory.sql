CREATE PROCEDURE [dbo].[usp_GetProductCategory]
(
	@ProductCategoryId INT
)AS
BEGIN
	SET NOCOUNT ON;

	SELECT ProductCategoryId, CategoryName, Picture
	FROM dbo.ProductCategories
	WHERE ProductCategoryId = @ProductCategoryId;
END;
GO

