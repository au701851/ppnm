using System;
using static System.Math;
using static System.Console;
public partial class lineq{
public static vector lsfit(Func<double, double>[] fs, vector x, vector y, vector dy){
if(x.size != y.size || y.size != dy.size)
	throw new ArgumentException("vectors x, y and dy must be of the same size");

//I must first create the matrix A with A_ik = fk(xi)/dyi
matrix A = new matrix(x.size, fs.Length);
for(int i = 0; i<A.size1; i++) {//rows
for(int k = 0; k<A.size2; k++)  //colomns 
	A[i, k] = fs[k](x[i])/dy[i];
}
for(int i = 0; i<y.size; i++)
	y[i] /= dy[i];


//Now my situation has become an ordinary least square problem, which I solve by solving Rc = Q^Ty
matrix R = new matrix(A.size2, A.size2);
QRGSdecomp(ref A, ref R);
return QRGSsolve(A, R, y);
}
}
