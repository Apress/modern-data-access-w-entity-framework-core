USE WWWingsV2_EN
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION dbo.[GetFlightsFromTVF]
(
	@departure nvarchar(30)
)
RETURNS TABLE
AS
RETURN

Select * from Flight where departure = @departure

GO


