CREATE PROCEDURE [dbo].[usp_UpdateCompanyEFT]
(
	@EFTId INT,
	@Bank NVARCHAR(100),
	@AccountType NVARCHAR(50),
	@AccountNumber NVARCHAR(25),
	@BranchCode NVARCHAR(15)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.CompanyEFTDetail
			SET Bank = @Bank,
				AccountType = @AccountType,
				AccountNumber = @AccountNumber,
				BranchCode = @BranchCode
			WHERE EFTId = @EFTId;

			SELECT EFTId, Bank, AccountType, AccountNumber, BranchCode
			FROM dbo.CompanyEFTDetail
			WHERE EFTId = @EFTId;
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
