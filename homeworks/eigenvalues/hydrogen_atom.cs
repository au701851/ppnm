using static System.Console;
using static System.Math;
using System.IO;

public class hydrogen_atom{
public static bool testing = false;
public static double RMAX, DR;

public static void Main(string[] args){
	testing = false;
	RMAX = 6;
	DR = 0.3; //default values
	foreach(var arg in args){
	var words = arg.Split(':');
	if(words[0] == "-rmax")
		RMAX = double.Parse(words[1]);
	if(words[0] == "-dr")
		DR = double.Parse(words[1]);
	if(words[0] == "-test")
		testing = bool.Parse(words[1]);}
	double eps = find_lowest_energy(RMAX, DR);
	WriteLine($"{RMAX} {DR} {eps}");
}	

public static void test(){
	double rmax = 10; //Bohr radii
	double dr = 0.2; //step length

	testing = true;
	find_lowest_energy(rmax, dr);
	testing = false; 
}

public static void varying_dr(){
	var outfile = new StreamWriter("varying_dr.txt");
	double rmax = 6;
	for(double dr = 1.5 + 1/32; dr>0.0; dr -= 1.0/32){
		double E_0 = find_lowest_energy(rmax, dr);
		outfile.WriteLine($"{dr} {E_0}");
	}
	outfile.Close();
}

public static void varying_rmax(){
	var outfile = new StreamWriter("varying_rmax.txt");
	double dr = 0.3;
	for(double rmax = 1.0; rmax < 15; rmax += 1.0/4){
		double E_0 = find_lowest_energy((double) rmax, dr);
		outfile.WriteLine($"{rmax} {E_0}");
	}
	outfile.Close();
}

public static double find_lowest_energy(double rmax, double dr){
	//Building Hamiltonian matrix
	int npoints = (int)(rmax/dr) - 1;

	vector r = new vector(npoints);
	for(int i = 0; i<npoints; i++) r[i] = dr*(i+1);

	matrix H = new matrix(npoints, npoints);
	for(int i = 0; i<npoints-1; i++){
		H[i,i] = -2;
		H[i, i+1] = 1;
		H[i+1, i] = 1; }
	H[npoints-1, npoints-1] = -2; //must be set outside the loop, or an indexOutOfBounds-exception would be cast at i+1
	H*=-0.5/dr/dr;
	//Now we have H = K (with K from the assignment)

	for(int i = 0; i<npoints; i++) H[i,i] += -1/r[i]; //H <- H+V

	//The energies of the system are determined as eigenvalues of H, which I can find using my jacobi diagonalization routine!
	matrix V = matrix.id(npoints);

	jacobi.cyclic(H, V); //H <- D

	if(testing){
		//The lowest energy is the first eigenvalue
		WriteLine($"Calculated lowest energy: {H[0,0]} \n(Should be -(E_h)/2 = -0.5 in units of E_h)");
		
		
		WriteLine($"I expect f to be given by r*psi100, and can as such write out the expected values of f. These are in the data files 'first eigenvector' and 'second eigenvector', and are plotted in 'Lowest eigenvectors'.");
		WriteLine($"I found these values to be off from their expected values, and realised that it had something to do with the normalisation:");
		WriteLine($"first eigenvector square norm: {V[0].norm()}, Second eigenvector square norm: {V[1].norm()}");
		WriteLine($"That means the found eigen vectors are normalised, whereas the square norm of the expected values should be <r^2> for the given state. I have used this factor to normalise the expected values.");
		

		var outfile = new System.IO.StreamWriter("first_eigenvector.data");
		var outfile2 = new System.IO.StreamWriter("second_eigenvector.data");
		outfile.WriteLine($"##Delta r		Exact result		Numerical result");
		outfile2.WriteLine($"##Delta r		Exact result		Numerical result");
		double theo_fi = 0;
		for(int i = 0; i < V.size1; i++){
			theo_fi = 2*r[i]*Exp(-r[i]);
			outfile.WriteLine($"{r[i]}	{theo_fi}	{V[i, 0]/Sqrt(dr)}");
			theo_fi = r[i]*1.0/Sqrt(2.0)*(1.0 - 1.0/2.0*r[i])*Exp(-r[i]/2);
			outfile2.WriteLine($"{r[i]}	{theo_fi}	{-V[i, 1]/Sqrt(dr)}");
			}
		outfile.Close();
		outfile2.Close();
	}

	double lowest_energy = H[0,0];
	return lowest_energy;
}
}
