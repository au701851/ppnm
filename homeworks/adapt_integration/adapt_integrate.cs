using static System.Math;
using System.IO;
using System;
using static System.Console;
public class adapt_integrate{
public static int counter; //number of subdivisions
public static int evals; //number of integrand evaluations
public static double error2;
public static (double, double) integrate(Func<double, double> f, double a, double b,
								double del = 0.001, double eps = 0.001,
								double f2 = double.NaN, double f3 = double.NaN, double maxit = 20000){
	if(a>b){
		Func<double, double> g = (double t) => (-f(t));
	 	return integrate(g, b, a, del, eps, f2, f3, maxit);}

	//Testing for infinite limits:
	if(double.IsInfinity(-a) || double.IsNegativeInfinity(a)){
		if(Double.IsInfinity(b) || double.IsPositiveInfinity(b)){
			//both limits are infinity
			Func<double, double> g = (double t) => ((f((1-t)/t) + f(-(1-t)/t))/t/t);
			return integrate(g, 0, 1, del, eps, f2, f3, maxit);
		}
		else{ //only lower limit is infinity
			Func<double, double> g = (double t) => (f(b - (1-t)/t)/t/t);
			return integrate(g, 0, 1, del, eps, f2, f3, maxit);
		}}
	else if(Double.IsInfinity(b) || double.IsPositiveInfinity(b)){
		//only upper limit is infinity
		Func<double, double> g = (double t) => (f(a+(1.0-t)/t)/t/t);
		return integrate(g, 0, 1, del, eps, f2, f3, maxit);
	}
	

								
	double h = b-a; //stepsize
	if(h < 1e-9) return (0.0, Sqrt(error2)); //around divergences, h becomes very small as the integral is split very finely -- therefore, if h becomes very small, 2/6h becomes 0, and the function ends up being evaluated at the singularity :(

	if(double.IsNaN(f2) || double.IsNaN(f3)){
		counter = 0; evals = 2; error2 = 0; 
		f2 = f(a + 2.0/6.0*h); f3 = f(a + 4.0/6.0*h);
		//Using the standard from eq (48) in the material
	}
	if(counter > maxit) throw new Exception("Max subdivisions succeeded");
	double f1 = f(a + 1.0/6.0*h); double f4 = f(a + 5.0/6.0*h); evals += 2;

	//I calculate the best approximation of the integral by using the trapezium rule
	double Q = (2.0/6.0*f1 + 1.0/6.0*f2 + 1.0/6.0*f3 + 2.0/6.0*f4)*h;

	//and a lower order approximation with the rectangle rule
	double q = (1.0/4.0*f1 + 1.0/4.0*f2 + 1.0/4.0*f3 + 1.0/4.0*f4)*h;
	//The factor h above is added to rescale the integral from 0 to 1 to a to b. 

	//I estimate the error as the difference between the two approximations
	double err = Abs(Q-q);
	if(double.IsNaN(err)) throw new Exception("Something went wrong in calculating the error");
	
	if(err <= del + Abs(Q)*eps) {//the factor Sqrt(h/(b-a)) is one here, so it is not added 
		error2 += err*err;
		return (Q, Sqrt(error2)); //If the error is small enough
		}
	else{
		counter++;
		return (integrate(f, a, (a+b)/2.0, del/Sqrt(2.0), eps, f1, f2, maxit-1).Item1
			 + integrate(f, (a+b)/2.0, b, del/Sqrt(2.0), eps, f3, f4, maxit-1).Item1, Sqrt(error2));}
				//Split the integral in two even parts
				//And pass the calculated points on, so I can halve the number of calculations. 
				//The absolute accuracy is lowered, since h -> h/2, so Sprt(h/(b-a)) -> Sqrt(2).
}

public static (double, double) ccintegrate(Func<double, double> f, double a, double b,
								double del = 0.001, double eps = 0.001, double maxit = 100) {
	Func<double, double> g = (double theta) => (f((a+b)/2+(b-a)/2*Cos(theta))*Sin(theta)*(b-a)/2);
	return integrate(g, 0, PI, del, eps, double.NaN, double.NaN, maxit);
}

public static bool approx(double x, double y, double acc=0.001, double eps=0.001){
	if(Abs(x-y)<acc)return true;
		if(Abs(x-y)/Max(Abs(x),Abs(y))<eps)return true;
			return false;
				}

//Implementation of the error function using my integration routine
public static double erf(double z){
	if(z<0) return -erf(-z);
	else if(0.0 <= z || z <= 1.0)
		return 2.0/Sqrt(PI)*integrate((x)=>(Exp(-x*x)), 0, z).Item1;
	else
		return 1.0 - 2.0/Sqrt(PI)*integrate(
					(t) => Exp(-Pow(z+(1-t)/t, 2))/t/t, 
					0, 1).Item1;

}


}
