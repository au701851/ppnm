using System;
using System.IO;
using static System.Console;
using static System.Math;
public class main{
public static void Main(){
	WriteLine("Part a");
	partA();
	}

public static void partA(){
	WriteLine("To test my function, I try to solve U'' = -U, by defining vec(y) = (y1, y2) where y1 = U and y2 = y1', so that I can solve y1' = y2 and y2' = -y1, as two coupled ODEs");

	//U'' = -U
	Func<double, vector, vector> f = (x, y) => (new vector(y[1], -y[0]));
	rungekutta myRK = new rungekutta(f);
	(genlist<double> xs, genlist<vector> ys) = myRK.driver(0, new vector(0,1), 2*PI);
	var outfile = new StreamWriter("diffeq1.data");
	for(int i = 0; i<xs.size; i++){
		if(ys[i].size != 2) throw new Exception("Something went wrong in constructing y");
		outfile.WriteLine($"{xs[i]}	{ys[i][0]}	{ys[i][1]}");}
	outfile.Close();
	WriteLine("I calculate a solution to U'' = -U with initial conditions y0 = (0, 1), expecting a sine-function for the answer.");


	//U'' = U
	f = (x,y) => (new vector(y[1], y[0]));
	myRK = new rungekutta(f);
	(xs, ys) = myRK.driver(0, new vector(1,-1), 2*PI);
	outfile = new StreamWriter("diffeq2.data");
	for(int i = 0; i<xs.size; i++){
		if(ys[i].size != 2) throw new Exception("Something went wrong in constructing y");
		outfile.WriteLine($"{xs[i]}	{ys[i][0]}	{ys[i][1]}");}
	outfile.Close();
	WriteLine("\nI similarly attempt to calculate U'' = U, with initial conditions y0 = (1, -1), expecting the solution f(x) = exp(-x)");


	WriteLine("\nThe solutions are plotted in GraphA.svg");
		

	//Pendulum with friction
	WriteLine("\nI also attempt to recreate the result of the pendulum with friction from the scipy manual");
	WriteLine("Here vec(y) = (theta, omega), where theta'=omega and omega' = -b*omega - c*sin(theta) with b =0.25 and c = 5.0 in our problem.");
	double b = 0.25; double c = 5.0;
	f = (x,y) => (new vector(y[1], -b*y[1] - c*Sin(y[0])));
	myRK = new rungekutta(f);
	(xs, ys) = myRK.driver(0, new vector(PI-0.1, 0.0), 10);
	outfile = new StreamWriter("diffeq3.data");
	for(int i = 0; i<xs.size; i++){
		if(ys[i].size != 2) throw new Exception("Something went wrong in constructing y");
		outfile.WriteLine($"{xs[i]}	{ys[i][0]}	{ys[i][1]}");}
	outfile.Close();
	WriteLine("The plot is recreated with values determined by my runge-kutta function in Pendulum.svg");
	
	
}
}
