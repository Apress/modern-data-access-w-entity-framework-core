<Query Kind="Expression">
  <Connection>
    <ID>3fc9fc4c-1c0c-464c-b3c5-94fb8681a0c5</ID>
    <Persist>true</Persist>
    <Driver Assembly="EF7Driver" PublicKeyToken="469b5aa5a4331a8c">EF7Driver.StaticDriver</Driver>
    <CustomAssemblyPath>H:\TFS\Demos\EFC\EFC_Samples\EFC_WWWingsV1_Reverse(NETFX)\bin\Debug\EFC_WWWings1_Reverse(NETFX).dll</CustomAssemblyPath>
    <CustomTypeName>EFC_WWWings1_Reverse_NETFX_.WWWingsV1Context</CustomTypeName>
  </Connection>
</Query>

from f in Flight
where f.FreeSeats > 100
select f
