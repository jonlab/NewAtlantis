using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereDistribution
{
	//créé une distribution uniforme de points sur une sphère
	public Vector3[] positions = null;
	public SphereDistribution()
	{
		
	}
	
	public void CreateSpiral(int count)
	{
		positions = new Vector3[count];
		float N = count;
		float s  = 3.6f/Mathf.Sqrt(N);
		float dz = 2.0f/N;
		float _long = 0;
		float z    = 1f - dz/2f;
		for (int k = 0 ; k<N ; ++k)
		{
			float r    = Mathf.Sqrt(1f-z*z);
			positions[k] = new Vector3(Mathf.Cos(_long)*r, Mathf.Sin(_long)*r, z);
			z    = z - dz;
			_long = _long + s/r;
		}
	}
}