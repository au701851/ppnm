using System;
using static System.Console;
using static System.Math;
public class main{
public static vector x, y, logy, dy;
public static void Main(){
//parsing the input as vectors
for(string s = ReadLine(); s != null; s = ReadLine()){
var words = s.Split(":");
if(words[0] == "-x"){
	var numbers = words[1].Split(",");
	x = new vector(numbers.Length);
	for(int i = 0; i<numbers.Length; i++)
		 x[i] = double.Parse(numbers[i]);
}
if(words[0] == "-y"){
	var numbers = words[1].Split(",");
	y = new vector(numbers.Length);
	for(int i = 0; i<numbers.Length; i++)
		y[i] = double.Parse(numbers[i]); //I take the logarithm so I can fit to a straight line
}
if(words[0] == "-dy"){
	var numbers = words[1].Split(",");
	dy = new vector(numbers.Length);
	for(int i = 0; i<numbers.Length; i++){
		double k = double.Parse(numbers[i]);
		dy[i] = k/y[i]; //error propagating
}}}
logy = new vector(y.size);
for(int i = 0; i<y.size; i++){
	logy[i] = Log(y[i]);
}
//Since I am fitting to straight line...
var fs = new Func<double, double>[] {z => 1.0, z => z};

vector c = lineq.lsfit(fs, x, logy, dy);

var outfile = new System.IO.StreamWriter("rad_decay_formatted.txt");
for(int i = 0; i<x.size; i++){
	outfile.WriteLine($"{x[i]}	{y[i]}	{Log(y[i])}	{dy[i]*y[i]}");
}
outfile.Close();

WriteLine("The vector with the best fit vaues of a and lambda is:");
c.print();

WriteLine($"... which yields a half-life of {-Log(2)/c[1]} -- today we know the value to be 3.5");

var outfile2 = new System.IO.StreamWriter("best_fit.txt");
outfile2.WriteLine($"{c[0]}	{c[1]}");
outfile2.Close();
}}
