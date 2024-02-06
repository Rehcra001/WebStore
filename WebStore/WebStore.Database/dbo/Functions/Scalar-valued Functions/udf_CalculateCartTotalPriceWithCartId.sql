CREATE FUNCTION [dbo].[udf_CalculateCartTotalPriceWithCartId]
(
	@CartId INT
)
RETURNS INT
AS
BEGIN
	DECLARE @TotalPrice MONEY;

	SELECT @TotalPrice = SUM(CI.Quantity * PR.Price)
	FROM dbo.CartItems AS CI
	INNER JOIN Products AS PR ON CI.ProductId = PR.ProductId
	WHERE CartId = @CartId;

	RETURN @TotalPrice;
END
