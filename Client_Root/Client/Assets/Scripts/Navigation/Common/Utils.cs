//------------------------------------------------------------------------
//
//  Name: utils.h
//
//  Desc: misc utility functions and constants
//
//  Author: Mat Buckland (fup@ai-junkie.com)
//
//------------------------------------------------------------------------
using System;

public static class Utils
{
	static Random rand = new Random();

	//returns a random double between zero and 1
	static double RandFloat()      {return rand.NextDouble();}

	public static double RandInRange(double x, double y)
	{
		return x + RandFloat()*(y-x);
	}
}