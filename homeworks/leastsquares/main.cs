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
for(int i = 0; i<y.size; i++)
	logy[i] = Log(y[i]);

//Formatting output for plotting
var outfile = new System.IO.StreamWriter("rad_decay_formatted.txt");
for(int i = 0; i<x.size; i++)
	outfile.WriteLine($"{x[i]}	{y[i]}	{Log(y[i])}	{dy[i]*y[i]}");
outfile.Close();


//Since I am fitting to straight line...
var fs = new Func<double, double>[] {z => 1.0, z => z};

//performing the fit
(vector c, matrix Cov) = lineq.lsfit(fs, x, logy, dy);
Cov.print();
WriteLine("The best fit vaues and their uncertainties are is:");
WriteLine($"a: {c[0]} +- {Sqrt(Cov[0,0])}");
WriteLine($"l: {c[1]} +- {Sqrt(Cov[1,1])}");

WriteLine($"... which yields a half-life of {-Log(2)/c[1]} +- {Log(2)/Pow(c[1], 2)*Sqrt(Cov[1,1])} -- today we know the value to be 3.5, which does not lie within the estimated uncertainty.");
WriteLine($"Here, the error in the half life has been determined by error propation: d(1/c) = 1/c^2 dc");

var outfile2 = new System.IO.StreamWriter("best_fit.txt");
outfile2.WriteLine($"{c[0]}	{c[1]}");
outfile2.Close();
}}
