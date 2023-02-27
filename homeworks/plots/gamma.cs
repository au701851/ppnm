using static System.Math;
using System;
 
//I write a partial class -- the error function will be in another file
public partial class functions{
	public static double gamma(double x){
		if(x<0) return PI/Sin(PI*x)/gamma(1-x);
		if(x<0) return gamma(x+1)/x;
		double lngamma = x*Log(x+1/(12*x-1/x/10))-x+Log(2*PI/x)/2;
		return Exp(lngamma);
	}
	public static int factorial(int x){
		if(x==0) return 1;
		else return x*factorial(x-1);
	}
	public static double lngamma(double x){
		if(x<=0) throw new ArgumentException("lngamma: x<=0");
		if(x<9) return lngamma(x+1)-Log(x);
		return x*Log(x+1/(12*x-1/x/10))-x+Log(2*PI/x)/2;
	}
}
