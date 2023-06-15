DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
MKSVG = gnuplot $(filter %.gpi, $^)
DIR = ../../math_resources


All: Out.txt


Out.txt: main.exe
	mono $< 1>Out.txt

main.exe: main.cs matrix.dll
	$(MKEXE) 



matrix.dll: $(DIR)/matrix.cs $(DIR)/vec.cs
	$(MKLIB)

	
clean: 
	rm -f *.dll *.exe *.svg *.txt
