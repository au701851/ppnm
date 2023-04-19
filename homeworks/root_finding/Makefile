DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
MKSVG = gnuplot $(filter %.gpi, $^)
DIR = ../../math_resources


All: GraphB1.svg GraphB2.svg

Out.txt hydrogen.data: main.exe
	mono $< 1>Out.txt -fileB1:hydrogen.data -fileB2:convergence.data

GraphB1.svg: graphB.gpi hydrogen.data
	$(MKSVG) >$@
GraphB2.svg: graphB2.gpi convergence.data
	$(MKSVG) >$@

main.exe: main.cs matrix.dll roots.dll rungekutta.dll genlist.dll
	$(MKEXE) 



roots.dll: roots.cs lineq.dll matrix.dll
	$(MKLIB)
matrix.dll: $(DIR)/matrix.cs $(DIR)/vec.cs
	$(MKLIB)
lineq.dll: $(DIR)/lineq.cs matrix.dll
			$(MKLIB)
genlist.dll: $(DIR)/genlist.cs
	$(MKLIB)

rungekutta.dll: ../../homeworks/ODE/rungekutta.cs matrix.dll genlist.dll
	$(MKLIB)
	
clean: 
	rm -f *.dll *.exe *.svg *.data *.txt
