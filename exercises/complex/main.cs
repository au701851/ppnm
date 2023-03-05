using static cmath; //I use functions from out library of complex math
using static System.Console;
public class main{
public static void Main(){
	Write("In this exercise I have to do some math with the predifined cmath library, containing a class for complex numbers, and one for operations to be done on them.\n\n");
	complex sqrt_1 = sqrt(new complex(-1,0));
	complex E = new complex(System.Math.E, 0);
	Write($"sqrt(-1) = {sqrt_1}\n");
	Write($"sqrt(i) = {sqrt(I)}\n");
	Write($"e^i = {cmath.pow(E, I)}\n");
	Write($"e^(i*pi) = {cmath.pow(E, I*System.Math.PI)}\n");
	Write($"i^i = {cmath.pow(I, I)}\n");
	Write($"ln(i) = {log(I)}\n");
	Write($"sin(i*pi) = {sin(I*System.Math.PI)}\n");






}
}
