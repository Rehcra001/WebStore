﻿CREATE TABLE [dbo].[Customers]
(
	CustomerId INT NOT NULL PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	EmailAddress NVARCHAR(100) NOT NULL,
	PhoneNumber NVARCHAR(100) NOT NULL,
	CONSTRAINT [UN_Customer_EmailAddress] UNIQUE (EmailAddress)
)

