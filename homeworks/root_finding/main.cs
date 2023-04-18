using static System.Console;
using System;

public class main{

public static int counter;
public static genlist<double> xlist;
public static genlist<vector> ylist;
public static string fileB1;

public static void Main(string[] args){
	foreach(var arg in args){
		var words = arg.Split(":");
		if(words[0] == "-fileB1")
			fileB1 = words[1];}
	WriteLine("======================================================");
	WriteLine("======================PART A==========================");
	partA();
	WriteLine("======================================================");
	partB(fileB1);
}
public static void partA(){
	WriteLine("For part a I had to create a rootfinder built on Newton's method.");
	WriteLine("I used the approximation for the derivative given in the BOOK eq. (7)");
	WriteLine("I begin by testing it on the simple function y = x + 1, expecting to find the root at x = -1. I pass x = -3 as my starting guess.");

	vector x = roots.newton((vector a) => (a.copy() + new vector(1.0)), new vector(-3.0));

	WriteLine($"Testing f(x) = x + 1 with initial guess -3 ... {x.approx(new vector(-1.0)) ? "PASSED" : "FAILED"}");	
	WriteLine($"Testing f(x,y) = (-x**2 + 4, y-x) with initial guess (-3, -3) ...");
	x = roots.newton((vector a) => (new vector(-a[0]*a[0]+4, a[1]-a[0])), new vector(-3.0, -3.0));
	x.print("Result:"); 
	WriteLine("... and with initial guess (3,3)");
	x = roots.newton((vector a) => (new vector(-a[0]*a[0]+4, a[1]-a[0])), new vector(3.0, 3.0));
	x.print("Result:"); 
	WriteLine("I see that my routine can find either of the zeroes, depending on where it starts looking.");

	WriteLine("\nLet's test the Rosenbrock valley function");
	WriteLine("To find extrema, I have to locate the zeroes of the gradient, which (I have calculated) is given by:");
	WriteLine("(df/dx, df/dy) = (2x-2 - 400(y-x**2)*x, 200(y-x**2)");
	WriteLine("Conferring with Wikipedia, I expect the zero to be at (1,1). I begin my search at the origin, and through my routine I obtain:");
	x = roots.newton((vector a) => (new vector(2*a[0]-2 - 400*(a[1]-a[0]*a[0])*a[0], 200*(a[1]-a[0]*a[0]))), new vector(0,0));
	x.print("(x,y) = ");
}

public static void partB(string fileB1){
	counter = 0;
	xlist = null;
	ylist = null;
	vector x = roots.newton(func, new vector(-0.5), 1e-4);
	xlist = new genlist<double>();
	ylist = new genlist<vector>();
	vector result = func(new vector(x[0]));

	var outfile = new System.IO.StreamWriter(fileB1);
	for(int i = 0; i<xlist.size; i++)
		outfile.WriteLine($"{xlist[i]}	{ylist[i][0]}");
	outfile.Close();
}

public static vector func(vector Evec){
	counter ++;
	double E = Evec[0];
	double rmin = 1.0/16, rmax = 10;
	Func<double, vector, vector> del = (double r, vector u) => (new vector(u[1], -2*(1.0/r - E)*u[0]));
	rungekutta myrk = new rungekutta(del);
	if(counter > 1000) throw new Exception("stoooop");

	vector a = myrk.driver(rmin, new vector(rmin-rmin*rmin, -2*(rmin-rmin*rmin)*(1.0/rmin+E)), rmax, 0.1, xlist, ylist);
	return new vector(a[0]);
	
}

}
