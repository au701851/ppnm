DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
MKSVG = gnuplot $(filter %.gpi, $^)
DIR = ../../math_resources


All: Out.txt GraphB.svg


Out.txt fileB.txt: main.exe
	mono $< < higgs.data -fileB:fileB.txt 1>Out.txt

main.exe: main.cs mini.dll matrix.dll genlist.dll
	$(MKEXE) 

GraphB.svg: graphB.gpi fileB.txt
	$(MKSVG) >$@



matrix.dll: $(DIR)/matrix.cs $(DIR)/vec.cs
	$(MKLIB)
lineq.dll: $(DIR)/lineq.cs matrix.dll
	$(MKLIB)
genlist.dll: $(DIR)/genlist.cs 
	$(MKLIB)

mini.dll: mini.cs matrix.dll
	$(MKLIB)
	
clean: 
	rm -f *.dll *.exe *.svg *.txt
