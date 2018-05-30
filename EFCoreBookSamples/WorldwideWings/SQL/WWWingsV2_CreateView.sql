USE WWWingsV2_EN
GO
CREATE VIEW dbo.[V_DepartureStatistics]
AS
SELECT departure, COUNT(FlightNo) AS FlightCount
FROM dbo.Flight
GROUP BY departure
GO