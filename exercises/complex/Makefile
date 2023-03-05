Out.txt: main.exe
	mono main.exe > Out.txt

CMATHDIR = ~/repos/ppnm/Exercises/complex
cmath.dll: $(CMATHDIR)/cmath.cs $(CMATHDIR)/complex.cs
	mcs -target:library -out:./$@ $^

main.exe: main.cs cmath.dll
	mcs main.cs -reference:cmath.dll -target:exe -out:main.exe
