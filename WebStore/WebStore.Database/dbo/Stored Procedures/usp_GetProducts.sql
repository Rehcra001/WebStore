CREATE PROCEDURE [dbo].[usp_GetProducts] AS
BEGIN
	SELECT p.ProductId, p.[Name], p.[Description], p.Picture, 
		   p.Price, p.QtyInStock, p.UnitPerId, p.CategoryId, 
		   u.UnitPer, pc.CategoryName
	FROM dbo.Products AS p
	INNER JOIN dbo.ProductCategories AS pc ON p.CategoryId = pc.ProductCategoryId
	INNER JOIN dbo.UnitPers AS u ON p.UnitPerId = u.UnitPerId;
END;
GO
