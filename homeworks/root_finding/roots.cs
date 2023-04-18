using System;
using static System.Math;
using static System.Console;

public class roots{
public static vector newton(Func<vector, vector> f, vector x, double eps = 1e-2){
//Returns x0, that approximates the root of the equation f(x) = 0.
int dim = x.size; 
double dx;
vector newx, fx;
matrix J = new matrix(dim, dim);
matrix R = new matrix(dim, dim);
do{
	
	dx = x.norm()*Pow(2, -26); if(dx == 0) dx = Pow(2,-26); //In case the initial guess is \vec{0}
	fx = f(x);
	
	//Calculate jacobian matrix
	for(int k = 0; k<dim; k++){
		newx = x.copy();
		newx[k] += dx;
		J[k] = (f(newx) - fx)/dx;
	}
	lineq.QRGSdecomp(ref J, ref R);
	newx = lineq.QRGSsolve(J, R, -fx);
	
	double lambda = 1.0;
	while( ((f(x+lambda*newx)).norm() > (1.0-lambda/2)*fx.norm()) && lambda > 1.0/32 ) 
		{ lambda = lambda/2;}

	x = x + lambda*newx;

} while(f(x).norm() > eps);

return x;
}

}
