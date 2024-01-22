CREATE PROCEDURE [dbo].[usp_GetAllUnitPers] AS
BEGIN
	SELECT UnitPerId, UnitPer
	FROM dbo.UnitPers;
END;
GO
