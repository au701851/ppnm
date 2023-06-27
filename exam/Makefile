DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
MKSVG = gnuplot $(filter %.gpi, $^) >$@
DIR = ../math_resources


All: Out.txt Derivatives.svg

Derivatives.svg: graph.gpi derivatives.data
	$(MKSVG)

Out.txt derivatives.data: main.exe
	mono $< >$@ -file1:Derivatives

main.exe: main.cs matrix.dll subspline.dll
	$(MKEXE) 


subspline.dll: subspline.cs matrix.dll 
	$(MKLIB)

matrix.dll: $(DIR)/matrix.cs $(DIR)/vec.cs
	$(MKLIB)



	
clean: 
	rm -f *.dll *.exe *.svg *.txt *.data
