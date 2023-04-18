DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
MKSVG = gnuplot $(filter %.gpi, $^)
DIR = ../../math_resources

All: GraphB.svg GraphC.svg

GraphC.svg: rsspoints.data graphC.gpi
	$(MKSVG) >$@

GraphB.svg: comparison.data graphB.gpi
	$(MKSVG) >$@

Out.txt comparison.data rsspoints.data: main.exe
	mono $< -fileB1:comparison.data -fileC1:rsspoints.data 1>Out.txt

main.exe: main.cs matrix.dll montecarlo.dll genlist.dll
	$(MKEXE) 

montecarlo.dll: montecarlo.cs matrix.dll genlist.dll
	$(MKLIB)
matrix.dll: $(DIR)/matrix.cs $(DIR)/vec.cs
	$(MKLIB)
genlist.dll: $(DIR)/genlist.cs
	$(MKLIB)


	
clean: 
	rm -f *.dll *.exe *.svg *.data *.txt
