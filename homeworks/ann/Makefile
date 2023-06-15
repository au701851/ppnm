DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
MKSVG = gnuplot $(filter %.gpi, $^) >$@
DIR = ../../math_resources
MINIDIR = ../minimisation


All: GraphA.svg GraphB.svg

GraphB.svg: graphB.gpi derivatives.data
	$(MKSVG)

GraphA.svg: graphA.gpi intervalues1.data intervalues2.data tabvalues.data
	$(MKSVG)

Out.txt intervalues1.data intervalues2.data tabvalues.data derivatives.data: main.exe
	mono $< -fileTab:tabvalues.data -fileInterp:intervalues.data -fileB:derivatives.data 1>Out.txt

main.exe: main.cs matrix.dll ann.dll
	$(MKEXE) 


ann.dll: ann.cs matrix.dll mini.dll
	$(MKLIB)

matrix.dll: $(DIR)/matrix.cs $(DIR)/vec.cs
	$(MKLIB)

mini.dll: $(MINIDIR)/mini.cs matrix.dll
	$(MKLIB)
	
clean: 
	rm -f *.dll *.exe *.svg *.txt *.data
