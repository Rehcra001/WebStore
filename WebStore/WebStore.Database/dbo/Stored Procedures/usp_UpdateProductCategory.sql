CREATE PROCEDURE [dbo].[usp_UpdateProductCategory]
(
	@ProductCategoryId INT,
	@CategoryName NVARCHAR(100),
	@Picture VARBINARY(MAX)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN

			SET NOCOUNT ON;

			UPDATE dbo.ProductCategories
			SET CategoryName = @CategoryName,
				Picture = @Picture
			WHERE ProductCategoryId = @ProductCategoryId;

			SELECT ProductCategoryId, CategoryName
			FROM dbo.ProductCategories
			WHERE ProductCategoryId = @ProductCategoryId;

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
