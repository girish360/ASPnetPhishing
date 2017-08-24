CREATE TABLE [dbo].[Product] (
    [Id]          INT          IDENTITY (1001, 1) NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    [Description] TEXT         NOT NULL,
    [Price]       MONEY        NOT NULL,
    [Cost]        MONEY        NOT NULL,
    [CategoryId]  INT          NOT NULL,
    [ImageFilename] VARCHAR(100) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [fk_product_productcategory] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[ProductCategory] ([Id])
);
