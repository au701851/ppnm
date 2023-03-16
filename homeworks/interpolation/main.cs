using static System.Math;
using static System.Console;
using System.IO;
public class main{
public static void Main(){
	partA();
	WriteLine("\n=================================");
	partB();
	WriteLine("\n=================================");
	}

public static void partA(){
	WriteLine("Part a");
	//Creating a series of tabulated values.
	double lim = 3*PI/2; double res = 4.0;
	vector x = new vector((int) (2*lim*res));
	vector y = new vector((int) (2*lim*res));  
	for(int i = 0; i < x.size; i++){
		x[i] = -lim + ((double) i)/res;
		y[i] = Sin(-lim + ((double) i)/res);}
		
	//Creating a spline object to calculate the spline	
	spline myspline = new spline(x, y, "l");
	WriteLine("safe");
	double[] z = new double[(int) (2*lim*res)-1];
	double[] intz = new double[(int) (2*lim*res)-1];
	for(int i = 0; i<z.Length; i++){
		double point = x[i] + 1.0/res/2;
		z[i] = myspline.evaluate(point);
		intz[i] = myspline.integral(point);
	}
	WriteLine("safe");
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
	vector x = new vector(5);
	vector y1 = new vector(5);
	vector y2 = new vector(5);
	vector y3 = new vector(5);
	for(int i = 1; i <= 5; i++){
		x[i-1] = i; y1[i-1] = 1; y2[i-1] = i; y3[i-1]=Pow(i,2);}
	WriteLine("I test my quad spline program, by writing out manually calculated results for each spline, along with my programs result:");
	WriteLine("For y = x^2, I expect the spline to satisfy s(x)=a(x-xi)^2 + b(x-xi) + c = y(x) = x^2, which is satisfied if a=1, b=2xi and c = xi^2. Lets check the results:");
	WriteLine("as			bs			cs				");
	spline myspline = new spline(x, y3, "q");
	for(int i = 0; i<4; i++){
	WriteLine($"{myspline.a[i]} (should be 1)		{myspline.b[i]} (should be {2*x[i]})		{myspline.c[i]} (should be {x[i]*x[i]})");
	}
	myspline = new spline(x, y2, "q");
	WriteLine("\nFor y=x, one would expect the spline to be s(x)=a(x-xi)^2+b(x-xi)+c = y(x) = x, which is satisfied if a = 0, b = 1 and c = xi.");
	for(int i = 0; i<4; i++)
		WriteLine($"{myspline.a[i]} (should be 0)		{myspline.b[i]} (should be 1)		{myspline.c[i]} (should be {x[i]})");

	myspline = new spline(x, y1, "q");
	WriteLine("\nFor y=1, one would expect the spline to be s(x)=a(x-xi)^2+b(x-xi)+c = y(x) = 1, which is satisfied if a = 0, b = 0 and c = 1.");
	for(int i = 0; i<4; i++)
		WriteLine($"{myspline.a[i]} (should be 0)		{myspline.b[i]} (should be 0)		{myspline.c[i]} (should be 1)");
	
	
}}
	
