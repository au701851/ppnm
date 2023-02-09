using System;
using static System.Math;
using static System.Console;
public class stdin{
	public static void Main(){
		char[] delimiters = {' ', '\t', '\n'}; //Define possible delimiters
		var options = StringSplitOptions.RemoveEmptyEntries;
		for(string line = ReadLine(); line != null; line = ReadLine()){
			//Create a for-loop over all line entries in the standard input
			var words = line.Split(delimiters, options);
			//Splits the strings at the desired delimiters
			foreach(var word in words){
				double x = double.Parse(word); //Interprets the word as a double
				WriteLine($"{x} {Sin(x)} {Cos(x)}");
			}
		}

	}
}
