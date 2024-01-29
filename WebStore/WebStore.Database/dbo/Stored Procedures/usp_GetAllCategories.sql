CREATE PROCEDURE [dbo].[usp_GetAllCategories] AS
BEGIN
	SELECT ProductCategoryId, CategoryName, Picture
	FROM dbo.ProductCategories;
END;
GO
