CREATE TABLE [dbo].[Payment] (
    [PaymentId]      INT          NOT NULL IDENTITY(1001, 1),
    [InvoiceID]      INT          NOT NULL,
    [CardNum]        VARCHAR (16) NOT NULL,
    [CCV]            VARCHAR (3)  NOT NULL,
    [ExpDate]        DATE         NOT NULL,
    [BillingAddress] VARCHAR (50) NOT NULL,
    [BillingCity]    VARCHAR (50) NOT NULL,
    [BillingState]   VARCHAR (2)  NOT NULL,
    [BillingZip]     VARCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([PaymentId] ASC),
    CONSTRAINT [fk_payment_invoice] FOREIGN KEY ([InvoiceID]) REFERENCES [dbo].[Invoice] ([Id])
);

