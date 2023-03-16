using System;
using static System.Math;
using static System.Console;
public class spline{
	public vector x, y, a, b, c;
	public int len;
	public string mode;
	
	public spline(vector xs, vector ys, string _mode){
		if(xs == null || ys == null) throw new ArgumentException("please not null");
		len = (int) xs.size;
		if(len != ys.size) throw new ArgumentException("x and y must be of the same dimension");
		x = xs.copy(); y = ys.copy();
		a = new double[len-1];
		b = new double[len-1];
		c = new double[len-1];
		if(_mode == "l")
			this.linterp();
		if(_mode == "q")
			this.qinterp();
		this.mode = _mode;
	}

	public double evaluate(double z){
		if(mode == "l") return linterpevaluate(z);
		else throw new Exception("Choose a mode");
	}
	public double integral(double z){
		if(mode == "l") return linterpInteg(z);
		else throw new Exception("Choose a mode");
	}
	public double derivative(double z){
		if(mode == "l") throw new Exception("You're in linear interpolation mode, where the derivative is not continuos.");
		
		else throw new Exception("Choose a mode");
	}

	public void qinterp(){
	// Build quadratic spline, yi(x) = a(x - xi)^2 + b(x-xi) + c
		a[0] = 0;
		b[0] = (y[1]-y[0])/(x[1]-x[0]);
		double dx, dy;

		//Calculating p and upward recursion
		for(int i = 1; i<len-1; i++){
			dx = x[i+1]-x[i];
			dy = y[i+1]-y[i];
			//c[i] = y[i]; //Just use y[i] instead
			b[i] = dy/dx; //p[i]
			a[i] = 1/dx*(b[i] - b[i-1] - a[i-1]*(x[i]-x[i-1]));
		}
		//use c to calculate as by downward recursion:
		c[len-2] = 0;
		for(int i = len-3; i>=0; i--){
			dx = x[i+1]-x[i];
			dy = y[i+1]-y[i];
			c[i] = 1/dx*(b[i+1] - b[i] - c[i+1]*(x[i+2]-x[i+1]));	
		}
		//Reforming p to b, and averaging a and c:
		for(int i = 0; i<len-1; i++){
			a[i] += c[i]; a[i] /= 2.0;
			b[i] -= a[i]*(x[i+1]-x[i]);
			c[i] = y[i];
		}
	}

	public double qinterpevaluate(double z){
		//interpolates the value of z from quadratic spline of y(x)
		int i = binsearch(x, z); 
		//the quadratic spline is y(x) = y[i] + b[i]*(x-x[i]) + c[i]*(x-x[i])^2...
		return y[i] + b[i]*(z-x[i]) + c[i]*Pow((z-x[i]),2);
	}	
	

	public void linterp(){
		for(int i = 0; i < len-1; i++){
			double dx = x[i+1]-x[i];
			if(dx <= 0) throw new Exception("x must be a series of rising values, x[i+1] > x[i]");
			double dy = y[i+1]-y[i];
			a[i] = dy/dx; b[i] = y[i];
			}
	}

	public double linterpevaluate(double z){
		//interpolates the value of z from a linear spline of y(x)
		int i = binsearch(x, z); //Now we know to calculate the interpolant between x[i] and x[i+1]
		//the linear spline is y(x) = y[i] + dy/dx*(x-x[i])...
		return b[i] + a[i]*(z-x[i]);	
	}

	public double linterpInteg(double z){
		//interpolates the integral from x[0] to z over y(x) using a linear spline
		double integ = 0;
		double dx;

		int j = binsearch(x, z);
		for(int i = 0; i<j; i++){
			//integral of y[i] + dy/dx*(x-x[i]) from x[i] to x[i+1] is y[i]*dx + dy/dx/2*(dx**2)
			dx = x[i+1]-x[i]; if(dx <= 0) throw new Exception("x must be increasing!");
			integ += b[i]*dx + a[i]/2*Pow(dx,2);
		}
		dx = x[j+1]-x[j];
		integ += b[j]*(z-x[j]) + a[j]/2*Pow(z-x[j],2);
		return integ;
	}

	//Function for binsearching, to locate z in interval x
	public static int binsearch(vector x, double z){
		//z must be in the interval of x:
		if(z <= x[0] || z >= x[x.size -1]) 
			throw new ArgumentException("Binsearch can not locate a variable z outside the interval x");

		//introduce integers i and j, indices of first and last element of the subarray, where we look for z.
		int i = 0, j = x.size-1;
		while(i < j-1){ //as long as we're considering an interval...
			int mid = (j+i)/2; //middle of array
			if(z > x[mid]) i = mid; //z is in upper half
			if(z <= x[mid]) j = mid; //z is in lower half
		}
		return i; //z is in interval (x[i]; x[i+1])
	}
}
