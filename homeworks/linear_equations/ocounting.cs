using static System.Console;
using System;

public class ocounting{
public static int n;
public static Random rnd = new Random(1);
public static void Main(string[] args){
	foreach(var arg in args){
		var words = arg.Split(':');
		if(words[0] == "-N")
			n = (int) double.Parse(words[1]);
	}
	matrix A = new matrix(n, n);
	matrix R = new matrix(n, n);
	for(int i = 0; i<n; i++)
	for(int j = 0; j<n; j++) A[i, j] = rnd.NextDouble();
	lineq.QRGSdecomp(ref A, ref R);
}
}
