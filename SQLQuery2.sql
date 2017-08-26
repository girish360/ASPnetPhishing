CREATE VIEW [dbo].vwInvoice
	AS 
	SELECT 
	i.DateTime AS "Invoice Date", 
	i.Total AS "Invoice Total", 
	u.Email AS "Customer Email", 
	cr.CardNumber AS "Card Number",
	sh.ShippingAddress AS "Ship To",
	sh.ShippingCity AS "City",
	sh.ShippingState AS "State",
	sh.ShippingZipCode AS "Zip Code",
	sh.ShippingPhone AS "Phone",
	sh.ShippingEmail AS "Email"
	FROM Invoice i
	INNER JOIN AspNetUsers u ON u.Id=i.UserID
	INNER JOIN Shipping sh ON sh.Id=i.ShippingId
	INNER JOIN PaymentRecord pr ON pr.PaymentId=i.PaymentId
	INNER JOIN CardRecord cr ON cr.Id=pr.CardRecordId 
