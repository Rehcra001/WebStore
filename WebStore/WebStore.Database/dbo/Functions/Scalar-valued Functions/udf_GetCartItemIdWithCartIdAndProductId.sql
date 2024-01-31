CREATE FUNCTION [dbo].[udf_GetCartItemIdWithCartIdAndProductId]
(
	@CartId INT,
	@ProductId INT
)
RETURNS INT
AS
BEGIN
	DECLARE @CartItemId INT;

	SELECT  @CartItemId = CartItemId
	FROM dbo.CartItems
	WHERE CartId = @CartId AND ProductId = @ProductId;

	RETURN @CartItemId;
END
