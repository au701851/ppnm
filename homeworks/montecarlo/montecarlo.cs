using System;
using static System.Math;
using static System.Console;
using System.Collections.Generic;
public class montecarlo{

public static Random rnd = new Random();
public static int[] bases = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67}; //Set of co prime numbers for Halton
public static int NMIN = 500; //lowest amount of points in rssmc
public static int COUNT = 0;

public static double rsssum;
public static double rssvar;
public static int N0;
public static double V0; 

//Integrate function f in region between vectors a and b
public static (double, double) plainmc(Func<vector,double> f, vector a, vector b, int N, List<vector> points = null, bool rss = false){
	int dim = a.size; //dimension of the problem
	double V = 1;
	for(int i = 0; i<dim; i++)
		V*=Abs(b[i]-a[i]); //volume of multidimensional box
	double sum = 0; //sum of f(x) to calc f_average
	double sum2 = 0; //sum of (f(x))^2 for calculating variance

	vector x = new vector(dim); //for holding random points
	for(int i = 0; i < N; i++){ //do montecarlo with N random picks 
		for(int j = 0; j < dim; j++)
			x[j] = a[j] + rnd.NextDouble()*(b[j]-a[j]); //Pick random x within region 
		if(points != null) points.Add(x); //possible to record path
		double fx = f(x); 
		sum += fx; 
		sum2 += fx*fx;
	}
	
	double f_avg = sum/N; 
	double sigma = Sqrt(sum2/N - f_avg*f_avg); //error is squareroot of variance
	
	//passing values on to rss if needed
	if(rss && !double.IsNaN(sum/N*V)) {rsssum += sum/N*V;}
	if(rss) {rssvar += sigma*sigma;}
	
	var result = (f_avg*V, sigma*V/Sqrt(N)); //formula (2) and (3) in the material
	return result;

}//method end


public static (double, double) rssmc(Func<vector, double> f, vector a, vector b, int N, List<vector> points = null){
	rsssum = 0; rssvar = 0; N0 = N;
	
	int dim = a.size;
	double V = 1;
	for(int i = 0; i<dim; i++)
		V*=Abs(b[i]-a[i]); //volume of multidimensional box
	V0 = V;
	rssmc_recursive(f, a, b, N, points); 
	return (rsssum, Sqrt(rssvar));
}



public static (double, double) rssmc_recursive(Func<vector, double> f, vector a, vector b, int N, List<vector> points = null){	
	COUNT += 1;
	int dim = a.size;
	double V = 1;
	for(int i = 0; i<dim; i++)
		V*=Abs(b[i]-a[i]); //volume of multidimensional box
	
	if(N<NMIN) {
		plainmc(f, a, b, N, points, true);
		var finalresult = (rsssum, Sqrt(rssvar)/Sqrt(N0)*V0);
		return finalresult;
		}

	
	vector newb, newa;
	

	
	vector[] suberr = new vector[dim];
	int maxsuberr = 0;
	for(int i = 0; i<dim; i++){
		newb = b.copy(); newa = a.copy();
		newb[i] = (b[i]-a[i])/2 + a[i];
		newa[i] = newb[i];
		
		var up = plainmc(f, newa, b, NMIN/dim/2, points);
		var low = plainmc(f, a, newb, NMIN/dim/2, points);
		suberr[i] = new vector(up.Item2*up.Item2, low.Item2*low.Item2);
		if(suberr[i].norm() >= suberr[maxsuberr].norm()) maxsuberr = i;
	}

	double ratio = suberr[maxsuberr][0]/(suberr[maxsuberr].norm());
	if(double.IsNaN(ratio)) ratio = 0; 
	int NUP = Convert.ToInt32((N-NMIN)*ratio);
	int NLOW = (N-NMIN)-NUP;
	
	newb = b.copy(); newa = a.copy();
	
	newb[maxsuberr] = (b[maxsuberr]+a[maxsuberr])/2;
	newa[maxsuberr] = newb[maxsuberr];
	rssmc_recursive(f, newa, b, NUP, points);
	rssmc_recursive(f, a, newb, NLOW, points);


	
	
	var result = (rsssum, rssvar);
	return result;
	
	
	
}//method end

//Integrate using quasi-random sequences
public static (double, double) quasimc(Func<vector, double> f, vector a, vector b, int N){
	int dim = a.size;
	double V = 1;
	for(int i = 0; i<dim; i++)
		V*=Abs(b[i]-a[i]); //volume of multidimensional box
	double sum = 0, sum2 = 0; //sum of f(x) to calc f_average

	vector x = new vector(dim); //for holding random x values
	for(int i = 0; i < N; i++){ //do montecarlo with N random picks 
		halton(i, dim, x, a, b);
		double fx = f(x); 
		sum += fx; 
		halton(i, dim, x, a, b, true); //Second sequence for error estimation
		sum2 += f(x);
	}	

	double f_avg = sum/N*V;
	double err = Abs(sum-sum2)/N*V;
	return (f_avg, err);

}//method end

//Calculate van der Corput number q_b(n)
public static double corput(int n, int b){
	double q = 0; 
	double bk = 1.0/b; //b to the power of minus k
	while(n>0) {
		q += (n % b)*bk; 
		n/=b;
		bk /= b; 
	}
	return q;
}// method end

//Fill a vector x with quasirandom numbers using a halton series
public static void halton(int n, int d, vector x, vector a, vector b, bool lower = false){
	//Each dimension is assigned a base
	for(int i = 0; i<d; i++){
		if(lower) x[i] = corput(n, bases[i+5])*(b[i]-a[i]);
		else x[i] = corput(n, bases[i])*(b[i]-a[i]);
	}
}//method end

}//class end
