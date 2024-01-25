CREATE PROCEDURE [dbo].[usp_UpdateProduct]
(
	@ProductId INT,
	@Name NVARCHAR(100),
	@Description NVARCHAR(250),
	@Picture VARBINARY(MAX),
	@Price MONEY,
	@QtyInStock INT,
	@UnitPerId INT,
	@CategoryId INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.Products
			SET [Name] = @Name,
				[Description] = @Description,
				Picture = @Picture,
				Price = @Price,
				QtyInStock = @QtyInStock,
				UnitPerId = @UnitPerId,
				CategoryId = @CategoryId
			WHERE ProductId = @ProductId;

			SELECT p.ProductId, p.[Name], p.[Description], p.Picture, 
				   p.Price, p.QtyInStock, p.UnitPerId, p.CategoryId, 
				   u.UnitPer, pc.CategoryName
			FROM dbo.Products AS p
			INNER JOIN dbo.ProductCategories AS pc ON p.CategoryId = pc.ProductCategoryId
			INNER JOIN dbo.UnitPers AS u ON p.UnitPerId = u.UnitPerId
			WHERE ProductId = @ProductId;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
	END CATCH;
END;
GO
