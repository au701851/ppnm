using System;
using static System.Math;
using static System.Console;

public class cmdline{
	//I select int as my return type, so I can indicate possible errors
	public static int Main(string[] args){
		//Strings can be provided when the program is run
		if(args.Length == 0){
			Error.Write("There was no argument\n");
			return 1;
			}
		foreach(var arg in args){
			double x = double.Parse(arg);
			WriteLine($"{x} {Sin(x)} {Cos(x)}");
			}
		return 0; //If everything worked out
	}
}
