using System;
using static System.Console;
public class main{
	public static void Main(){
		//part a
		genlist<double[]> list = new genlist<double[]>();
		char [] delimiters = {' ', '\t'}; 
		//Tells the reader what delimiters to use when splitting the text
		var options = StringSplitOptions.RemoveEmptyEntries; //From System
		for(string line = ReadLine(); line != null; line = ReadLine()){
			var words = line.Split(delimiters, options); //Splits the text into words at chosen delimiters, and removes empty entries
			int n = words.Length; // for iteration
			double[] numbers = new double[n];
			for(int i = 0; i<n; i++){
				numbers[i] = double.Parse(words[i]); //interprets words as doubles one by one
			}
			list.add(numbers);
			}
		for(int i = 0; i<list.size; i++){
			//Iterating over all elements in the list
			var numbers = list[i];
			foreach(var number in numbers){
				Write($"{number : 0.00e+00; -0.00e+00} ");}
			WriteLine();
			}

		//part b
		Write("\nI attempt to remove the first element, and print again:\n");
		list.remove(list[0]);
		for(int i = 0; i<list.size; i++){
			//Iterating over all elements in the list
			var numbers = list[i];
			foreach(var number in numbers){
				Write($"{number : 0.00e+00; -0.00e+00} ");}
			WriteLine();
			}

		//part c
		Write("\nI have now created my list som it only extends if it needs to\n");
		genlist<int> testList = new genlist<int>();
		for(int i = 0; i<8; i++){
			testList.add(i);
		}
		Write("My testlist is: ");
		for(int i = 0; i<testList.size; i++) Write($"{testList[i]} ");
		Write($"\nIts capacity is {testList.capacity}\n");
		testList.add(9);
		Write($"After adding one more element, its capacity is {testList.capacity}\n");
		
	}
}

