using System;
using static System.Math;

public class rungekutta{
public Func<double, vector, vector> f; //f from dy/dx = d(x,y)
public double acc, eps;
public rungekutta(Func<double, vector, vector> _f){
	f = _f;
	acc = 0.01; //absolute accuracy goal
	eps = 0.01; //relative accuracy goal
}

public (vector, vector) rkstep12(double x, vector y, double h){
	vector k0 = f(x,y); //tangent in x
	vector k1 = f(x+h/2, y+k0*(h/2)); //tangent in x+h/2
	vector yh = y + k1*h;
	vector er = (k1-k0)*h; //the error compared to taking a half h step
	return (yh,er);
}

public vector driver(double a, vector ya, double b, double h = 0.1,
					genlist<double> xlist = null, genlist<vector> ylist = null){
	if(a>b) throw new ArgumentException("The integration must be in positive direction, a must be less than b");
	double x = a; vector y = ya.copy(); //copies to preserved original pointers
	if(xlist != null && ylist != null){ //lists are only filled out if they are given
		xlist.add(x);
		ylist.add(y);}
	vector tol = new vector(y.size);
	//Instead of forlooping over a double (illegal!) I make a while loop and terminate the function by returning a value inside.
	while(true){
		if(x>=b) return y; //The integration ends when I have reached b, and I return the final vector
		if(x+h>b) h = b-x; //if another step takes us above b, h is adjusted so we end at x = b.
		(var yh, var erv) = rkstep12(x, y, h);
		
		for(int i = 0; i<y.size; i++)
			tol[i] = Max(acc, Abs(yh[i])*eps) * Sqrt(h/(b-a)); 
			//The squareroot above is added to make the errors statistically uncorrelated
			//PART B: yh.norm() was exchanged for yh[i] above
			
		//double err = erv.norm(); CHANGED FOR PART B
		bool ok = true;
		for(int i = 0; i < y.size; i++)
			if(!(erv[i]<tol[i])) ok = false; //compares erv and tol componentwise rather than by size
		

		if(ok){
			x += h; y = yh;
			if(xlist != null && ylist != null){
				xlist.add(x);
				ylist.add(y);}
		}
		//The factor tol/err below is replaced by the smallest quotient between components of tol and erv
		double quotient = tol[0]/Abs(erv[0]);
		for(int i = 1; i<y.size; i++)
			quotient = Min(quotient, tol[i]/Abs(erv[i]));
		h *= Pow(quotient, 0.25)*0.95; //Some empirically optimised formula for adjustable step size. 
	}
	}
	
}
