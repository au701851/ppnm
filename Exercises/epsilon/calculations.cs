using static System.Console;
using static System.Math;

public static class calculations{
	public static void sumAsumB(){
		int n = (int) 1e6; ///large number
		double epsilon = Pow(2, -52);
		double tiny = epsilon/2;
		double sumA=0, sumB=0;

		sumA += 1; ///SumA starts at 1
		for(int i = 0; i<n; i++){
			sumA += tiny;
			sumB += tiny;
		}
		sumB += 1; /// SumB is added one

		Write($"SumA-1 = {sumA-1:e}, should be {n*tiny:e}\n");
		Write($"SumB-1 = {sumB-1:e}, should be {n*tiny:e}\n");
	}
	
	public static bool approx(double a, double b, double tau = 1e-9, double epsilon = 1e-9){
		if(Abs(a-b) < tau){
			return true;
		}
		if(Abs(a-b)/(Abs(a)+Abs(b)) < epsilon){
			return true;
			}
		return false;
	}
}
