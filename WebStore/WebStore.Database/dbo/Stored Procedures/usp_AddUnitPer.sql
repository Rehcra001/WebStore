CREATE PROCEDURE [dbo].[usp_AddUnitPer]
(
	@UnitPer NVARCHAR(10)
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		DECLARE @UnitPerId INT;

		INSERT INTO dbo.UnitPers (UnitPer)
		VALUES (@UnitPer);

		SET @UnitPerId = SCOPE_IDENTITY();

		SELECT UnitPerId, UnitPer
		FROM dbo.UnitPers
		WHERE UnitPerId = @UnitPerId;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
	END CATCH;
END;
GO
