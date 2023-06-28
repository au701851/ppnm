using System;
using System.IO;
using static System.Console;
using static System.Math;
public class main{
public static void Main(){
	WriteLine("Part a");
	partA();
	partB();
	}

public static void partA(){

	//After writing part B, I need to pass initiated genlists to my routine
	genlist<double> xs = new genlist<double>(); genlist<vector> ys = new genlist<vector>();

	WriteLine("To test my function, I try to solve U'' = -U, by defining vec(y) = (y1, y2) where y1 = U and y2 = y1', so that I can solve y1' = y2 and y2' = -y1, as two coupled ODEs");

	//U'' = -U
	Func<double, vector, vector> f = (x, y) => (new vector(y[1], -y[0]));
	rungekutta myRK = new rungekutta(f);
	myRK.driver(0, new vector(0,1), 2*PI, 0.01, xs, ys);
	var outfile = new StreamWriter("diffeq1.data");
	for(int i = 0; i<xs.size; i++){
		if(ys[i].size != 2) throw new Exception("Something went wrong in constructing y");
		outfile.WriteLine($"{xs[i]}	{ys[i][0]}	{ys[i][1]}");}
	outfile.Close();
	WriteLine("I calculate a solution to U'' = -U with initial conditions y0 = (0, 1), expecting a sine-function for the answer.");


	//U'' = U
	xs = new genlist<double>(); ys = new genlist<vector>();
	f = (x,y) => (new vector(y[1], y[0]));
	myRK = new rungekutta(f);
	myRK.driver(0, new vector(1,-1), 2*PI, 0.01, xs, ys);
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
	xs = new genlist<double>(); ys = new genlist<vector>();
	
	myRK = new rungekutta(f);
	myRK.driver(0, new vector(PI-0.1, 0.0), 10, 0.01, xs, ys);
	outfile = new StreamWriter("diffeq3.data");
	for(int i = 0; i<xs.size; i++){
		if(ys[i].size != 2) throw new Exception("Something went wrong in constructing y");
		outfile.WriteLine($"{xs[i]}	{ys[i][0]}	{ys[i][1]}");}
	outfile.Close();
	WriteLine("The plot is recreated with values determined by my runge-kutta function in Pendulum.svg");	
}

public static void partB(){
	WriteLine("I have implemented the changes in my routine, so I can evaluate only the end vector, thus saving time.");
	WriteLine("I have also changed the step acceptence condition to compare the toleranse and the error componentwise rather than by norm");
	WriteLine("Now I have come to the part, where I must recreate the plot of the Lotka-Volterra system.");
	WriteLine("I must solve the system of coupled diff equations:");
	WriteLine("dx/dt = ax - bxy");
	WriteLine("dy/dt = dxy - cy");
	WriteLine("with boundary conditions x(0) = 10, y(0) = 5.");
	WriteLine("I write the coupled diff eq as a Function, and solve it with my routine. The result is plotted in LotkaVolterra.svg");

	genlist<double> xs = new genlist<double>(); genlist<vector> ys = new genlist<vector>();

	double a = 1.5; double b = 1; double c = 3; double d = 1;
	Func<double, vector, vector> f = (x, y) => (new vector(a*y[0]-b*y[0]*y[1], -c*y[1] + d*y[1]*y[0]));
	rungekutta myRK = new rungekutta(f);
	vector y0 = new vector(10.0, 5.0); //boundary cond
	y0 = myRK.driver(0, y0, 15, 0.01, xs, ys);

	var outfile = new StreamWriter("diffeq4.data");
	for(int i = 0; i<xs.size; i++){
		if(ys[i].size != 2) throw new Exception("Something went wrong in constructing y");
		outfile.WriteLine($"{xs[i]}	{ys[i][0]}	{ys[i][1]}");}
	outfile.Close();

}
}
