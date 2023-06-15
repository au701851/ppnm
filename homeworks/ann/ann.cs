using System;
using static System.Math;
using static System.Console;

public class ann{
	public int n; //Number of hidden neurons
	public Func<double, double> f = x => x*Exp(-x*x); //Activation function
	public Func<double, double> delf = x => -2*x*x*Exp(-x*x)+Exp(-x*x); //First derivative of activation function
	public Func<double, double> del2f = x => -4*x*Exp(-x*x)+4*x*x*x*Exp(-x*x)-2*x*Exp(-x*x); //Second derivative
	public Func<double, double> adelf = x => -0.5*Exp(-x*x); //antiderivative
	public vector p; //parameters, consisting of n*3 (n*(w+a+b))
	public double grad; //expression of how fitting the parameters are right now
	public Random rnd;

	public ann(int n){
		this.n = n;
		p = new vector(n*3);
		for(int i = 0; i<p.size; i++) p[i]=0.7; //to avoid divide by zero error
		grad = 100;

		rnd = new Random();
	}

	public double response(double x){
		double Fp = 0;
		for(int i = 0; i<n; i++){
			double w = p[3*i];
			double a = p[3*i+1];
			double b = p[3*i+2];
			Fp += w*f((x-a)/b);
		}
		return Fp;
	}

	//Returns first derivative -- assuming activation function to be gaussian
	public double del(double x){
		double delFp = 0;
		for(int i = 0; i<n; i++){
			double w = p[3*i];
			double a = p[3*i+1];
			double b = p[3*i+2];
			delFp += w/b*delf((x-a)/b); //remember to multiply with inner func derived
		}
		return delFp;
	}

	//Returns second derivative -- assuming activation function to be gaussian
	public double del2(double x){
		double del2Fp = 0;
		for(int i = 0; i<n; i++){
			double w = p[3*i];
			double a = p[3*i+1];
			double b = p[3*i+2];
			del2Fp += w/b/b*del2f((x-a)/b);
		}
		return del2Fp;
	}

	public double adel(double x){
		double adelFp = 0;
		for(int i = 0; i<n; i++){
			double w = p[3*i];
			double a = p[3*i+1];
			double b = p[3*i+2];
			adelFp += w*b*adelf((x-a)/b);
		}
		return adelFp;
	}
	
	public void train(vector x, vector y){
		vector pold = p.copy(); //saving old coordinates
		
		Func<vector, double> C = (vector p0) => {
			p = p0;
			double Cp = 0; 
			for(int i = 0; i < x.size; i++)
				Cp += Pow(response(x[i])-y[i], 2);
			return Cp/(x.size);
		};	

		//simplex is filled with random vectors, to see if they can approximate better
		vector[] simplex = new vector[3*n+1];
		vector prand; 
		for(int j = 0; j <= 3*n; j++){		
			prand = new vector(3*n);
			for(int i = 0; i<prand.size; i++) {double r = rnd.NextDouble(); prand[i]=r;}
			simplex[j] = prand;}
			
		//Best result from random simplex
		prand = mini.simplex(C, simplex, 1e-9);

		//comparison with old results
		if(mini.gradient(C, prand).norm() > grad)
			p = pold; //return to old values.
		else{ p = prand; grad = mini.gradient(C, prand).norm();}
		
	}

	//Train N times
	public void trainN(vector x, vector y, int N){
		for(int i = 0; i<N; i++){
			rnd = new Random(17*i);
			train(x,y);}
	}
	


}
