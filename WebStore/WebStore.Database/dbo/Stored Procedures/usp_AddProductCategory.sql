CREATE PROCEDURE [dbo].[usp_AddProductCategory]
(
	@CategoryName NVARCHAR(100)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			DECLARE @CategoryId INT;

			INSERT INTO dbo.ProductCategories (CategoryName)
			VALUES (@CategoryName);

			SET @CategoryId = SCOPE_IDENTITY();

			SELECT ProductCategoryId, CategoryName
			FROM ProductCategories
			WHERE ProductCategoryId = @CategoryId;
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
