CREATE TABLE [dbo].[Invoice] (
    [Id]       INT            IDENTITY (1001, 1) NOT NULL,
    [DateTime] DATETIME       NOT NULL,
    [Total]    MONEY          NOT NULL,
    [UserID]   NVARCHAR (128) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [fk_invoice_user] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

