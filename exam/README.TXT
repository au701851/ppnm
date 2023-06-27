I have project no 18.
CUBIC SUBSPLINE FOR DATA WITH DERIVATIVES - A-LEVEL
I have implemented both a cubic subspline and a quartic spline, both equipped with methods of returning derivatives. 
For the cubic subspline, I even built a function to estimate a definite integral using the spline. 
Having done all of the 'extra' points, I believe this project to be at C-level (10 points).



APPROACH
Similar to the splines homework, I took an object-oriented approach, creating a class in subspline.cs, which could hold information about the tabulated 
values, and the optimised coefficients of the splines. 

I implemented, other than the build function, a function to return the spline evaluated at a certain point, along with its first and second derivative. 
I also implemented a function evaluating the definite integral.

In main.cs I test these splines by letting them interpolate a function with known derivatives and antiderivative. The results are in Out.txt. 
Finally, I visually compared the second derivative of the cubic and quartic spline, in Derivatives.svg, at a point where the interpolated function
curved sharply, so it is clear that only one of the derivatives are continuous. 



THEORETICAL CALCULATIONS
For the CUBIC SUBSPLINE
First I calculated formulas for all the coefficients in
S_i(x) = a_i + b_i(x-x_i) + c_i(x-x_i)^2 + d_i(x-x_i)^3

to satisfy boundary conditions
S_i(x_i) = y_i
S_i(x_(i+1)) = y_(i+1)

S_i'(x_i) = y'_i
S_i'(x_(i+1)) = y'_(i+1)


I found these coefficients to be
a_i = y_i

b_i = y'_i

c_i = (3p_i - y'_(i+1) - 2y'_i) / Dx_i

d_i = (y'_(i+1) + y'_i - 2p_i) / Dx_i^2


Where 	Dx_i = x_(i+1) - x_i, 
		p_i = (y_(i+1) - y_i) / Dx_i



For the QUARTIC SPLINE, I calculated the coefficients, demanding that
S_i''(x+i) = S_(i+1)''(x+i).

This gave me a recursive formula for e_i:

e_(i+1) = (c_i - c_(i+1) + 3*d_i Dx_i + e_i*Dx_i) / Dx_(i+1)^2.

I decided my values for e by first setting e_0 = 0, and finding the rest by recursion,
then by setting e_n = 0, and finding the rest of the values backwards. I finally averaged 
the two sets of values for my final e. 


