Out.txt: main.exe
	mono main.exe > Out.txt
	#mono kører main filen
	#> means override, the >> just adds to bottom
#make kører første rule i Makefile-filen

main1.exe: main.cs sfuns.cs
	mcs main.cs sfuns.cs -target:exe -out:main.exe 
#main.exe laves ved at oversætte main.cs

math.dll: main.cs sfuns.cs
	mcs main.cs sfuns.cs -target:library -out:math.dll 

main.exe: main.cs math.dll
	mcs main.cs -reference:math.dll -target:exe -out:main.exe
