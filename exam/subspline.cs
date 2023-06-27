using System;
using static System.Math;
using static System.Console;
public class subspline{
	public vector x, y, dely, a, b, c, d, e;
	public int n;
	public int order;
	
	public subspline(vector xs, vector ys, vector delys, int _order = 3){
		if(xs == null || ys == null) throw new ArgumentException("please not null");
		n = (int) xs.size;
		if(n != ys.size) throw new ArgumentException("x and y must be of the same dimension");
		x = xs.copy(); y = ys.copy(); 
		
		a = new double[n-1];
		b = new double[n-1];
		c = new double[n-1];
		d = new double[n-1];

		order = _order; //cubic or quartic

		//If first derivatives are given, the routine builds a subspline
		if(delys != null){
			if(n != delys.size) throw new ArgumentException("y' and y must be of the same dimension");
			dely = delys.copy();
			if(order == 3)
				cubic_sub();
			else if(order == 4)
				quartic_sub();
			}
		//If first derivatives aren't given, the routine builds a natural cubic spline 
		else
			cubic();
	}


	public void cubic_sub(){
	//Build a cubic spline, Si(x) = yi + bi(x-xi) + ci(x-xi)^2 + di(x-xi)^3
		if(dely == null) throw new Exception("The cubic subspline requires a set of derivatives, (x, y, y')");
		
		for(int i = 0; i < n-1; i++){
			double dXi = x[i+1]-x[i];
			double pi = (y[i+1]-y[i])/dXi;
			a[i] = y[i];
			b[i] = dely[i];
			c[i] = -(dely[i+1] + 2*dely[i] - 3*pi)/dXi;
			d[i] = (dely[i+1]+dely[i]-2*pi)/dXi/dXi;
		}
	}

	public void quartic_sub(){
	//Build a quartic spline, to have a continuous second derivative
		if(dely == null) throw new Exception("The quartic subspline requires a set of derivatives, (x, y, y')");
		
		e = new double[n-1];
 		double pi;

 		e[0] = 0;
		for(int i = 0; i < n-1; i++){
			double dXi = x[i+1]-x[i];
			pi = (y[i+1]-y[i])/dXi;
			a[i] = y[i];
			b[i] = dely[i];
			d[i] = (dely[i] + dely[i+1] - 2*pi)/dXi/dXi;
			c[i] = (3*pi-2*dely[i]-dely[i+1])/dXi;
			if(i != 0) e[i] = (c[i-1]-c[i]+3*d[i-1]*(x[i]-x[i-1]) + e[i-1]*Pow(x[i]-x[i-1], 2))/(dXi*dXi);
		}

		//Do another round of recursion over e from e[n-1] = 0, and choosing the average
		double[] e_back = new double[n-1];
		e[n-2] = 0;
		for(int i = n-3; i>=0; i--){
			double dXi = x[i+1]-x[i];
			double dXi1 = x[i+2]-x[i+1];
			e_back[i] = (e_back[i+1]*dXi1*dXi1 + c[i+1] - c[i] - 3*d[i]*dXi)/dXi/dXi;
		}
		for(int i = 0; i<e.size; i++)
			e[i] = (e[i] + e_back[i])/2;
	}

	public void cubic(){
		int len = n;
		//Build a cubic spline, Si(x) = yi + bi(x-xi) + ci(x-xi)^2 + di(x-xi)^3																																																																																		vector D = new vector(len);
		vector Q = new vector(len);
		vector B = new vector(len);
		vector C = new vector(len); 
		vector D = new vector(len);

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

	public double evaluate(double z){
		int i = binsearch(x, z);
		if(order == 4){
			if(i == x.size-1) throw new Exception("Too close to end of interval");
			return a[i] + b[i]*(z-x[i]) + c[i]*Pow((z-x[i]),2) + d[i]*Pow((z-x[i]), 3) + e[i]*Pow((z-x[i]), 2)*Pow((z-x[i+1]),2);}
		return a[i] + b[i]*(z-x[i]) + c[i]*Pow((z-x[i]),2) + d[i]*Pow((z-x[i]), 3);
	}

	public double derivative(double z){
		int i = binsearch(x, z);
		if(order == 4){
			if(i == x.size-1) throw new Exception("Too close to end of interval");
			return b[i] + 2*c[i]*Pow((z-x[i]),1) + 3*d[i]*Pow((z-x[i]), 2) + 2*e[i]*Pow((z-x[i]),2)*Pow((z-x[i+1]),1) + 2*e[i]*Pow((z-x[i]),1)*Pow((z-x[i+1]),2);}
		return b[i] + 2*c[i]*(z-x[i]) + 3*d[i]*Pow(z-x[i], 2);
	}

	public double derivative2(double z){
		int i = binsearch(x, z);
		if(order == 4){
			if(i == x.size-1) throw new Exception("Too close to end of interval");
			return 2*c[i] + 6*d[i]*(z-x[i]) + 2*e[i]*(Pow((z-x[i]), 2) + Pow((z-x[i+1]),2) + 4*(z-x[i])*(z-x[i+1]));}
		return 2*c[i] + 6*d[i]*(z-x[i]);
	}

	//definite integral from A to B
	public double integrate(double A, double B){
		if(A>B) return -integrate(B, A);
		int j = binsearch(x, A);
		int k = binsearch(x, B);

		double sum = 0; double dx;

		dx = A-x[j];
		sum -= a[j]*dx + b[j]/2*dx*dx + c[j]/3*dx*dx*dx + d[j]/4*dx*dx*dx*dx;
		
		for(int i = j; i<k; i++){
			dx = x[i+1]-x[i];
			sum += a[i]*dx + b[i]/2*dx*dx + c[i]/3*dx*dx*dx + d[i]/4*dx*dx*dx*dx;
		}
		dx = B-x[k];
		sum += a[k]*dx + b[k]/2*dx*dx + c[k]/3*dx*dx*dx + d[k]/4*dx*dx*dx*dx;
		return sum;
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
