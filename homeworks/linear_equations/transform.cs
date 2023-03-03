//Write a function that reads the output from partc.txt and writes a nice table
using System;
using static System.Console;
public class reader{
public static double N, user_time;
public static void Main(){

char[] split_delimiters = {' ','\t','\n'};
var split_options = StringSplitOptions.RemoveEmptyEntries;

for(string line = ReadLine(); line != null; line = ReadLine()){
	var words = line.Split(split_delimiters, split_options);
	if(words[0] == "N"){
		N = double.Parse(words[1]);}
	if(words[0] == "user") {user_time = double.Parse(words[1]); WriteLine($"{N} {user_time}");}
}
}
}
