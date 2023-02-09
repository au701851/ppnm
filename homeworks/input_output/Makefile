Out.txt: stdin.exe cmdline.exe fileio.exe inputFileio.txt
	echo 1 2 3 4 5 > input.txt
	echo "From stdin:\n" > Out.txt
	mono stdin.exe < input.txt >> Out.txt
	echo "\nFrom cmdline:\n" >> Out.txt
	mono cmdline.exe 1 0.5 0.25 0.5 0.125 >> Out.txt
	echo "\nFrom fileio:\n" >> Out.txt
	mono fileio.exe -input:inputFileio.txt -output:Out.txt

stdin.exe: stdin.cs
	mcs stdin.cs -target:exe -out:stdin.exe

cmdline.exe: cmdline.cs
	mcs cmdline.cs -target:exe -out:cmdline.exe

fileio.exe: fileio.cs
	mcs $< -target:exe -out:fileio.exe
