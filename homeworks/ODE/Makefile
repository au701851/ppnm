DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
DIR = ../../math_resources

all: GraphA.svg Pendulum.svg

GraphA.svg: graphA.gpi Out.txt diffeq1.data
	gnuplot $<

Pendulum.svg: pendulum.gpi Out.txt diffeq3.data
	gnuplot $<

LotkaVolterra.svg: lotkavolterra.gpi Out.txt diffeq4.data
	gnuplot $<

Out.txt: main.exe
	mono $<  > $@

main.exe: main.cs matrix.dll genlist.dll rungekutta.dll
	$(MKEXE)

matrix.dll: $(DIR)/matrix.cs $(DIR)/vec.cs
	$(MKLIB)
genlist.dll: $(DIR)/genlist.cs
	$(MKLIB)

rungekutta.dll: rungekutta.cs matrix.dll genlist.dll
	$(MKLIB)
	
clean: 
	rm -f *.dll *.exe *.svg *.data *.txt
