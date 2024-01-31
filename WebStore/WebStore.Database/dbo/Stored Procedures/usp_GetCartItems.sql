CREATE PROCEDURE [dbo].[usp_GetCartItems]
(
	@EmailAddress NVARCHAR(100)
)AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CustomerId INT;
	DECLARE @CartId INT;

	SET @CustomerId = dbo.udf_GetCustomerIdWithEmail(@EmailAddress);
	SET @CartId = dbo.udf_GetCartIdWithCustomerId(@CustomerId);

	SELECT CartItemId, CartId, ProductId, Quantity
	FROM dbo.CartItems
	WHERE CartId = @CartId;

END;
GO