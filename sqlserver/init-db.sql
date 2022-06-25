CREATE DATABASE UnityExercise;
GO

USE UnityExercise;
GO

CREATE TABLE dbo.Payloads (
  [Id] [int] IDENTITY(1,1) PRIMARY KEY,
  [Message] [nvarchar](1024) NOT NULL
);
GO

-- run the following to confirm database is ready
INSERT INTO dbo.Payloads (Message)
VALUES
('{"message": "hello message"}');

SELECT * FROM dbo.Payloads;
GO
