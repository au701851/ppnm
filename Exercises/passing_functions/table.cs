using static System.Console;
using System;

public static class table{
public static void make_table(Func<double, double> f, double a, double b, double dx){
	// Takes a function transforming one double into another...
	// ... a starting pint a and an ending point b, and step size dx.
	for(double x = a; x <= b; x+=dx){
		//Iteterates between a and b with steps of size dx
		WriteLine($"{x}		{f(x)}");
		} 
}



}
