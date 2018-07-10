Entity Framework Core Code Samples
------------------------------------------
Author: Dr. Holger Schwichtenberg, www.EFCore.net

Notes
------------------------------------------
1. Unfortunately, APress renumbered the chapters of my book during type setting. The folder numbers in this solution cannot change as the code is needed for annother book as well.

The folders have meaningful names. However, if you want to use the numbers:
Chapter 4 in the book is folder 10 in this solution.
Chapter 5 in the book is folder 11 in this solution.
and so on
Chapter 16 in the book is folder 22 und 23 in this solution.
Chapter 17 in the book is folder 24
Chapter 18 in the book is folder 25
Chapter 19 in the book is folder 26
Chapter 20 in the book is folder 27
Appendix A: see other Visual Studio solutions folders
Appendix B: http://www.efcore.net/DOTNETDOKTOR/EFCore/Links

2.
The assembly /Lib/ITV_DemoUtil.dll contains some helper routines used in the samples.

How to create the databases?
------------------------------------------
Option 1)
Run the two SQL Scripts in the "SQL" Folder

Option 2) - only for Version 2 of the World Wide Wings Schema
a) Run Update-Database in the Visual Studio Package Manager Console (OPMC) on the project "EFC_DA" using "EFC_Tools" as start project
b) Call DataGenerator.Run() in main() of EFC_Console and compile + start EFC_Console.exe
c) run all SQL scripts in "/SQL/Additional Scripts for V2 Forward Engineering"