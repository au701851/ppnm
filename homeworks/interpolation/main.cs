using static System.Math;
using static System.Console;
using System.IO;
using System;

public class main{
public static vector x, y;
public static double lim, res;
public static Random rand = new Random();
public static void Main(){
	//Creating a series of tabulated values.
	defineXY();
	
	partA();
	WriteLine("\n=================================");
	partB();
	WriteLine("\n=================================");
	partC();
	WriteLine("\n=================================");
	}

public static void defineXY(string mode = "sine"){
	if(mode == "sine"){
	//Creating a series of tabulated values.
	lim = 3*PI/2; res = 4.0;
	x = new vector((int) (2*lim*res));
	y = new vector((int) (2*lim*res));  
	for(int i = 0; i < x.size; i++){
		x[i] = -lim + ((double) i)/res;
		y[i] = Sin(-lim + ((double) i)/res);}
	}
	if(mode == "rand"){
	x = new vector((int) 11);
	y = new vector((int) 11);
	for(int i = 0; i<x.size; i++){
		x[i] = i;
		y[i] = rand.NextDouble();
	}
	}
	}

public static void partA(){
	WriteLine("Part a");
		
	//Creating a spline object to calculate the spline	
	spline myspline = new spline(x, y, "l");
	double[] z = new double[(int) (2*lim*res)-1];
	double[] intz = new double[(int) (2*lim*res)-1];
	for(int i = 0; i<z.Length; i++){
		double point = x[i] + 1.0/res/2;
		z[i] = myspline.evaluate(point);
		intz[i] = myspline.integral(point);
	}
	var outfile = new StreamWriter("tab_values_sine.data");
	var outfile2 = new StreamWriter("spline_values_sine.data");
	for(int i = 0; i<z.Length; i++){
		outfile.WriteLine($"{x[i]}	{y[i]}");
		outfile2.WriteLine($"{x[i]+1.0/res/2}	{z[i]}	{intz[i]}");
	}
	outfile.Close(); outfile2.Close();

	WriteLine("I have collected tabulated values of sin(x) in a file, and sent these values to my linterp object. I have the collected interpolated values, and integrals.");
	WriteLine("Since I expect my integral of the sine function to be cos(x)-cos(x[0]), where x[0] is the first number in my array, I have done my calculations on an inteval [-3/4pi, 3/4pi], so my plots are prettier... :)");
	WriteLine("The graph showing my work on part a is Linear_interpolation.svg"); 
}

public static void partB(){
	WriteLine("Part b");
	vector x1 = new vector(5);
	vector y1 = new vector(5);
	vector y2 = new vector(5);
	vector y3 = new vector(5);
	for(int i = 1; i <= 5; i++){
		x1[i-1] = i; y1[i-1] = 1; y2[i-1] = i; y3[i-1]=Pow(i,2);}
	WriteLine("I test my quad spline program, by writing out manually calculated results for each spline, along with my programs result:");
	WriteLine("For y = x^2, I expect the spline to satisfy s(x)=a(x-xi)^2 + b(x-xi) + c = y(x) = x^2, which is satisfied if a=1, b=2xi and c = xi^2. Lets check the results:");
	WriteLine("as			bs			cs				");
	spline myspline = new spline(x1, y3, "q");
	for(int i = 0; i<4; i++){
	WriteLine($"{myspline.a[i]} (should be 1)		{myspline.b[i]} (should be {2*x1[i]})		{myspline.c[i]} (should be {x1[i]*x1[i]})");
	}
	myspline = new spline(x1, y2, "q");
	WriteLine("\nFor y=x, one would expect the spline to be s(x)=a(x-xi)^2+b(x-xi)+c = y(x) = x, which is satisfied if a = 0, b = 1 and c = xi.");
	for(int i = 0; i<4; i++)
		WriteLine($"{myspline.a[i]} (should be 0)		{myspline.b[i]} (should be 1)		{myspline.c[i]} (should be {x1[i]})");

	myspline = new spline(x1, y1, "q");
	WriteLine("\nFor y=1, one would expect the spline to be s(x)=a(x-xi)^2+b(x-xi)+c = y(x) = 1, which is satisfied if a = 0, b = 0 and c = 1.");
	for(int i = 0; i<4; i++)
		WriteLine($"{myspline.a[i]} (should be 0)		{myspline.b[i]} (should be 0)		{myspline.c[i]} (should be 1)");


	///////////////////////////////////////////////////////////////////
	
	WriteLine("\nFurthermore, to test my quadratic spline, I have made a plot similar to the one from part a, using the tabulated values of sine");

	//Creating a spline object to calculate the spline	
	myspline = new spline(x, y, "q");
	double[] z = new double[(int) (2*lim*res)-1];
	double[] intz = new double[(int) (2*lim*res)-1];
	double[] devz = new double[(int) (2*lim*res)-1];
	for(int i = 0; i<z.Length; i++){
		double point = x[i] + 1.0/res/2;
		z[i] = myspline.evaluate(point);
		intz[i] = myspline.integral(point);
		devz[i] = myspline.derivative(point);
	}
	var outfile = new StreamWriter("qspline_values_sine.data");
	for(int i = 0; i < z.Length; i++)
		outfile.WriteLine($"{x[i]+1.0/res/2}	{z[i]}	{intz[i]}	{devz[i]}");
	outfile.Close();

	WriteLine("These calculated values are saved in qspline_values_sine.data and plotted in Q_interpolation.svg");
	
}


public static void partC(){
	WriteLine("For part C, I had to implement a tridiagonal matrix equation solver.");

	WriteLine("Testing spline.tridiag ...");
	vector a = new vector("0,-1,-1,-1, -1");
	vector b = new vector("2,2,2,2,2");
	vector c  = new vector("-1,-1,-1,-1,0");
	vector d = new vector("1, 1, 1, 1, 1");
	vector sol = spline.tridiag(a, b, c, d);

	WriteLine($"{sol.approx(new vector("2.5, 4, 4.5, 4, 2.5")) ? "... PASSED" : "...FAILED"} \n");


	WriteLine("Testing with random 5x5 tridiagonal matrix...");
	matrix M = new matrix(5,5);
	for(int i = 0; i<5; i++){
		if(i!=0) {a[i]=rand.Next(); M[i, i-1] = a[i];}
		if(i!=4) {c[i]=rand.Next(); M[i, i+1] = c[i];}
		b[i] = rand.Next(); M[i,i] = b[i];
		d[i] = rand.Next();
	}

	sol = spline.tridiag(a,b,c,d.copy());
	WriteLine($"{(M*sol).approx(d) ? "...PASSED" : "... FAILED"} \n");

	defineXY("rand");
	WriteLine("Next I had to implement a cubic spline, solving the values of b using my tridiagonalised routine.");
	spline myspline = new spline(x.copy(),y.copy(), "c");

	WriteLine("I compared my cubic spline to the built-in cubic spline from gnuplot, by plotting som random points, and using my routine to connect them. The graoh showing my work is C_interpolation.svg");
	
	var outfile = new StreamWriter("cspline.data");
	outfile.WriteLine("###x	y	x	f(x)	f'(x)	F(x)");
	for(int i = 0; i < 100; i++){
		double n = (i+0.5)/10;
		if(i<x.size-1)
			outfile.WriteLine($"{n}	{myspline.evaluate(n)}	{myspline.derivative(n)}	{myspline.integral(n)}	{x[i]}	{y[i]}");
		else
			outfile.WriteLine($"{n}	{myspline.evaluate(n)}	{myspline.derivative(n)}	{myspline.integral(n)}");}
	outfile.Close();
}
}
