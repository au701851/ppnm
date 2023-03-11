DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(DLLS) $^

all: Out.txt Plot.svg

Plot.svg: plot.gpi rad_decay_formatted.txt best_fit.txt
	gnuplot $<
	
Out.txt: main.exe rad_decay.txt
	mono $< <$(filter %.txt, $^)  > $@

main.exe: main.cs linalg.dll 
	$(MKEXE) 

linalg.dll: vec.cs matrix.cs lineq.cs least_squares.cs
	$(MKLIB) 
	
clean: 
	rm -f *.dll *.exe *.svg
