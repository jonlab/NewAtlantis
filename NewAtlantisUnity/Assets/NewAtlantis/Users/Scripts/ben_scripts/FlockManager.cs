using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FlockManager : MonoBehaviour {

	public List<SteeringVehicle> flock;

	void Start () {
	}

	public void AddVehicle (SteeringVehicle v)
	{
		flock.Add(v);
	}

	void Update () {
	
	}
}
