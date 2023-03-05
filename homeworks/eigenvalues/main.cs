using static System.Console;
using static System.Math;
using System;
public class main{

public static Random rnd = new Random(1);

public static void Main(){
WriteLine($"For part a. I had to program a Jacobi diagonalization method. \nThe program is in the file jacobi.cs, and the result of my testing it can be seen below:\n");
//Method for testing the functionality of the jacobi diagonalization
WriteLine("Testing jacobi.cyclic ...");
bool succes = true;

int n = 4; //moderate size of matrix
matrix A = new matrix(n, n);
//making A a random symmetric matrix:
for(int i = 0; i < A.size1; i++)
for(int j = i; j < A.size1; j++){
	double val = rnd.NextDouble();
	A[i, j] = val; A[j, i] = val; 
}
matrix A_org = A.copy(); //keeping a copy of the original A for future reference

//initiating V as the unit matrix
matrix V = matrix.id(n);


jacobi.cyclic(A, V);

WriteLine("Testing V^TAV = D ...");
if((V.transpose()*A_org*V).approx(A)) WriteLine("... passed");
else {WriteLine("... FAILED!"); succes = false;}

WriteLine("Testing VDV^T = A...");
if((V*A*(V.transpose())).approx(A_org)) WriteLine("... passed");
else {WriteLine("... FAILED!"); succes = false;}

WriteLine("Testing V^TV = 1...");
if((V.transpose()*V).approx(matrix.id(V.size1))) WriteLine("... passed");
else {WriteLine("... FAILED!"); succes = false;}

WriteLine("Testing VV^T = 1...");
if((V*(V.transpose())).approx(matrix.id(V.size1))) WriteLine("... passed");
else {WriteLine("... FAILED!"); succes = false;}

if(succes) WriteLine("All tests passed!");
else WriteLine("An error occured :(");


////////////////////////////////////////////////////////
WriteLine("\n\n\nIn part b, I used my jacobi routine to find permitted energies for the hydrogen atom, by solving an eigen-value problem.\n");

hydrogen_atom.test();

WriteLine("\n\nVarying dr from 0.05 to 0.3 and rmax from 1 to 20, I have collected som data in varying_dr.txt and varying_rmax.txt");
hydrogen_atom.varying_dr();
hydrogen_atom.varying_rmax();
}


}
