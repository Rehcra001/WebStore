CREATE PROCEDURE [dbo].[usp_AddProduct]
(
	@Name VARCHAR(100),
	@Decription VARCHAR(250),
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
			VALUES (@Name, @Decription, @Picture, @Price, @QtyInStock, @UnitPerId, @CategoryId);

			SET @ProductId = SCOPE_IDENTITY();

			SELECT ProductId, [Name], [Description], Picture, Price, QtyInStock, UnitPerId, CategoryId
			FROM dbo.Products
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
