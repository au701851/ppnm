using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Math;
using static System.Console;
class parallel{
	public static void Main(string[] args){
		int N = (int) 1e9; //default
		foreach(var arg in args){
			var words = arg.Split(':');
			if(words[0] == "-terms") N = (int) float.Parse(words[1]);
		}
		double sum = 0;
		Parallel.For(1, N+1, delegate(int i){sum += 1.0/i;}); //Wrong result, because the different threads are trying to acces sum at the same time
		WriteLine($"The harmonic sum calculated with Parallel.For {N} terms = {sum}. The sum is wrong because the variabel 'sum' is not local!");
	}
}
