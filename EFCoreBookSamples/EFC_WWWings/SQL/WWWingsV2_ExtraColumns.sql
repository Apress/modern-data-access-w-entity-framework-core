USE WWWingsV2_EN
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Airline ADD
	Address nvarchar(100) NULL,
	FoundingYear int NULL,
	Bunkrupt bit NULL
GO
ALTER TABLE dbo.Airline SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
