﻿CREATE TABLE [dbo].[Cart]
(
	CartId INT NOT NULL PRIMARY KEY IDENTITY,
	CustomerId INT NOT NULL UNIQUE,
	CONSTRAINT FK_Cart_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(CustomerId)
)
