using static System.Console;
public class main{
public static void Main(){
	erf();
	gamma();
	lngamma();
	factorial();
}
public static void erf(){
	var outstream = new System.IO.StreamWriter("erf_fun.txt");
	for(double x = -5.0+1.0/64; x<=5; x+=1.0/64){
		outstream.WriteLine($"{x} {functions.erf(x)}");
	}
	outstream.Close();
}
public static void gamma(){
	var outstream = new System.IO.StreamWriter("gamma_fun.txt");
	for(double x = -5.0+1.0/64; x<=5; x+=1.0/64){
		outstream.WriteLine($"{x} {functions.gamma(x)}");
	}
	outstream.Close();
}
public static void factorial(){
	var outstream = new System.IO.StreamWriter("factorials.txt");
	for(int i = 0; i<=5; i++){
		outstream.WriteLine($"{i} {functions.factorial(i)}");
	}
	outstream.Close();
}
public static void lngamma(){
	var outstream = new System.IO.StreamWriter("lngamma_fun.txt");
	for(double x = 0.0+1.0/64; x<=5; x+=1.0/64){
		outstream.WriteLine($"{x} {functions.lngamma(x)}");
	}
	outstream.Close();
}
}
