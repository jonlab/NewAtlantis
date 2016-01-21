using UnityEngine;
using System.Collections;

	public class GenaLib
	{

	void Start() {
		}
	/* ICHING (): chooses one or two of the 64 hexagrams of the iChing through the traditional choice process, 
	i.e. a roll of three coins for each of the six lines of the hexagram. 0 for yin and 1 for yang; 2 for old yang
	and 3 for old yin.  If any old yin or yang are chosen, the hexagram is said to be "moving" and the functions returns 1.
	Hence, the first hexagram is unstable and its number is placed into the public integer variable "hexagram1."
	Then, the old in and yang are converted to new, and the new number of a subsequent hexagram is put into 
	public variable "hexagram2".  If the first hexagram contained no old yin or yang, it is said to be "not moving" 
	or stable and the function returns 0 generating no second hexachord.  The variable hexagram2 then contains 0.

	Calling sequence: "yourint" = iChing (), where state is an integer resulting in 0 or 1, and the hexagram numbers are found
	in the public interger variables "hex1" and "hex2."  */ 
		
	public int[] iChing ()
		{
		float flip;
		int HorT, stable, unstable;
		int[] hex = new int[2];
		int[] yinyang = new int[6], yinyang1 = new int[6];
		int[] twoExp = new int[6] { 1, 2, 4, 8, 16, 32};
		int[] chart = new int[64] {2,24,7,19,15,36,46,11,16,51,40,54,62,55,32,34,8,3,29,60,
			39,63,48,5,45,17,47,58,31,49,28,43,23,27,4,41,52,22,18,16,35,21,64,38,56,30,50,14,20,
			42,59,61,53,37,57,9,12,25,6,10,33,13,44,1};
		stable = 0;
		unstable = 0;

		for (int i = 0; i < 6; i++) 
				{
					flip = (Random.value) * 4;
					HorT = (int)flip;
					yinyang [i] = HorT;
					yinyang1 [i] = HorT;

					if (yinyang [i] > 1) // construct the hexagram (from bottom up)
						{
							yinyang [i] = HorT - 2;
							yinyang1 [i] = Mathf.Abs (HorT - 3);
						}
					stable = stable + (yinyang [i] * twoExp [i]);
					unstable = unstable + (yinyang1 [i] * twoExp [i]);
				}

			if (unstable == stable) {	// if not moving
				hex[0] = chart[stable];
				hex[1] = 0;
			} else {
				hex[0] = chart[stable];
				hex[1] = chart[unstable];
				}
		return hex;
	}
	/*  Ranf() - RANDOM NUMBER GENERATOR - returns a random number between
		“minrange” and “maxrange". */
	public int Ranf(int minrange, int maxrange)
	{
	int num, range, offset;
	float rand;
		offset = minrange - 0;
		range = (maxrange - minrange) + 1;
		rand =  (Random.value) * range;
		num = (int) rand + offset;
	return num;
	}

	/* 	Function ZIPF: P. Gena,1973, rev. 1991.
	 Returns an integer between 0 and num (max 30) -1, according to Zipf’s Law, where
	 the probablilty of each successive element is equal to the inverse of its number.
	i.e. probability of 1st element is unity, 2 is 1/2, 3 is 1/3, etc.
	Calling sequence example: x = zipf(max), where the result is placed in x	 */
	
	public int Zipf(int num)
	{
		int[] lim = new int[30];
		int i, numm, topnum = 0;
		float[] per = new float[30];

		if (num > 30) num = 30;  // limit num to 30
			for (i = 1; i <= num; i++) {  // build scale according to Zipf's law
			lim [i-1] = 0;
			per [i - 1] = 1.0f / i;
			lim [i - 1] = (int)(per [i - 1] * 624f) + topnum;
			topnum = lim [i-1];
				}
		numm = Ranf (1, topnum);	// generate a choice
		i = 0;
		while (numm > lim[i]) {		// find numm within its probable range
						i++;
				}
		return i;
	}

	/* MLROW: P. Gena, 11/73, SUNY, at Buffalo.
			Extraction of an element of a stored row (modeled after the 12-tone matrix).
				Input: row = row of integers; numbered 0 to length of row.
						lf = form of the row sought.
						lf = 0.  the original form.
						lf = 1.  the inverted form.
						lf = 2.  the retrograde form.
						lf = 3.  the retrograde inversion form.
					lt = the (0 to No. of row items -1) number transposition sought (in the matrix).
					li = the (0 to No. of row items -1) number of the item sought.
				RETURN: The item.  */
	public int Mlrow (int[] row, int lf, int lt, int li)
	{
		int n1, result = 0, r = 0;
		//int r = 0, lf = 1, lt = 3, li = 6;
		n1 = row.Length - li +1;
		switch(lf) {
		case 1:
			r = row[lt] + (row[li] - row[0]);
			break;
		case 2:
			r = row[lt] - (row[li] - row[0]);
			break;
		case 3:
			r = row[lt] + (row[n1] - row[0]);
			break;
		case 4:
			r = row[lt] - (row[n1] - row[0]);
			break;
		default:
			Debug.Log ("Invalid form of the row sought: " + lf);
				break;
		}
		if(r > row.Length)  r -= row.Length;
		if(r < 1)  r += row.Length;
		for (n1 = 0; n1 < row.Length - 1; ++n1) { 
			if (row [n1] == r) {
				result = row [n1];
				} 
			}
		return result;
}

	// Returns x! up to 20?
	public int Factorial (int x)
	{
		if( x<0){
			return -1;
		}else if( x==1 || x==0 )
		{
			return 1;
		}else {
			return x* Factorial(x-1);
		}
	}

	void Update () {
	}
}

