﻿CREATE PROCEDURE [dbo].[usp_AddProduct]
(
	@Name VARCHAR(100),
	@Description VARCHAR(250),
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

			DECLARE @ProductId INT;

			INSERT INTO dbo.Products ([Name], [Description], Picture, Price, QtyInStock, UnitPerId, CategoryId)
			VALUES (@Name, @Description, @Picture, @Price, @QtyInStock, @UnitPerId, @CategoryId);

			SET @ProductId = SCOPE_IDENTITY();

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
