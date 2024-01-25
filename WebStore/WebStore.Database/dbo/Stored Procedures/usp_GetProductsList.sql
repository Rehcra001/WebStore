CREATE PROCEDURE [dbo].[usp_GetProductsList] AS
BEGIN
	SELECT p.ProductId, p.[Name]
	FROM dbo.Products AS p;
END;
GO
