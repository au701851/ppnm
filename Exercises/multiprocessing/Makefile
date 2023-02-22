N = 1e8
TIME = time --portability --append --output $@
Out.txt: main.exe parallel.exe Makefile
	>$@
	$(TIME) mono $< -threads:1 -terms:$N >>$@
	$(TIME) mono $< -threads:2 -terms:$N >>$@
	$(TIME) mono $< -threads:3 -terms:$N >>$@
	$(TIME) mono $< -threads:4 -terms:$N >>$@
	$(TIME) mono parallel.exe -terms:$N >>$@

main.exe: main.cs
	mcs $<

parallel.exe: parallel.cs
	mcs $<
