TIME = time --portability --append --output $@

Out.txt: main.exe Graph.svg
	mono $< > $@
	echo My graph of the fit from part c. can be seen in "Graph.svg" >>$@

Graph.svg: graph.gpi table.txt
	gnuplot $<

timed_QR.txt: ocounting.exe
	echo N 100 >$@
	$(TIME) mono $< -N:100 >>$@
	echo N 200 >>$@
	$(TIME) mono $< -N:200 >>$@
	echo N 300 >>$@
	$(TIME) mono $< -N:300 >>$@
	echo N 400 >>$@
	$(TIME) mono $< -N:400 >>$@
	echo N 500 >>$@
	$(TIME) mono $< -N:500 >>$@
	echo N 600 >>$@
	$(TIME) mono $< -N:600 >>$@

table.txt: timed_QR.txt transform.exe
	mono $(filter %.exe, $^) < $^ > $@

transform.exe: transform.cs
	mcs $< -target:exe -out:$@

main.exe: main.cs matrix.dll lineq.dll
	mcs $< -target:exe $(addprefix -reference:, $(filter %.dll, $^)) -out:$@

ocounting.exe: ocounting.cs matrix.dll lineq.dll
	mcs $< -target:exe $(addprefix -reference:, $(filter %.dll, $^)) -out:$@

matrix.dll: vec.cs matrix.cs
	mcs -target:library -out:$@ $^
lineq.dll: lineq.cs matrix.dll
	mcs -target:library $(addprefix -reference:, $(filter %.dll, $^)) -out:$@ $<

clean: 
	rm -f *.dll *.txt *.exe *.svg
