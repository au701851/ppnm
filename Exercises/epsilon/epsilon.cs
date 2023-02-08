using static System.Console;
using static System.Math;

class epsilon{
	static void Main(){
		int i = 1;
		while(i+1>i){
			i++;
		}
		Write("I have calculated my max int to be = {0}\n", i);
		Write("The int.MaxValue is {0}\n", int.MaxValue);

		int j = 1;
		while(j-1<j){
			j--;
			}
		Write("I have calculated my min int to be = {0}\n", j);
		Write("The int.MinValue is {0}\n", int.MinValue);
		Write("-----------------------------------------");
	
		double x = 1;
		while(1+x!=1){
			x/=2; }					
		x*= 2;

		Write("The difference between 1.0 and the next double = {0}\n", x);
		Write("-----------------------------------------");
		

		float y = 1F;
		while((float)(1F+y)!=1F){
			y/=2F;}
		Write("The difference between 1.0 and the next float = {0}\n", y);
		Write("For reference we have 2^(-24) = {0}, and 2^(-52) = {1}\n", Pow(2, -24), Pow(2, -52));
		Write("-----------------------------------------");

		
	
	}
}

