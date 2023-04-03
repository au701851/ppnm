using System;
using static System.Console;
using static System.Math;
public static class main{
public static string fileA;

public static void Main(string[] args){
	fileA = null;
	foreach(var arg in args){
		var words = arg.Split(":");
		if(words[0] == "-fileA"){ 
			fileA = words[1];}
		}
	
	WriteLine("-------------------------------");
	WriteLine("PART A - RECURSIVE ADAPTIVE INTEGRATOR");
	partA();
	if(fileA == null) throw new Exception("Please pass a name to fileA");
	erf(fileA);
	WriteLine("-------------------------------");
	WriteLine("PART B - OPEN QUADRATURE WITH CLENSHAW-CURTIS");
	partB();
	WriteLine("-------------------------------");
	WriteLine("PART C - ERROR ESTIMATE AND INFINTIE LIMITS");
	partC();
	WriteLine("-------------------------------");
	}
public static void partA(){
	WriteLine("I have implemented the recursive adaptive integrator using the trapezium rule as my higher order rule, and the rectangular rule as my lower.");
	WriteLine("To test the below integrals, i have used acc = 0.001 and eps = 0.001, and I use these same numbers when comparing my results to the expected values.");
	WriteLine("I also had to implement a lower limit on h (stepsize), to avoid evaluation of the function in its singularties. This limit is now 1e-9.");
	WriteLine("Let's test it on some integrals:");


	WriteLine("\nTesting ∫_0^1 dx √(x) (should be 2/3) ...");
	Func<double, double> f = (double x) => (Sqrt(x));
	//WriteLine($"{f(1.0/6)}	{f(2.0/6)}");
	double res = adapt_integrate.integrate(f, 0, 1).Item1;
	WriteLine($"Result: {res}, {adapt_integrate.approx(res, 2.0/3.0) ? "PASSED" : "FAILED"}");
	WriteLine($"with {adapt_integrate.counter} subdivisions and {adapt_integrate.evals} integrand evaluations.");

	WriteLine("\nTesting ∫_0^1 dx 1/√(x) (should be 2) ...");
	f = (double x) => (1.0/Sqrt(x));
	res = adapt_integrate.integrate(f, 0, 1).Item1;
	WriteLine($"Result: {res}, {adapt_integrate.approx(res, 2.0) ? "PASSED" : "FAILED"}");
	WriteLine($"with {adapt_integrate.counter} subdivisions and {adapt_integrate.evals} integrand evaluations.");

	WriteLine("\nTesting ∫_0^1 dx 4√(1-x^2) (should be pi) ...");
	f = (double x) => (4.0*Sqrt(1-x*x));
	res = adapt_integrate.integrate(f, 0, 1).Item1;
	WriteLine($"Result: {res}, {adapt_integrate.approx(res, PI) ? "PASSED" : "FAILED"}");
	WriteLine($"with {adapt_integrate.counter} subdivisions and {adapt_integrate.evals} integrand evaluations.");

	WriteLine("\nTesting ∫_0^1 dx ln(x)/√(x) (should be -4) ...");
	f = (double x) => (Log(x)/Sqrt(x));
	res = adapt_integrate.integrate(f, 0, 1).Item1;
	WriteLine($"Result: {res}, {adapt_integrate.approx(res, -4) ? "PASSED" : "FAILED"}");
	WriteLine($"with {adapt_integrate.counter} subdivisions and {adapt_integrate.evals} integrand evaluations.");

	WriteLine($"\nI have implemented the error function in its integral form, and calculated some of its values in {fileA}, and plotted them in the corresponding svg file.");
}

public static void partB(){
	WriteLine("For part B, I had to implement an integral transform. I have constructed a method, which properly transforms the integrand and the limits, and passes this on to my routine from part A.");
	WriteLine("Let's test it:");

	WriteLine("\nTesting ∫_0^1 dx 1/√(x) (should be 2)");
	Func<double, double> f = (double x) => (1/Sqrt(x));
	(double res, double err) = adapt_integrate.ccintegrate(f, 0, 1);
	WriteLine($"Result: {res} +- {err}, {adapt_integrate.approx(res, 2) ? "PASSED" : "FAILED"}");
	WriteLine($"with {adapt_integrate.counter} subdivisions and {adapt_integrate.evals} integrand evaluations.");
	
	WriteLine("\nTesting ∫_0^1 dx ln(x)/√(x) (should be -4)");
	f = (double x) => (Log(x)/Sqrt(x));
	(res, err) = adapt_integrate.ccintegrate(f, 0, 1);
	WriteLine($"Result: {res} +- {err}, {adapt_integrate.approx(res, -4) ? "PASSED" : "FAILED"}");
	WriteLine($"with {adapt_integrate.counter} subdivisions and {adapt_integrate.evals} integrand evaluations.");

	WriteLine("\nIt is clear that the integral transform significantly lowers the needed amount of operations.");
	WriteLine("I solved the same two integrals with pythons scipy, which relies on a fortran routine.\nIt solved ln(x)/sqrt(x) in only eight subdivisions, but with 315 integrand evaluations, and the 1/sqrt(x) integral in 6 subdivisions and 231 integrand evaluations.");
	WriteLine("It did however find the results with much greater precision, so it would appear, that for now, scipys routine works better, when more difficult integrals must be solved with higher precision."); 
	WriteLine("I tried lowering my tolerance to acc = 1e-08 and rel = 1e-08, but the my routine crashed, even when I set max subdivisions to 20.000... Could it be, that the rules I have used are not optimal for such high precision?");
}

public static void partC(){
	WriteLine("For the first part of c, I made my routine return a tuple with the result and the estimated error. This is the error, I have printed for the evaluations of part b.");
	WriteLine("The second part of c was to generalise my routine to accept infinite limits. I have implemented this by a series of checks in the beginning of my routine, which transforms the limits and the integrand, in accordance with eqs. (60) - (65) in the material");
	WriteLine("Let's test it on some convergent integrals:");

	WriteLine("\nTesting ∫_0^oo exp(-x) dx (should be 1)");
	Func<double, double> f = (double x) => (Exp(-x));
	(double res, double err) = adapt_integrate.integrate(f, 0, double.PositiveInfinity);
	WriteLine($"Result: {res} +- {err}, {adapt_integrate.approx(res, 1) ? "PASSED" : "FAILED"}");
	WriteLine($"with {adapt_integrate.counter} subdivisions and {adapt_integrate.evals} integrand evaluations.");

	WriteLine("\nTesting ∫_-oo^oo dx 1/(1+x^2) (should be pi)");
	f = (double x) => 1/(1+x*x);
	(res, err) = adapt_integrate.integrate(f, double.NegativeInfinity, double.PositiveInfinity);
	WriteLine($"Result: {res} +- {err}, {adapt_integrate.approx(res, PI) ? "PASSED" : "FAILED"}");
	WriteLine($"with {adapt_integrate.counter} subdivisions and {adapt_integrate.evals} integrand evaluations.");

	WriteLine("\n I made scipy solve the same integrals -- It solved 1/(1+x^2) in only one subdivision (30 evaluations), which seems to show, that it must hold some convenient variable transformation, suitable for solving this integral... Or, perhaps, it is just quickly converging. Afterall, my routine solved it in less than 30 evaluations too");
	WriteLine("For exp(-x), however, scipy needed 3 subdivisions and 75 function evaluations, which can't compete with my routine. However, it acchieves a much more accurate result, even when I manually lower the required accuracy on it.");
}

public static void erf(string filename){
	var outfile = new System.IO.StreamWriter(filename);
	for(double x = 0+1.0/64; x<3.0; x+=1.0/8)
		outfile.WriteLine($"{x}	{adapt_integrate.erf(x)}");
	
	outfile.Close();
}
}
	
