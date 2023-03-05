Erf.svg: erf.gpi tab_error.txt erf_fun.txt
	gnuplot $<
Gamma.svg: gamma.gpi gamma_fun.txt
	gnuplot $<
Lngamma.svg: lngamma.gpi lngamma_fun.txt
	gnuplot $<

erf_fun.txt gamma_fun.txt factorials.txt lngamma_fun.txt: main.exe #Output is redirected in the file
	mono $< > $@


main.exe: main.cs functions.dll
	mcs $< -target:exe -out:$@ \
			$(filter %.cs, $^) $(addprefix -reference:,$(filter %.dll, $^))

#Math functions to calculate approximations of gamma and erf
functions.dll: erf.cs gamma.cs
	mcs $^ -target:library -out:$@

clean: 
	rm -f Erf.svg erf_tab.txt main.exe fileio.exe genlist.dll functions.dll erf_fun.txt 
