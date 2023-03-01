using System;
using static System.Console;
public class main{
public static Random rnd = new Random(1);

public static void Main(){
	testQRGSdecomp();
	testQRGSsolve();
	testQRGSinverse();
}

public static void testQRGSdecomp(){
	WriteLine("Now testing QRGSdecomp...");
	bool passed = true;
	int n = 6; //rows
	int m = 4; //coloumns
	matrix A = new matrix(n, m);	
	for(int i = 0; i<n; i++)
		for(int j = 0; j<m; j++) A[i,j] = rnd.NextDouble();
	matrix Q = A.copy();

	matrix R = new matrix(m, m);
	lineq.QRGSdecomp(ref Q, ref R);

    ////////////////////////////////////////////////////////////////
	WriteLine("Affirming QR = A...");
	if(A.approx(Q*R)) WriteLine("...passed");
	else {WriteLine("...FAILED!"); passed = false;}

	WriteLine("Affirming R is upper triangular...");
	bool ut = true;
	for(int i = 0; i<R.size2; i++) {//iterating over rows
		if(matrix.approx(R[i,i], 0)) ut = false; //Checking diagonal elements to be non-zero
		for(int j = 0; j<i; j++)
			if(!matrix.approx(R[i,j], 0)) ut = false; //checking that the first i elements of the row are 0
	}
	WriteLine($"...{ut ? "passed" : "FAILED"}");
	passed = passed & ut;

	WriteLine("Affirming Q^T Q = 1...");
	if((Q.transpose()*Q).approx(matrix.id(Q.size2))) WriteLine("...passed");
	else {WriteLine("...FAILED!"); passed = false;}

	////////////////////////////////////////////////////////////////
	
	WriteLine($"{passed ? "All tests passed" : "An error was found"}");
		
}
public static void testQRGSsolve(){
	bool passed = true;
	int n = 5; //Size of matrix
	matrix A = new matrix(n, n);
	vector b = new vector(n);	
	for(int i = 0; i<n; i++){
		for(int j = 0; j<n; j++) A[i,j] = rnd.NextDouble();
		b[i] = rnd.NextDouble();
	}
	matrix Q = A.copy();
	matrix R = new matrix(n,n);
	lineq.QRGSdecomp(ref Q, ref R);
	vector sol = lineq.QRGSsolve(Q, R, b);
	////////////////////////////////////////////////////////
	WriteLine("\nNow testing QRGSsolve...");
	WriteLine("Testing Ax=b...");
	if((A*sol).approx(b)) WriteLine("...passed");
	else {WriteLine("...FAILED"); passed = false;}

	WriteLine($"{passed ? "All tests passed" : "An error was found"}");
	
}

public static void testQRGSinverse(){
	WriteLine("\nNow testing QRGSinverse...");
	bool passed = true;
	int n = 5; //Size of matrix
	matrix A = new matrix(n, n);
	for(int i = 0; i<n; i++)
		for(int j = 0; j<n; j++) A[i,j] = rnd.NextDouble();
	matrix Q = A.copy();
	matrix R = new matrix(n,n);
	lineq.QRGSdecomp(ref Q, ref R);
	matrix B = lineq.QRGSinverse(Q, R);
	
	/////////////////////////////////////////////////
	WriteLine("Testing A*A^(-1) = 1...");
	if((A*B).approx(matrix.id(n))) WriteLine("...passed");
	else {passed = false; WriteLine("...FAILED!");}

	WriteLine($"{passed ? "All tests passed" : "An error was found"}");
}
}


