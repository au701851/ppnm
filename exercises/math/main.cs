using static System.Console;
using static System.Math;

class math{
	static void Main(){
		double sqrt2 = System.Math.Sqrt(2.0);
		System.Console.Write($"sqrt(2) = {sqrt2}\n");
		
		System.Console.Write($"sqrt(2)*sqrt(2) = {sqrt2*sqrt2}\n");

		double fifthroot2 = Pow(2, 0.2);
		Write($"2^(1/5) = {fifthroot2}\n");
		Write($"(2^(1/5))^(5) = {Pow(fifthroot2, 5)}\n");

		
		
		double epi = System.Math.Exp(System.Math.PI);
		System.Console.Write($"e^pi = {epi}\n");

		double pie = Pow(PI, E);
		Write($"pi^e = {pie}\n");

		Write("------------------------------\n");

		double gamma1 = sfuns.gamma(1);
		double gamma2 = sfuns.gamma(2);
		double gamma3 = sfuns.gamma(3);
		double gamma10 = sfuns.gamma(10);
		Write($"gamma(1) = {gamma1}\n");
		Write($"gamma(2) = {gamma2}\n");
		Write($"gamma(3) = {gamma3}\n");
		Write($"gamma(10) = {gamma10}\n");

		Write("------------------------------\n");

		double[] variables = {1, 2, 3, 10};
		foreach (double x in variables){
			double lngamma1 = sfuns.lngamma(x);
			double elngamma1 = Exp(lngamma1);
			Write($"From lngamma function: e^(ln(gamma({x}))) = {elngamma1}\n");
			}
	}
}

 
