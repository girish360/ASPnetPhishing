CREATE TABLE [dbo].Shipping
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1001, 1), 
    [CustomerId] NVARCHAR(128) NOT NULL, 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [ShippingAddress] VARCHAR(100) NOT NULL, 
    [ShippingCity] VARCHAR(50) NOT NULL, 
    [ShippingState] VARCHAR(2) NOT NULL, 
    [ShippingZipCode] VARCHAR(10) NOT NULL, 
    [ShippingPhone] VARCHAR(14) NULL, 
    [ShippingEmail] VARCHAR(100) NULL,
	CONSTRAINT	fk_shipping_aspnetuser FOREIGN KEY (CustomerId)
	REFERENCES AspNetUsers(Id)
)
