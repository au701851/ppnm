using static System.Console;
using static System.Math;
using System; 

public class main{

public static string filetab, fileinterp, fileB;

public static void Main(string[] args){
	foreach(string arg in args){
		var words = arg.Split(":");
		if(words[0] == "-fileTab") filetab = words[1];
		if(words[0] == "-fileInterp") fileinterp = words[1]; 
		if(words[0] == "-fileB") fileB = words[1];
	}

	WriteLine("--------------------------------------");
	WriteLine("----------------PART A----------------");
	partA();
	WriteLine("--------------------------------------");
	WriteLine("--------------------------------------");
	WriteLine("----------------PART B----------------");
	partB();
	WriteLine("--------------------------------------");
}
public static void partA(){
	WriteLine("For part A, I had to construct a simple artificial neural network, that could interpolate a function");

	//Create tabbed values for function, that I wish replicated
	Func<double, double> g = (double x) => Cos(5*x-1)*Exp(-x*x);

	int s = 64; //number of tabbed values
	vector X = new vector(s);
	vector Y = new vector(s);
	var outfile = new System.IO.StreamWriter(filetab);
	for(int i = 0; i<s; i++){
		X[i] = -1 + 2.0/s*i;
		Y[i] = g(X[i]);
		outfile.WriteLine($"{X[i]}	{Y[i]}");
	} 
	outfile.Close();

	//Create neural network
	ann myArtNetwork = new ann(3); 

	//Save interpolated values after one training
	myArtNetwork.train(X, Y); 
	outfile = new System.IO.StreamWriter(fileinterp.Replace(".data","1.data"));
	for(int i = 0; i<s-1; i++){
		double x = -1+2.0/s*(i+0.5);
		outfile.WriteLine($"{x}	{myArtNetwork.response(x)}");
	}
	outfile.Close();
	
	myArtNetwork.trainN(X, Y, 5);  
	outfile = new System.IO.StreamWriter(fileinterp.Replace(".data","2.data"));
	for(int i = 0; i<s-1; i++){
		double x = -1+2.0/s*(i+0.5);
		outfile.WriteLine($"{x}	{myArtNetwork.response(x)}");
	}
	outfile.Close();

	WriteLine("GraphA.svg shows how succesful this function was by plotting interpolated values of the function Cos(5x-1)*Exp(-x*x) after one and five trainings.");
}


public static void partB(){
	WriteLine("For part B I modified my code to also return first and second derivative, and the antiderivative.");
	WriteLine("To test my program, I first trained my network to approximate a simple function, f(x)=cos(x), so I could easily check if my code was correct.");


	
	int s = 64; //number of tabbed values
	double a = PI/2; //interval
	vector X = new vector(s);
	vector Y = new vector(s);
	Func<double, double> g = (double x) => Cos(x);
	for(int i = 0; i<s; i++){
		X[i] = -a + 2.0*a/s*i;
		Y[i] = g(X[i]);
	} 

	
	
	//Create neural network
	ann myArtNetwork = new ann(3); 
	myArtNetwork.trainN(X,Y,5);


	var outfile = new System.IO.StreamWriter(fileB);
	for(int i = 0; i<s-1; i++){
		double x = -a+2.0*a/s*(i+0.5);
		outfile.WriteLine($"{x}	{myArtNetwork.response(x)}	{myArtNetwork.del(x)}	{myArtNetwork.del2(x)}	{myArtNetwork.adel(x)}");
	}
	outfile.Close();
	
	
	WriteLine("The result of my work is in GraphB.svg.");
	WriteLine("My experience was that the first derivative was pretty well approximated, as long as you stayed clear of edges of the interval.");
	

}

}
