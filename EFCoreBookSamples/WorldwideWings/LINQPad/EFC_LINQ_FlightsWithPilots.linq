<Query Kind="SQL">
  <Connection>
    <ID>e2da098d-431c-41ab-998d-96d512228b8c</ID>
    <Persist>true</Persist>
    <Driver Assembly="EF7Driver" PublicKeyToken="469b5aa5a4331a8c">EF7Driver.StaticDriver</Driver>
    <CustomAssemblyPath>H:\TFS\Demos\EFC\EFC_Samples\EFC_WWingsV1_Reverse\bin\Debug\netstandard2.0\EFC_WWWingsV1_Reverse.dll</CustomAssemblyPath>
    <CustomTypeName>EFC_WWWingsV1_Reverse.WWWingsV1Context</CustomTypeName>
  </Connection>
</Query>

SELECT [f.PilotPerson].[PersonID], [f.PilotPerson].[FlightHours], [f.PilotPerson].[FlightSchool], [f.PilotPerson].[Flightscheintyp], [f.PilotPerson].[LicenseDate], [f].[FlightNo], [f].[Departure], [f].[Destination]
FROM [Operation].[Flight] AS [f]
LEFT JOIN [People].[Pilot] AS [f.PilotPerson] ON [f].[Pilot_PersonID] = [f.PilotPerson].[PersonID]
WHERE [f].[Departure] = N'Berlin'
GO

