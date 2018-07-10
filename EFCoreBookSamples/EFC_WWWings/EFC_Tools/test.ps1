$cs = "Server=E62;Database=WWWingsV2_EN;Trusted_Connection=True;MultipleActiveResultSets=True;App
=Entityframework"

. "$PSScriptRoot\bin\debug\EFC_Tools.exe" migrate -c $cs 
. "$PSScriptRoot\bin\debug\EFC_Tools.exe" createtestdata -connectionstring=$cs --flightcount=100 --passengercount=100 --pilotcount=10