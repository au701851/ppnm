using System;
using System.Collections.Generic;
using static System.Math;
using static System.Console;

public class main{

public static string fileB1, fileC1; 

public static void Main(string[] args){
	fileB1 = "default.data"; fileC1 = "default.data";
	foreach(var arg in args){
		var words = arg.Split(":");
		if(words[0] == "-fileB1")
			fileB1 = words[1];
		if(words[0] == "-fileC1")
			fileC1 = words[1];
	}
	WriteLine("----------------------------------------------------");
	WriteLine("--------------------Part A--------------------------");
	partA();
	WriteLine("----------------------------------------------------");
	WriteLine("--------------------Part B--------------------------");
	partB();
	//comparison(fileB1);
	WriteLine("----------------------------------------------------");
	WriteLine("--------------------Part C--------------------------");
	partC();
	//recordingpath(fileC1);
	WriteLine("----------------------------------------------------");
	

}
public static void partA(){
	WriteLine("For part a, I had to implement a plain montecarlo integrator. I test it on some integrals:");

	int N = 1000000;
	WriteLine("\nFirst the integral of f(x,y) = x on x going from 0 to 3 and b going from 0 to 3. This should half the volume of V, that is 3*3*3/2 = 13.5:");
	var result = montecarlo.plainmc((a) => (a[0]), new vector(0,0), new vector(3,3), N);
	WriteLine($"Obtained result:{result.Item1}		Estimated error: {result.Item2}		with N = {N}.");
	WriteLine($"I ran the computation with different values of N, and found that the error was generally underestimated, until I reached very large N.");

	WriteLine("\nA simple spherical integral, the function f(r, theta, phi) = 16z = 16r*cos(theta) in the upper half sphere with radius one.");
	WriteLine("That means I must evaluate along (0,1) for r, (0, pi/2) for theta and (0, 2pi) for phi, and remember to multiply the integrand by r^2sin(theta), for spherical integration.");
	result = montecarlo.plainmc((a) => (16*a[0]*Cos(a[1])*a[0]*a[0]*Sin(a[1])), new vector(0,0,0), new vector(1, PI/2.0, 2.0*PI), N);
	WriteLine($"Obtained result:{result.Item1}		Estimated error: {result.Item2}		with N = {N}.");
	WriteLine($"The expected result was 4*PI = {4*PI}");

	WriteLine("\nLet's attempt the integral given in the exercise");
	WriteLine("Setting f(x, y, z) = 1/PI^3 [1 - cos(x)cos(y)cos(z)]^-1, and choosing a and b to mark the integration limits, a = (0,0,0), b = (PI, PI, PI)...");
	result = montecarlo.plainmc((a) => 1.0/PI/PI/PI*Pow(1-Cos(a[0])*Cos(a[1])*Cos(a[2]), -1), new vector(0,0,0), new vector(PI, PI, PI), N);
	WriteLine($"Obtained result:{result.Item1}		Estimated error: {result.Item2}		with N = {N}.");
	WriteLine($"The expected result was 1.3932039296856768591842462603255, 
		giving a relative error of {(result.Item1-1.3932039296856768591842462603255)/1.3932039296856768591842462603255}");
	
}

public static void partB(){
	int n = 253;
	WriteLine("For part b, I first had to implement a van der corput number generator.\nSuch one sort of 'mirrors' the number around the decimal point, so for instance, I expect corput(253)=0.352 for base 10. \nLets check:");
	double c = montecarlo.corput(n, 10);
	WriteLine($"Corput({n}) = {c}");

	WriteLine("\nI used my corput number calculator to implement a Halton sequence to choose my quasirandom points. I estimate the error by using a Halton sequance relying on other co prime numbers than my main sequence, and calculate the difference in their results.");
	WriteLine("I test it on the first function from part A:");
	var result = montecarlo.quasimc((a) => (a[0]), new vector(0,0), new vector(3,3), 1000000);
	WriteLine($"quasiresult = {result.Item1} with error {result.Item2}");

	WriteLine("\nIt appears that the convergence is better here. I compare the convergence of the two methods by performing the 4PI integral from the part a multiple times with different Ns and recording the relative error");
	WriteLine($"I plotted my findings in {fileB1.Replace(".data", ".svg")}");
}

public static void comparison(string file){
	var outfile = new System.IO.StreamWriter(file);
	outfile.WriteLine("N	plain_result	plain_err	quasi_result	quasi_err");
	for(int i = 10; i<100000000; i*=10){
		var resultplain = montecarlo.plainmc((a) => (16*a[0]*Cos(a[1])*a[0]*a[0]*Sin(a[1])), new vector(0,0,0), new vector(1, PI/2.0, 2.0*PI), i);
		var resultquasi = montecarlo.quasimc((a) => (16*a[0]*Cos(a[1])*a[0]*a[0]*Sin(a[1])), new vector(0,0,0), new vector(1, PI/2.0, 2.0*PI), i);
		outfile.WriteLine($"{i}	{resultplain.Item1}	{resultplain.Item2}	{resultquasi.Item1}	{resultquasi.Item2}");
	}
	outfile.Close();

}


public static void partC(){
	WriteLine("Testing my rss-montecarlo integrator on the good ol' triangle integral from the other parts.");
	var result = montecarlo.rssmc((a) => (a[0]), new vector(0,0), new vector(3,3), 10000000);
	WriteLine($"Rss result = {result.Item1} with error {result.Item2}, should be 13.5 with {montecarlo.COUNT} iterations");

	WriteLine("\nI have built in the option to record the 'path' of integration, that is the random points. I show these in some file");
	
}

public static void recordingpath(string file){
	List<vector> points = new List<vector>();
	Func<vector, double> f = delegate(vector a)
				{if(a.norm() < 0.8) return 1; 
				else return 0;};
				
	var result = montecarlo.rssmc(f, new vector(0,0), new vector(1,1), 100000, points);
	var outfile = new System.IO.StreamWriter(file);
	outfile.WriteLine("x	y");
	foreach(vector point in points)
		outfile.WriteLine($"{point[0]}	{point[1]}");
	outfile.Close();
	WriteLine($"Final integral = {result.Item1} +- {result.Item2}");
}

}
