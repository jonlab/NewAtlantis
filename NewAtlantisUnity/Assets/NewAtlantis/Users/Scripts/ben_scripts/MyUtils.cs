using UnityEngine;
using System.Collections;

public class MyUtils {

	public static bool RandomFrequency ( float f ) 
	{
		float r = Random.value;
		float n = f * Time.deltaTime;
		if (r<n)
		{
			return true;
		}
		else
		{
			return false;
		}

	}


}
