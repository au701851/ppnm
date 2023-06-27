using System;
using static System.Console;
using static System.Math;

public class main{
	public static Random rand; 
	public static Func<double, double> f, delf, delf2, F; 
	public static vector x, y, dely;
	public static string file1;

public static void Main(string[] args){
	foreach(var arg in args){
		var words = arg.Split(":");
		if(words[0] == "-file1") file1 = words[1];
	}
	WriteLine("==================================================");
	WriteLine("I test my cubic sub spline on tabulated values from the function f(x) = exp(-x)*cos(5x-1)");
	f = (double X) => Exp(-X)*Cos(5*X-1);
	delf = (double X) => -Exp(-X)*(Cos(5*X-1) + 5*Sin(5*X-1));
	delf2 = (double X) => - 2*Exp(-X)*(12*Cos(5*X-1)-5*Sin(5*X-1));
	F = (double X) => -1.0/26*Exp(-X)*(Cos(1-5*X)+ 5*Sin(1-5*X));

	x = new vector(20);
	y = new vector(20);
	dely = new vector(20);
	for(int i = 0; i<x.size; i++){
		x[i] = -1 + 2.0*i/x.size;
		y[i] = f(x[i]);
		dely[i] = delf(x[i]);
	}
	
	subspline ss = new subspline(x, y, dely);
	test_evaluate(ss);
	test_derivative(ss);
	test_integrate(ss);

	WriteLine("\n==================================================");
	WriteLine("I build a quartic spline, which shold give me a continuous second derivative.");

	subspline qs = new subspline(x,y,dely, 4);
	test_evaluate(qs);
	test_derivative(qs);

	WriteLine("\n==================================================");
	WriteLine($"I plot the second derivatives of my two splines in {file1}.svg");	

	var outfile = new System.IO.StreamWriter(file1.ToLower() +".data");
	outfile.WriteLine("##x	cubic 	quartic");
	for(int i = 0; i<900; i++){
		double a = -0.95 + 2.0*i/1000;
		outfile.WriteLine($"{a}	{ss.derivative2(a)}	{qs.derivative2(a)}");
	}
	outfile.Close();
	
}

public static void test_evaluate(subspline ss){
	rand = new Random(); bool passed; double acc;
	double z;
	
	WriteLine("\nTesting build and subspline.evaluate()...");
	acc = 1e-6;
	do{
	passed = true;
	for(int i = 0; i<50; i++){
		z = rand.NextDouble()*(x[x.size-2]-x[1])+x[1]; //pick random number in interval
		passed = passed && matrix.approx(ss.evaluate(z), f(z), acc, acc);}
	acc*=10;
	}while(passed == false && acc<1);
	
	WriteLine($"... {passed ? "PASSED" : "FAILED"} 50 random samples with relative accuracy of {acc/10}");

}

public static void test_derivative(subspline ss){
	rand = new Random(); bool passed; double acc;
	double z;
	
	WriteLine("\nTesting subspline.derivative()...");
	acc = 1e-6;
	do{
	passed = true;
	for(int i = 0; i<50; i++){
		z = rand.NextDouble()*(x[x.size-2]-x[1])+x[1]; //pick random number in interval
		passed = passed && matrix.approx(ss.derivative(z), delf(z), acc, acc);}
	acc*=10;
	}while(passed == false && acc < 1);
	
	WriteLine($"... {passed ? "PASSED" : "FAILED"} 50 random samples with relative accuracy of {acc/10}");



	
	WriteLine("\nTesting second derivative...");
	acc = 1e-6; double av_acc;
	do{
	av_acc = 0;
	passed = true;
	for(int i = 0; i<50; i++){
		z = rand.NextDouble()*(x[x.size-2]-x[1])+x[1]; //pick random number in interval
		passed = passed && matrix.approx(ss.derivative2(z), delf2(z), acc, acc);
		av_acc += Pow(delf2(z)-ss.derivative2(z), 2);}
	acc*=10;
	}while(passed == false && acc < 1);

	av_acc /= 50;
	
	WriteLine($"... {passed ? "PASSED" : "FAILED"} 50 random samples with relative accuracy of {acc/10} - average accuracy was {Sqrt(av_acc)}");

	}
	
public static void test_integrate(subspline ss){
	rand = new Random(); bool passed; double acc;

	WriteLine("\nTesting definite integral function subspline.integrate(A, B)...");
	acc = 1e-6;
	do{
	passed = true;
	for(int i = 0; i<50; i++){
		double A = rand.NextDouble()*(x[x.size-2]-x[1])+x[1]; //pick random number in interval
		double B = rand.NextDouble()*(x[x.size-2]-x[1])+x[1]; //pick random number in interval
		
		passed = passed && matrix.approx(ss.integrate(A, B), F(B)-F(A), acc, acc);}
	acc*=10;
	}while(passed == false && acc < 1);
	
	WriteLine($"... {passed ? "PASSED" : "FAILED"} 50 random samples with relative accuracy of {acc/10}");

}


}
