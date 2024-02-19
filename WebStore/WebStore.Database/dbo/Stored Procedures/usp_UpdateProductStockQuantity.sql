CREATE PROCEDURE [dbo].[usp_UpdateProductStockQuantity]
(
	@OrderQuantities udtOrderQuantitiesTable READONLY
)AS
BEGIN
	;WITH productQuantity AS
	(
		SELECT PR.ProductId, PR.QtyInStock, (PR.QtyInStock - OQ.QuantityOrdered) AS RemainingStock
		FROM dbo.Products AS PR
		INNER JOIN @OrderQuantities AS OQ ON PR.ProductId = OQ.ProductId
	)
	UPDATE productQuantity
	SET QtyInStock = RemainingStock;
END;
GO
