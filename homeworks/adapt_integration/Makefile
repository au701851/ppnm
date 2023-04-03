DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
MKSVG = gnuplot $(filter %.gpi, $^)
DIR = ../../math_resources
ERFDIR = ../../exercises/plots

All: Erf.svg Out.txt

Erf.svg: erf.txt $(ERFDIR)/tab_error.txt erf.gpi
	$(MKSVG)

Out.txt erf.txt: main.exe
	mono $< -fileA:erf.txt 1>Out.txt

main.exe: main.cs adapt_integrate.dll
	$(MKEXE) 


adapt_integrate.dll: adapt_integrate.cs
	$(MKLIB)

	
clean: 
	rm -f *.dll *.exe *.svg *.data *.txt
