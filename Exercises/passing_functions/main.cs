using static System.Math;
using static System.Console;

public class main{
	public static void Main(){
		for(int k=1; k<4; k+=1){
			Write($"\nTable of sin({k}x):\n");
			System.Func<double,double> sin = delegate(double x)	{return Sin(k*x);};
			table.make_table(sin, 0.0, PI, 0.5);
		}


	}
}
