CREATE PROCEDURE [dbo].[usp_GetAddressLines]
(
	@EmailAddress NVARCHAR(100)
)AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CustomerId INT;
	SET @CustomerId = dbo.udf_GetCustomerIdWithEmail(@EmailAddress);

	SELECT AddressId, AddressLine1
	FROM Addresses
	WHERE CustomerId = @CustomerId
	ORDER BY AddressId;

END;
GO
