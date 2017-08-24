CREATE TABLE [dbo].[LineItem] (
    [LineItemId] INT   NOT NULL IDENTITY,
    [InvoiceId]  INT   NOT NULL,
    [ProductId]  INT   NOT NULL,
    [Qty]        INT   NOT NULL,
    [LineTotal]  MONEY NOT NULL,
    PRIMARY KEY CLUSTERED ([LineItemId] ASC),
    CONSTRAINT [fk_lineitem_invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id]),
    CONSTRAINT [fk_lineitem_product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

