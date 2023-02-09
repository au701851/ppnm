Out.txt: main.exe
	mono main.exe > Out.txt
	#mono kører main filen
	#> means override, the >> just adds to bottom
#make kører første rule i Makefile-filen

epsilon.dll: epsilon.cs calculations.cs
	mcs epsilon.cs calculations.cs -target:library -out:epsilon.dll 

main.exe: epsilon.cs epsilon.dll
	mcs epsilon.cs -reference:epsilon.dll -target:exe -out:main.exe
