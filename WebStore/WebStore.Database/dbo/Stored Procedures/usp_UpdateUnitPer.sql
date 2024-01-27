CREATE PROCEDURE [dbo].[usp_UpdateUnitPer]
(
	@UnitPerId INT,
	@UnitPer NVARCHAR(10)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			
			SET NOCOUNT ON;

			UPDATE dbo.UnitPers
			SET UnitPer = @UnitPer
			WHERE UnitPerId = @UnitPerId;

			SELECT UnitPerId, UnitPer
			FROM dbo.UnitPers
			WHERE UnitPerId = @UnitPerId;

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
