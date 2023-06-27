using System;
using static System.Math;
using static System.Console;
public class spline{
	public vector x, y, a, b, c, d;
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
		d = new double[len-1];
		if(_mode == "l")
			this.linterp();
		if(_mode == "q")
			this.qinterp();
		if(_mode == "c")
			this.cinterp();
		this.mode = _mode;
	}

	public double evaluate(double z){
		if(mode == "l") return linterpevaluate(z);
		else if(mode == "q") return qinterpevaluate(z);		
		else if(mode == "c") return cinterpevaluate(z);
		else throw new Exception("Choose a mode");
	}
	public double integral(double z){
		if(mode == "l") return linterpInteg(z);
		else if(mode == "q") return qinterpInteg(z);
		else if(mode == "c") return cinterpInteg(z);
		else throw new Exception("Choose a mode");
	}
	public double derivative(double z){
		if(mode == "l") throw new Exception("You're in linear interpolation mode, where the derivative is not continuos.");
		else if(mode == "q") return qinterpderivative(z);
		else if(mode == "c") return cinterpderivative(z);
		else throw new Exception("Choose a mode");
	}
	public double derivative2nd(double z){
		if(mode == "l" || mode == "q") throw new Exception("Second dericative is only available in cubic mode");
		else if(mode == "c") return cinterp2derivative(z);
		else throw new Exception("Choose a mode"); 
	}


	public void cinterp(){
	//Build a cubic spline, Si(x) = yi + bi(x-xi) + ci(x-xi)^2 + di(x-xi)^3
		//First construct vectors D, Q and B for finding bi
		vector D = new vector(len);
		vector Q = new vector(len);
		vector B = new vector(len);
		vector C = new vector(len); 

		double dy = (y[1]-y[0]);
		double h = (x[1]-x[0]);
		double dyi, hi;
		
		D[0] = 2; Q[0] = 1; B[0] = 3*dy/h; C[0] = 0;
		
		for(int i = 0; i < len-2; i++){
			dyi = (y[i+2]-y[i+1]); //dy_(i+1)
			hi = (x[i+2] - x[i+1]); //h_(i+1)

			D[i+1] = 2*h/hi + 2; 
			Q[i+1] = h/hi;
			B[i+1] = 3*(dy/h + dyi/hi*h/hi);
			C[i+1] = 1;


			dy = dyi;
			h = hi;			
		}
		D[len-1] = 2; B[len-1] = 3*dy/h; Q[len-1] = 0;
		
		//Finding vector b by solving eq (22)
		b = tridiag(C, D, Q, B);

		//Finding c and d from b, and setting a = yi
		for(int i = 0; i < len-1; i++){
			hi = x[i+1]-x[i];
			double pi = (y[i+1]-y[i])/hi;
			
			a[i] = y[i];
			c[i] = 1.0/hi*(-2*b[i]-b[i+1]+3*pi);
			d[i] = 1.0/hi/hi*(b[i]+b[i+1]-2*pi);

		}	
	}

	public double cinterpevaluate(double z){
		int i = binsearch(x, z);
		return a[i] + b[i]*(z-x[i]) + c[i]*Pow((z-x[i]),2) + d[i]*Pow((z-x[i]), 3);
	}

	public double cinterpderivative(double z){
		int i = binsearch(x, z);
		return b[i] + 2*c[i]*(z-x[i]) + 3*d[i]*Pow(z-x[i], 2);
	}

	public double cinterp2derivative(double z){
		int i = binsearch(x, z);
		return 2*c[i] + 6*d[i]*(z-x[i]);
	}

	public double cinterpInteg(double z){
		int j = binsearch(x, z);

		double sum = 0; double dx;
		for(int i = 0; i<j; i++){
			dx = x[i+1]-x[i];
			sum += a[j]*dx + b[i]/2*dx*dx + c[i]/3*dx*dx*dx + d[i]/4*dx*dx*dx*dx;
		}
		dx = z-x[j];
		sum += a[j]*dx + b[j]/2*dx*dx + c[j]/3*dx*dx*dx + d[j]/4*dx*dx*dx*dx;
		return sum;

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
	
	public double qinterpInteg(double z){
		int j = binsearch(x, z);
		
		double integ = 0;
		double dx;
		for(int i = 0; i<j; i++){
			dx = x[i+1]-x[i];
			//The integral from xi to xi+1 is a/3dx^3 + b/2dx^2 + cdx
			integ += a[i]/3*Pow(dx, 3) + b[i]/2*dx*dx + c[i]*dx;
		}
		dx = z-x[j];
		integ += a[j]/3*Pow(dx, 3) + b[j]/2*dx*dx + c[j]*dx;
		return integ;		
	}

	public double qinterpderivative(double z){
		int j = binsearch(x, z);
		return 2*a[j]*(z-x[j]) + b[j];
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

	//Solves the tridiagonal set of eq aix_(i-1) + bixi + cix_(i+1) = di
	//All vectors must have length n, but the routine assumes a1 = cn = 0
	public static vector tridiag(vector a, vector b, vector c, vector d){
		int n = d.size;
		if((a.size != n) || (c.size != n) || (b.size != n))
				throw new Exception("Vector a, b, c, d should have the same length. This routine sets a1=cn=0");

		//Gauss elimination
		for(int i = 1; i < n; i++){
		double w = a[i]/b[i-1];
		b[i] -= w*c[i-1];
		d[i] -= w*d[i-1];
		}

		//backwards substitution
		vector x = new vector(n);
		x[n-1] = d[n-1]/b[n-1];
		for(int i = n-2; i >= 0; i-=1)
			x[i] = (d[i]-c[i]*x[i+1])/b[i];

		return x;

	}
}
