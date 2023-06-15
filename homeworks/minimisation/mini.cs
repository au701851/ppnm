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

public static vector simplex(Func<vector, double> f, vector[] xs, double tol = 1e-3){
	counter = 0; //number of tries, terminates when this number exceeds MAXTRIES
	int n = xs[0].size;
	if(xs.Length != xs[0].size + 1)
		throw new Exception("The downhill simplex method uses n+1 vectors, where n is the dimension of the problem");

	
	double maxdist; //max distance between corners of simplex - must be less than tol
	int min = 0; //index of lowest point of simplex
	
	do{
	maxdist = 0; //resets maximum distance (used for comparison with tol)
	counter++;
	if(counter > MAXCOUNTER) throw new Exception("Maximum iterations reached");
	
	//find highest and lowest point
	int max = 0; min = 0;
	double fmin = f(xs[0]); double fmax = f(xs[0]);
	
	for(int i = 1; i < xs.Length; i++){
		if((fmin == 0) || (f(xs[i]) < fmin)) {min = i; fmin = f(xs[i]);}
		if((fmax == 0) || (f(xs[i]) > fmax)) {max = i; fmax = f(xs[i]);}
		}
		
	vector phice = new vector(n);
	for(int i = 0; i< xs.Length; i++) 
		if(i != max) phice += 1.0/n*xs[i]; //calculate centroid


	//try reflection
	vector pre = phice + (phice - xs[max]); double fre = f(pre);
	if(fre < fmin){
		//try expansion
		vector pex = phice + 2*(phice - xs[max]);
		if(f(pex) < fre)
			xs[max] = pex; //accept expansion 
		else
			xs[max] = pre; //accept reflection
		}
	
	
	else{
		if(fre < fmax)
			xs[max] = pre; //accept reflection
		else{
			//try contraction
			vector pco = phice + 0.5*(xs[max] - phice); double fco = f(pco);
			if(fco < fmax)
				xs[max] = pco; //accept contraction
			else{
				//do reduction
				for(int i = 0; i<xs.Length; i++)
				if(i != min)
					xs[i] = 0.5*(xs[i] + xs[min]);
			} 
		}
	}
	for(int i = 0; i < xs.Length; i++)
	for(int j = 0; j < xs.Length; j++)
		if((i!=j) && ((xs[i]-xs[j]).norm() > maxdist)) maxdist = (xs[i]-xs[j]).norm(); 
	
	}while(maxdist > tol);//end of while loop

	return xs[min];
}

}
