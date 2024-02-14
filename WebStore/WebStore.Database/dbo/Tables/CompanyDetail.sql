CREATE TABLE [dbo].[CompanyDetail]
(
	CompanyId INT NOT NULL PRIMARY KEY IDENTITY,
	CompanyName NVARCHAR(100) NOT NULL,
	CompanyLogo VARBINARY(MAX) NOT NULL,
	AddressId INT,
	EFTId INT,
	PhoneNumber NVARCHAR (100) NOT NULL,
	EmailAddress NVARCHAR(100) NOT NULL,
	CONSTRAINT UQ_CompanyName UNIQUE (CompanyName),
	CONSTRAINT UQ_AddressId UNIQUE (AddressId),
	CONSTRAINT UQ_EFTId UNIQUE (EFTId),
	CONSTRAINT FK_CompanyDetail_Addresses_AddressId FOREIGN KEY (AddressId) REFERENCES dbo.Addresses(AddressId),
	CONSTRAINT FK_CompanyDetail_CompnayEFTDetail_EFTId FOREIGN KEY (EFTId) REFERENCES dbo.CompanyEFTDetail(EFTId)
)
