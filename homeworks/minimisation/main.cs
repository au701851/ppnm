using static System.Console;
using static System.Math;
using System;
public class main{

public static double ACC = 1e-9;
public static double resACC = 0.01;
public static genlist<double> E, sigma, err;
public static string fileB;

public static void Main(string[] args){
	foreach(var arg in args){
		var words = arg.Split(":");
		if(words[0] == "-fileB") fileB = words[1];
	}	
	WriteLine("============================================");
	WriteLine("===================PART A===================");
	partA();
	WriteLine("============================================");
	WriteLine("===================PART B===================");
	partB();
	WriteLine("============================================");
}
public static void partA(){
	WriteLine("For part a I had to implement a quasi newton method for minimization. I used a numerical gradient.");

	WriteLine("\nFinding minimum of Rosenbrock's valley function...");
	vector res = mini.qnewton((vector a) => ((1-a[0])*(1-a[0])+100*Pow(a[1]-a[0]*a[0], 2)), new vector(-1,0.5), ACC);
	res.print("Result (should be (1,1)): ");
	WriteLine($"{vector.approx(new vector(1,1), res, resACC) ? "PASSED" : "FAILED"}");

	bool passed = true;
	Func<vector, double> f = (vector a) => (Pow(a[0]*a[0]+a[1]-11, 2)+Pow(a[0]+a[1]*a[1]-7, 2));
	WriteLine("\nFinding minimum of Himmelblau's function...");
	WriteLine("After seeing a plot of the function, I decide to place my start values at each corner of a 10x10 square centered on the origin, hoping it will find all of the minima.");
	res = mini.qnewton(f, new vector(-4,-4), ACC);
	res.print($"{himmelblau(res) ? "Acceptable" : "Unacceptable"} result: ");
	passed = passed & himmelblau(res);
	res = mini.qnewton(f, new vector(4,4), ACC);
	res.print($"{himmelblau(res) ? "Acceptable" : "Unacceptable"} result: ");
	passed = passed & himmelblau(res);
	res = mini.qnewton(f, new vector(4,-4), ACC);
	res.print($"{himmelblau(res) ? "Acceptable" : "Unacceptable"} result: ");
	passed = passed & himmelblau(res);
	res = mini.qnewton(f, new vector(-4,4), ACC);
	res.print($"{himmelblau(res) ? "Acceptable" : "Unacceptable"} result: ");
	passed = passed & himmelblau(res);
	WriteLine($"... {passed ? "PASSED" : "FAILED :("}");

	WriteLine("\nI note that not all minima of the Himmelblau function are found, despite the different starting points. \nThis is of course because the routine isn't very critical in accepting a step, requiring only that it's lower than the current position, and can as such take very long steps.\nTherefore the routine risks 'tunneling' through a 'hill' of the function, and missing a minimum");
}

public static void partB(){
	WriteLine("For part B, I used my minimisation routine to minimise chi squared for a fit.");
	WriteLine("Using this method, I optimised the parameters of the Breit-Wigner function to fit experimental data.");
	WriteLine($"The result is plotted in {fileB.Replace(".txt", ".svg")}, and the results shortly sketched out below:\n");


	E = new genlist<double>();
	sigma = new genlist<double>();
	err = new genlist<double>();

	var separators = new char[] {' ','\t'};
	var options = StringSplitOptions.RemoveEmptyEntries;
	do{
		string line=Console.In.ReadLine();
	    if(line==null) break;
	    string[] words=line.Split(separators,options);
	    if(words[0] != "#"){
	    E.add(double.Parse(words[0]));
	    sigma.add(double.Parse(words[1]));
	    err.add(double.Parse(words[2]));}
	    }while(true);
	    
	vector res = mini.qnewton(D, new vector(123, 2, 6), 1e-6);
	res.print("Result is:");
	WriteLine($"With {mini.counter} iterations and chi^2 = {D(res)}");

	WriteLine($"\nThus, I have found the mass of the Higg's boson to be {res[0]} and the experimental width to be {res[1]}.");
	WriteLine($"This is, with an estimated error of +-1, {vector.approx(res[0],125.3, 0.8) ?  "in" : "not in"} accordance with theoretical values.");

	var outfile = new System.IO.StreamWriter(fileB);
	outfile.WriteLine("Energy	signal");
	for(int i = 100; i<160; i++)
		outfile.WriteLine($"{i}	{F(i, res[0], res[1], res[2])}");
	outfile.Close();
}

public static double F(double E, double m, double G, double A){
	return A/(Pow(E-m, 2)+G*G/4);
}
public static double D(vector p){
	double m = p[0];
	double G = p[1];
	double A = p[2];
	
	double res = 0;
	for(int i = 0; i<E.size; i++){
		res += Pow((F(E[i], m, G, A)-sigma[i])/err[i], 2);
	}
	return res; 
}

public static bool himmelblau(vector res){
	if(vector.approx(new vector(3,2), res, resACC)) return true;
	if(vector.approx(new vector(-2.805118,3.131312), res, resACC,resACC)) return true;
	if(vector.approx(new vector(-3.779310,-3.283186), res, resACC, resACC)) return true;
	if(vector.approx(new vector(3.584428, -1.848126), res, resACC, resACC)) return true;
	return false; 
}
}
