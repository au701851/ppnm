DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
DIR = ../../math_resources

all: Out.txt Linear_interpolation.svg Q_interpolation.svg C_interpolation.svg

Linear_interpolation.svg: graphA.gpi Out.txt tab_values_sine.data spline_values_sine.data
	gnuplot $<
Q_interpolation.svg: graphB.gpi Out.txt tab_values_sine.data qspline_values_sine.data
	gnuplot $<
C_interpolation.svg: graphC.gpi cspline.data
	gnuplot $< >$@

Out.txt tab_values_sine.data spline_values_sine.data qspline_values_sine.data cspline.data: main.exe
	mono $<  > $@

main.exe: main.cs qspline.dll matrix.dll
	$(MKEXE)

matrix.dll: $(DIR)/matrix.cs $(DIR)/vec.cs
	$(MKLIB)

qspline.dll: qspline.cs matrix.dll
	$(MKLIB)
	
clean: 
	rm -f *.dll *.exe *.svg *.data *.txt
