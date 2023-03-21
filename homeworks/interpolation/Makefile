DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
DIR = $(HOME)/repos/ppnm/math_resources

all: Out.txt Linear_interpolation.svg Q_interpolation.svg

Linear_interpolation.svg: graphA.gpi Out.txt tab_values_sine.data spline_values_sine.data
	gnuplot $<
Q_interpolation.svg: graphB.gpi Out.txt tab_values_sine.data qspline_values_sine.data
	gnuplot $<

Out.txt: main.exe
	mono $<  > $@

main.exe: main.cs qspline.dll matrix.dll
	$(MKEXE)

matrix.dll: $(DIR)/matrix.cs $(DIR)/vec.cs
	$(MKLIB)

qspline.dll: qspline.cs matrix.dll
	$(MKLIB)
	
clean: 
	rm -f *.dll *.exe *.svg *.data *.txt
