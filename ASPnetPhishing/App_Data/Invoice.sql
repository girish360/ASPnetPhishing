CREATE TABLE [dbo].[Invoice] (
    [Id]       INT            IDENTITY (1001, 1) NOT NULL,
    [DateTime] DATETIME       NOT NULL,
    [Total]    MONEY          NOT NULL,
    [UserID]   NVARCHAR (128) NOT NULL,
    [PaymentId] INT NULL, 
    [ShippingId] INT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [fk_invoice_user] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT fk_invoice_payment FOREIGN KEY (PaymentId) REFERENCES Payment(PaymentId),
	CONSTRAINT fk_invoice_shipping FOREIGN KEY (ShippingId) REFERENCES Shipping(Id)
);

