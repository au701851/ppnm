using static System.Console;
using static System.Math;
using System.IO;

public class hydrogen_atom{
public static bool testing = false;

public static void test(){
	double rmax = 10; //Bohr radii
	double dr = 0.3; //step length

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
		WriteLine($"I expect f to be given by r*psi100, and can as such write out the expected values of f. For the first three elements, these are:");
		WriteLine($"Exact result		Numerical result");
		for(int i = 0; i < 3; i++){
			double exp_fi = r[i]*1.0/Sqrt(PI)*Exp(-r[i]);
			WriteLine($"{exp_fi}	{V[i, 0]}"); 
			}
		WriteLine($"These results are evidently a factor 2 off... Hmmm\n\n");
	}

	double lowest_energy = H[0,0];
	return lowest_energy;
}
}
