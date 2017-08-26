CREATE TABLE [dbo].[PaymentRecord] 
(
    [PaymentId]      INT          IDENTITY (1001, 1) NOT NULL,
    [CardRecordId]        INT NOT NULL,
    PRIMARY KEY CLUSTERED ([PaymentId] ASC),
	CONSTRAINT [fk_paymentrecord_cardrecord] FOREIGN KEY ([CardRecordId]) REFERENCES [dbo].[CardRecord] ([Id])
);

