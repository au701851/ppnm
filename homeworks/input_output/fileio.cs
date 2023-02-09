using static System.Console;
using static System.Math;
using System;
public class fileio{
	public static int Main(string[] args){
		string infile = null, outfile = null;
		if(args.Length == 0){
			Error.Write("No arguments given\n");
			return 1;
		}
		foreach(var arg in args){
			var words = arg.Split(':'); 
			//The first part of arg will be -input: or -output:. 
			//I use this to save the args properly
			if(words[0] == "-input")
				{infile = words[1];}
			else if(words[0] == "-output")
				{outfile = words[1];}
			else 
				{Error.WriteLine("Wrong argument"); 
				return 1;}
			}
				
		//I define my own input file stream
		var instream = new System.IO.StreamReader(infile);
		var outstream = new System.IO.StreamWriter(outfile, append:true);

		for(string line = instream.ReadLine(); line != null; line = instream.ReadLine()){
		//Loops over all entries...
			double x = double.Parse(line); //... and interprets them as doubles
			//They are printed in the outstream
			outstream.WriteLine($"{x} {Sin(x)} {Cos(x)}");
		}
		//I remember to close my files!
		instream.Close();
		outstream.Close();	
		return 0; //Everything was in order, so I return 0
	}







}
