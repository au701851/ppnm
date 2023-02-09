Out.txt: main.exe
	mono main.exe > Out.txt
	#mono kører main filen
	#> means override, the >> just adds to bottom
#make kører første rule i Makefile-filen

vec.dll: vec.cs
	mcs vec.cs -target:library -out:vec.dll 

main.exe: main.cs vec.dll
	mcs main.cs -reference:vec.dll -target:exe -out:main.exe
