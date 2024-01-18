CREATE PROCEDURE [dbo].[usp_GetUnitPer]
(
	@UnitPerId INT
)AS
BEGIN
	SELECT UnitPerId, UnitPer
	FROM dbo.UnitPers
	WHERE UnitPerId = @UnitPerId;
END;
GO
