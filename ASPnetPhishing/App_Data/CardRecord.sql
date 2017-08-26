CREATE TABLE [dbo].CardRecord
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1001, 1), 
    [CustomerId] NVARCHAR(128) NOT NULL, 
    [CardNumber] VARCHAR(16) NOT NULL, 
    [CCV] VARCHAR(3) NOT NULL, 
    [ExpDate] VARCHAR(5) NOT NULL, 
    [BillingAddress] VARCHAR(100) NOT NULL, 
    [BillingCity] VARCHAR(50) NOT NULL, 
    [BillingState] VARCHAR(2) NOT NULL, 
    [BillingZip] VARCHAR(10) NOT NULL, 
    [BillingEmail] NVARCHAR(256) NULL,
	CONSTRAINT fk_cardrecord_aspnetusers FOREIGN KEY (CustomerId) REFERENCES AspNetUsers(Id)
)
