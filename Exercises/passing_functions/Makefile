Out.txt: main.exe
	mono main.exe > Out.txt

table.dll: table.cs
	mcs -target:library -out:./$@ $^

main.exe: main.cs table.dll
	mcs $< -reference:table.dll -target:exe -out:$@
	
