Entity Framework Core Code Samples
-----------------------------------------
Author: Dr. Holger Schwichtenberg, www.EFCore.net
Version: 01.01.2018 for version 4.3 EN of the book "Modern Data Access with Entity Framework Core"

Notes
--------------
The assembly /Lib/ITV_DemoUtil.dll contains some helper routines used in the samples.

How to create the databases?
--------------
Option 1)
Run the SQL Scripts in the "SQL" Folder

Option 2) - only for Version 2 
a) Run Update-Database in the package manager console on the Project EFC_DA using EFC_Tools as start project
b) Call DataGenerator.Run() in main() of EFC_Console and compile + start EFC_Console.exe