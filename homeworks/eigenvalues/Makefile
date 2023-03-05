TIME = time --portability --append --output $@
DLLS = $(addprefix -reference:, $(filter %.dll, $^))

Convergence.svg: convergence_graph.gpi Out.txt
	gnuplot $<

Out.txt: main.exe
	mono $< >$@

main.exe: main.cs matrix.dll
	mcs $< -target:exe $(DLLS) -out:$@

matrix.dll: vec.cs matrix.cs jacobi.cs hydrogen_atom.cs
	mcs -target:library -out:$@ $^

clean: 
	rm -f *.dll *.txt *.exe *.svg
