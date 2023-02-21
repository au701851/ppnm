Out.txt: main.exe input.txt
	mono main.exe < input.txt > Out.txt

main.exe: main.cs genlist.dll
	mcs main.cs -target:exe -reference:genlist.dll -out:main.exe

genlist.dll: genlist.cs
	mcs $< -target:library
