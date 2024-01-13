CREATE PROCEDURE [dbo].[usp_GetCustomer]
(
	@EmailAddress NVARCHAR(100)
)AS
BEGIN
	SET NOCOUNT ON;

	--Get the customer
	SELECT CustomerId, FirstName, LastName, EmailAddress, PhoneNumber
	FROM Customers
	WHERE EmailAddress = @EmailAddress;

	--Get the customer address/es
	;WITH cust AS
	(
		SELECT CustomerId
		FROM Customers
		WHERE EmailAddress = @EmailAddress
	)
	SELECT ad.AddressId, ad.AddressLine1, ad.AddressLine2, ad.Suburb, ad.City,ad.PostalCode, ad.Country, ad.CustomerId
	FROM Addresses AS ad
	INNER JOIN cust ON ad.CustomerId = cust.CustomerId;
END;
GO