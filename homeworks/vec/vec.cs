using static System.Console;
using static System.Math;

public class vec{
	// Coordinates
	public double x,y,z;

	// A constructor with no components gives the zero-vector
	public vec(){
		x = y = z = 0; 
	}	

	// Coordinates can be handed to the constructor
	public vec(double a, double b, double c){
		x = a;
		y = b;
		z = c;
	}

	//Operators, teaches the mathematical operators how to work with our vec class
	// Multiplication by scalar:
	public static vec operator*(vec v, double c)
		{return new vec(c*v.x, c*v.y, c*v.z);}
	public static vec operator*(double c, vec v)
		{return v*c;} //scalarmultiplication is commutative
	// Adding vectors together:
	public static vec operator+(vec u, vec v)
		{return new vec(v.x+u.x, v.y+u.y, v.z+u.z);}
	public static vec operator-(vec u)
		{return new vec(-u.x, -u.y, -u.z);}
	public static vec operator-(vec u, vec v)
		{return u+(-v);}

	// Print methods to debug
	public void print(string s)
		{Write(s); WriteLine($"{x} {y} {z}");}
	public void print()
		{print("");}

	// Overriding the toString method
	public override string ToString()
		{return $"{x} {y} {z}";}

	//dot-product
	public static double dot(vec u, vec v)
		{return u.x*v.x + u.y*v.y + u.z*v.z;}
	public double dot(vec u)
		{return dot(this, u);}

	//norm
	public static double norm(vec v)
		{return Sqrt(dot(v, v));}
	public double norm()
		{return norm(this);}

	//cross-product
	public static vec cross(vec u, vec v)
		{return new vec(u.y*v.z-u.z*v.y, -u.x*v.z+u.z*v.x, u.x*v.y-u.y*v.x);}
	public vec cross(vec u)
		{return cross(this, u);}

	//comparing to doubles:
	public static bool approx(double a, double b, double tau = 1e-9, double eps = 1e-9){
		if(Abs(a-b)<tau) return true;
		if(Abs(a-b)/(Abs(a)+Abs(b)) < eps) return true;
		return false;}
	// using comparison of doubles to compare vectors
	public static bool approx(vec u, vec v){
		if(!approx(u.x, v.x)) return false;
		if(!approx(u.y, v.y)) return false;
		if(!approx(u.z, v.z)) return false;
		return true;
		}
	public bool approx(vec u) 
		{return approx(this, u);}

}
