CREATE TABLE [dbo].Question
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomerId] NVARCHAR(128) NOT NULL, 
    [Question] TEXT NOT NULL, 
    [Answer] TEXT NULL, 
    [IsAnswered] BIT NOT NULL DEFAULT 0, 
	CONSTRAINT fk_question_aspnetusers FOREIGN KEY (CustomerId)
	REFERENCES AspNetUsers(Id)
)