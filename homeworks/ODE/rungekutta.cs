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

public (genlist<double>, genlist<vector>) driver(
					double a, vector ya,
					double b, double h = 0.1){
	if(a>b) throw new ArgumentException("The integration must be in positive direction, a must be less than b");
	double x = a; vector y = ya.copy(); //copies to preserved original pointers
	var xlist = new genlist<double>(); 
	xlist.add(x);
	var ylist = new genlist<vector>();
	ylist.add(y);

	//Instead of forlooping over a double (illegal!) I make a while loop and terminate the function by returning a value inside.
	while(true){
		if(x>=b) return (xlist, ylist); //The integration ends when I have reached b
		if(x+h>b) h = b-x; //if another step takes us above b, h is adjusted so we end at x = b.
		(var yh, var erv) = rkstep12(x, y, h);
		double tol = Max(acc, yh.norm()*eps) * Sqrt(h/(b-a)); 
			//The squareroot above is added to make the errors statistically uncorrelated
		double err = erv.norm();
		if(err<=tol){
			x += h; y = yh;
			xlist.add(x);
			ylist.add(y);
		}
		h *= Pow(tol/err, 0.25)*0.95; //Some empirically optimised formula for adjustable step size. 
	}
}
}
