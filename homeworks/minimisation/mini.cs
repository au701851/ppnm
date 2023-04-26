using static System.Math;
using System;
using static System.Console;

public class mini{
public static int MAXCOUNTER = 100000;
public static int counter;

public static vector qnewton(Func<vector, double> f, vector start, double acc=0.01){
	counter = 0;

	int dim = start.size;
	matrix B = new matrix(dim, dim); //inverse Hessian matrix
	B.setid();
	
	vector x = start.copy();

	while(gradient(f, x).norm() > acc){
		if(counter>MAXCOUNTER) throw new Exception($"haaaaaaaaaaaalt! Max iterations ({MAXCOUNTER}) reached");
		counter ++;

		//Saving some values so I don't have to recalculate
		double fx = f(x);
		vector gradphi = gradient(f,x);	
		vector dx = -B*gradphi;
		

		
		//linesearch
		double lambda = 1.0;
		bool searching = true; int counter2 = 0;
		while(searching){
			if(counter2>MAXCOUNTER) throw new Exception("stopstopstop: linesearch failed");
			counter2 ++;
			if(f(x + lambda*dx) < fx) { //accept step
				x += lambda*dx;
				// u = dx - By,	y = grad(x+dx) - grad(x), 	dB = uu^T/(u^Ty)
				vector newgradphi = gradient(f, x);
				
				matrix u = new matrix(dim, 1);
				u[0] = dx - B*(newgradphi - gradphi);
				
				double denominator = (u.transpose()*(newgradphi-gradphi))[0];
				if(Abs(denominator) > Pow(2, -26))
					B += (u*u.transpose())/denominator;
				else B.setid();  //if the fraction diverges

				gradphi = newgradphi;
				searching = false;
				}
			lambda /= 2;
			if(lambda < Pow(2, -12)){
				x += lambda*dx; //step is unconditionally accepted
				B.setid();
				gradphi = gradient(f, x);
				searching = false;
				}
			}
		}
	return x;	
}

//Finds the gradient of f at point x 
public static vector gradient(Func<vector, double> f, vector x){
	vector grad = new vector(x.size);

	double fx = f(x);
	
	double dx = Pow(2, -26);
	for(int i = 0; i < x.size; i++){
		vector xnew = x.copy();
		xnew[i]+=dx;
		grad[i] = (f(xnew)-fx)/dx;
	}
	return grad;
}

}
