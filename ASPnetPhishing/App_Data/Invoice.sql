CREATE TABLE [dbo].[Invoice] (
    [Id]         INT            IDENTITY (1001, 1) NOT NULL,
    [DateTime]   DATETIME       NOT NULL,
    [Total]      MONEY          NULL,
    [UserID]     NVARCHAR (128) NOT NULL,
    [PaymentId]  INT            NULL,
    [ShippingId] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [fk_invoice_paymentrecord] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[PaymentRecord] ([PaymentId]),
    CONSTRAINT [fk_invoice_shipping] FOREIGN KEY ([ShippingId]) REFERENCES [dbo].[Shipping] ([Id]),
    CONSTRAINT [fk_invoice_user] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

