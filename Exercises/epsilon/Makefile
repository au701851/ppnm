out.txt : epsilon.exe              # out.txt depends on hello.exe
	mono epsilon.exe epsilon.exe > out.txt # run hello.exe, send output to out.txt

epsilon.exe : epsilon.cs             # hello.exe depends on hello.cs
	mcs epsilon.cs             # compile hello.cs into hello.exe

clean:                           # a phoney target, no dependencies
	rm -f out.txt epsilon.exe  # remove secondary files
