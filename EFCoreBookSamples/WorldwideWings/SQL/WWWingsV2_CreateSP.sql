USE WWWingsV2_EN
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure dbo.[GetFlightsFromSP]
(
	@departure nvarchar(30)
)
AS
Select * from Flight where departure = @departure
GO