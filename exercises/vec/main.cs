using System;
using static System.Console;
using static System.Math;
public class main{
	public static void Main(){
		vec o = new vec();
		o.print("A vector created with no components: ");

		vec a = new vec(1, 2, 3);
		vec b = new vec(-2, 4.5, 1);

		a.print("Vector a: ");
		b.print("Vector b: ");

		Write("-------------------------\n");

		(a+b).print("a + b = ");
		(a-b).print("a - b = ");
		(-b).print("-b = ");
		
		Write("-------------------------\n");

		(2.0*a).print("2*a = ");
		vec d = a*3.5;
		d.print("a*3.5 = ");

		Write("-------------------------\n");

		Write("Testing the to string method by printing a: ");
		Write(a);
		Write("\n");
		
		Write("-------------------------\n");
		double adotb = vec.dot(a, b);
		double normb = b.norm();
		Write($"a dot b = {adotb}\n");
		Write($"The norm of b is {normb}\n");
		
		Write("-------------------------\n");
		
		Write($"a cross b = {a.cross(b)}\n");
		Write($"b cross a = {b.cross(a)}\n");
		
		Write("-------------------------\n");

		bool test = true;
		Random rnd = new Random();
		int n = 10;
		vec[] vecs = new vec[n];
		for(int i = 0; i<n; i++)
			vecs[i] = new vec(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());
			
		Write("Testing addition by affirming Cauchy Schwarz ...\n");
			for(int i = 0; i<n-1; i++){
				vec v = vecs[i];
				vec u = vecs[i+1];
				test = test && (u.norm() + v.norm() >= (u+v).norm());}
		if(test) Write("...passed\n");
		else {Write("...FAILED\n");}

		test = true; 
		Write("Testing dot product less than |v||u| ...\n");
		for(int i = 0; i < n-1; i++){
			vec v = vecs[i];
			vec u = vecs[i+1];
			test = test && (u.norm()*v.norm() >= u.dot(v));
		}
		if(test) Write("...passed\n");
		else {Write("...FAILED\n");}
		
		test = true; 
		Write("Testing u dot (a*v + b*w) = a*(u dot v) + b*(u dot w)...\n");
		for(int i = 0; i < n-2; i++){
			vec v = vecs[i];
			vec u = vecs[i+1];
			vec w = vecs[i+2];
			double a_0 = rnd.NextDouble();
			double b_0 = rnd.NextDouble();
			vec t = a_0*v + b_0*w;
			test = test && (vec.approx(u.dot(t), a_0*u.dot(v)+b_0*u.dot(w)));
		}
		if(test) Write("...passed\n");
		else {Write("...FAILED\n");}
					
		
	}
}


