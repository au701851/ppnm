using System;
using static System.Math;
public class jacobi{

public static void timesJ(matrix A, int p, int q, double theta){
//multiplies matrix A with the Jacobi matrix, and saves result in A:
// A <- AJ 
double c = Cos(theta);
double s = Sin(theta); 

//Iterating over the rows of A
for(int i = 0; i<A.size1; i++){
	double aip = A[i,p]; //The original values of A are saved, but exist only within this loop -- which saves memory compared to keeping old values of A in an A.copy()
	double aiq = A[i,q];
	//Since the jacobi matrix is almost the identity matrix, only elements in coloumns p and q of A is changed.
	A[i, p] = c*aip - s*aiq;
	A[i, q] = s*aip + c*aiq;
}
}

public static void Jtimes(matrix A, int p, int q, double theta){
// A <- JA
double c = Cos(theta);
double s = Sin(theta);

//Iterating over the coloumns of A
for(int i = 0; i<A.size2; i++){
	double api = A[p, i]; //Only the pth and qth element of each coloumn is changed by the multiplication...
	double aqi = A[q, i];
	//... so only the pth and qth row are changed in A.
	A[p,i] = c*api + s*aqi;
	A[q,i] = c*aqi - s*api;
}
}

public static void cyclic(matrix A, matrix V){
//Performs cyclic sweeps of the jacobi algorithm
//The algorithm should be initiated with V = the identity matrix.
if((A.size1 != A.size2) | (V.size1 != V.size2)) throw new ArgumentException("Jacobi eigenvalue algorithm can only be performed on square matrices");
if(A.size1 != V.size1) throw new ArgumentException("V and A must be of the same size. V should be the identity matrix");
if(!(A.transpose()).approx(A)) throw new ArgumentException("A must be symmetric, to perform Jacobi eigenvalue algorithm.");

int n = A.size1; //size of matrix

bool changed;
//changed is true if the sweep changed the values of the diagonal elements. 
//The sweeps stop only when the diagonal elements are stable
do{
	changed = false;
	for(int p = 0; p<n-1; p++) //rows of A
	for(int q = p+1; q<n; q++){ //coloums of A avoiding p=q
		double apq = A[p, q];
		double app = A[p, p];
		double aqq = A[q, q];
		double theta = 0.5*Atan2(2*apq, aqq-app);
		//(Atan2 returns the angle whose tangent is the quotient of the two arguments)
		double c = Cos(theta);
		double s = Sin(theta);

		//Calculating the diagonal elements to see if another sweep will change them
		double new_app = c*c*app - 2*s*c*apq + s*s*aqq;
		double new_aqq = s*s*app + 2*s*c*apq + c*c*aqq;
		if(new_app!=app || new_aqq!=aqq) {
			changed = true;
			timesJ(A, p, q, theta);
			Jtimes(A, p, q, -theta); //since J^T = J(-theta)
			// A <- J^T*A*J
			timesJ(V, p, q, theta);
			//V is determined as V = I*J_1*J_2...
		}
	}
}while(changed); //keep doing sweeps until diagonal elements are stable 
}



}
