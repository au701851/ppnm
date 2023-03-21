TIME = time --portability --append --output $@
DLLS = $(addprefix -reference:, $(filter %.dll, $^))
RMAX = 6.0
DR = 0.3
all: parta partb Convergence.svg Eigenvectors.svg

Convergence.svg: convergence_graph.gpi partb
	gnuplot $<

Eigenvectors.svg: eigenvectors.gpi partb
	gnuplot $<

parta: main.exe
	echo A > Out.txt
	mono $< >> Out.txt

partb: hydrogen_atom.exe varying_dr.data varying_rmax.data
	echo B >> Out.txt

varying_dr.data: hydrogen_atom.exe
	mono $< -rmax:$(RMAX) -dr:0.1 > $@  &
	mono $< -rmax:$(RMAX) -dr:0.2 >> $@  &
	mono $< -rmax:$(RMAX) -dr:0.3 >> $@  &
	mono $< -rmax:$(RMAX) -dr:0.4 >> $@  &
	mono $< -rmax:$(RMAX) -dr:0.5 >> $@  &
	mono $< -rmax:$(RMAX) -dr:0.6 >> $@  &
	mono $< -rmax:$(RMAX) -dr:0.7 >> $@  &
	mono $< -rmax:$(RMAX) -dr:0.8 >> $@  &
	mono $< -rmax:$(RMAX) -dr:0.9 >> $@  &
varying_rmax.data: hydrogen_atom.exe
	mono $< -dr:$(DR) -rmax:1 > $@ &
	mono $< -dr:$(DR) -rmax:2 >> $@ &
	mono $< -dr:$(DR) -rmax:3 >> $@ &
	mono $< -dr:$(DR) -rmax:4 >> $@ &
	mono $< -dr:$(DR) -rmax:5 >> $@ &
	mono $< -dr:$(DR) -rmax:6 >> $@ &
	mono $< -dr:$(DR) -rmax:7 >> $@ &
	mono $< -dr:$(DR) -rmax:8 >> $@ &
	mono $< -dr:$(DR) -rmax:9 >> $@ &

main.exe: main.cs matrix.dll
	mcs $< -target:exe $(DLLS) -out:$@

matrix.dll: vec.cs matrix.cs jacobi.cs hydrogen_atom.cs
	mcs -target:library -out:$@ $^

hydrogen_atom.exe: hydrogen_atom.cs matrix.dll
	mcs $(DLLS) -out:$@ $<

clean: 
	rm -f *.dll *.txt *.exe *.svg *.data
