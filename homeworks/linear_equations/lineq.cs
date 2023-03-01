using System;
public class lineq{
	//Perform QR decomposition by Gram Schmidt orthogonalization
	public static void QRGSdecomp(ref matrix A, ref matrix R){
		if(A.size1 < A.size2){ 
			//If A has more coloumns than rows
			throw new ArgumentException("Matrix A must be tall");
		}
		if(R.size1 != A.size2 | R.size2 != A.size2){
			throw new ArgumentException("Matrix R must be square with size equal to number of colomns in A");
		}
		matrix Q = A.copy();
		for(int i = 0; i<A.size2; i++){
		//Iterating over the coloumns of A
			R[i,i] = Q[i].norm(); //The diagonal elements of R are the norms of the vectors of A
			Q[i]/=R[i,i]; //The first vector of Q is normalized
			for(int j = i+1; j<A.size2; j++){
				//Iterating over the rest of the row
				R[i,j] = Q[i].dot(Q[j]); //Projecting coloumn
				Q[j] -= Q[i]*R[i,j]; 
				}
			}
		A = Q;
	}
	public static vector QRGSsolve(matrix Q, matrix R, vector b){
		if(R.size1 != R.size2)
			throw new ArgumentException("R must be square");
		//Solving the matrix system QRx = b is equivalent to solving...
		//... Rx = Q^T b, because of the special properties of Q.
		//Since R is upper triangular, the above system can be solved with back-substitution.
		vector x = Q.transpose()*b;
		for(int i = x.size-1; i>=0; i--){ 
		//The usual for-loop, just backwards 
			double sum = 0;
			for(int k = i+1; k<R.size2; k++)
				sum += R[i, k]*x[k];
			x[i] = 1/R[i,i]*(x[i]-sum);
		}
		return x;
	}
	public static matrix QRGSinverse(matrix Q, matrix R){
		//Calculates the inverse of A = QR.
		//This must be done by solving linear equations.
		if(R.size1 != R.size2) 
			throw new ArgumentException("R must be square");
		matrix inverse = new matrix(R.size1, R.size2);
		for(int i = 0; i<R.size1; i++){
			vector ei = new vector(R.size1); ei[i] = 1; //creating unit vectors
			inverse[i] = QRGSsolve(Q, R, ei); //solving linear equation
		}
		return inverse;
	}

}
