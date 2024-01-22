CREATE PROCEDURE [dbo].[usp_GetAllCategories] AS
BEGIN
	SELECT ProductCategoryId, CategoryName
	FROM dbo.ProductCategories;
END;
GO
