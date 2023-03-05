using System; 
using System.Threading;
using static System.Math;
using static System.Console;
class main{
//Inner class
public class data {
	public int a, b; public double sum;
}

public static void harm(object obj){
	data local = (data) obj;
	local.sum = 0; //initialise sum
	for(int i = local.a; i<local.b; i++){
		local.sum += 1.0/i; //Remember the .0 to mark the result as a double
	}
}

public static void Main(string[] args){
	int nterms = (int) 1e8, nthreads = 1; //default values
	foreach(var arg in args){
		var words = arg.Split(':');
		if(words[0] == "-threads") nthreads = (int) float.Parse(words[1]); //In arguments we interpret the number after -threads to be the desired number of threads
		if(words[0] == "-terms") nterms = (int) float.Parse(words[1]);
		//Its easer to pass floats as arguments than ints, so we transform them in the program instead. 
	}
	WriteLine($"nthreads = {nthreads}, nterms = {nterms}");

	data[] x = new data[nthreads]; //Prepare to split data over threads
	var threads = new Thread[nthreads]; //Prepare to create nthreads
	
	for(int i = 0; i<nthreads; i++){
		x[i] = new data();
		x[i].a = 1 + nterms/nthreads*i;
		x[i].b = 1 + nterms/nthreads*(i+1);
		WriteLine($"{x[i].a} {x[i].b}");
		// Splits the sum into even parts 
		
		threads[i] = new Thread(harm);
		threads[i].Start(x[i]);
	}

	//join the threads
	for(int i = 0; i<nthreads; i++) threads[i].Join();

	//Adding up all the parts
	double sum = 0;
	for(int i = 0; i<nthreads; i++) sum+=x[i].sum; 	

	WriteLine($"The sum from 1 to {nterms} of 1/n is {sum}");
}
}
