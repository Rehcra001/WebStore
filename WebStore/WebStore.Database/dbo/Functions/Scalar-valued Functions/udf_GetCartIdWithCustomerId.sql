CREATE FUNCTION [dbo].[udf_GetCartIdWithCustomerId]
(
	@CustomerId INT
)
RETURNS INT
AS
BEGIN
	DECLARE @CartId INT;

	SELECT @CartId = CartId
	FROM dbo.Cart
	WHERE CustomerId = @CustomerId;

	RETURN @CartId;
END
