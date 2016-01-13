using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringBehavior  {

	protected Transform transform;		// reference to the transform of the gameobject I'm attached to 
	protected float weight;

	protected 	SteeringVehicle vehicle;

	public SteeringBehavior (SteeringVehicle v, float w)
	{
		vehicle = v;
		transform = vehicle.transform;
		weight=w;
	}

	public virtual Vector3 Update (  )
	{
		Vector3 v = new Vector3();
		return v;
	}
}


public class Arrival : SteeringBehavior {
	Transform target;
	float slowing_distance;
	float arrival_threshold;

	public Arrival (SteeringVehicle v, float weight, Transform t, float s): base (v,weight)
	{
		target = t;
		slowing_distance = s;
	}
	public void SetTarget (Transform t)
	{
		target=t;
	}
	public override Vector3 Update()
	{

		if (target != null)
		{
			Vector3 desired_velocity = new Vector3();

			Vector3 target_offset = target.position - transform.position; 
			float distance = target_offset.magnitude;
			float ramped_speed = vehicle.maxSpeed * (distance / slowing_distance);
			float clipped_speed = Mathf.Min(ramped_speed, vehicle.maxSpeed);
			if (distance != 0f )	
				desired_velocity = (clipped_speed / distance ) * target_offset;

			Vector3 steering  = desired_velocity - vehicle.GetVelocity();
			return steering * weight;
		}
		else
		{
			return Vector3.zero;

		}
	}
}

// loop through all in neighborhood, calculate distance, apply repulsion force  away with 1/r squared with radius

public class Separation : SteeringBehavior {

	public Separation (SteeringVehicle v, float weight): base (v,weight)
	{

	}

	public override Vector3 Update ( )
	{
		List<SteeringVehicle> neighborhoodVehicles = vehicle.GetNeighborhood();
		float neighborhoodRadius = vehicle.GetNeighborhoodRadius();
		Vector3 desired_velocity = new Vector3();
		for (int i=0;i<neighborhoodVehicles.Count;i++)
		{
			Vector3 v=transform.position - neighborhoodVehicles[i].transform.position;
			// multiply by square of distance 

			if (v.sqrMagnitude>0)
			{
		  		v = v * (1 / v.sqrMagnitude);
			}
			else
			{
				v=Vector3.zero;

			}
	  		// accumulate forces
			desired_velocity+=v;
		}
		Vector3 steering = desired_velocity - vehicle.GetVelocity();
		return steering * weight;
	}
}

// apply a force in the direction of the average center of the neighborhood

public class Cohesion : SteeringBehavior {

	public Cohesion (SteeringVehicle v, float weight): base (v,weight)
	{

	}

	public override Vector3 Update ( )
	{
		List<SteeringVehicle> neighborhoodVehicles = vehicle.GetNeighborhood();
		float neighborhoodRadius = vehicle.GetNeighborhoodRadius();
		Vector3 f= new Vector3();
		Vector3 center = new Vector3(); 

		if (neighborhoodVehicles.Count > 0)
		{
			for (int i=0;i<neighborhoodVehicles.Count;i++)
			{
				center += neighborhoodVehicles[i].transform.position;
			}
			f = center / neighborhoodVehicles.Count;
			f = f-vehicle.GetVelocity();
		}

		return f * weight;
	}
}



public class Alignment : SteeringBehavior {
	public Alignment (SteeringVehicle v, float weight): base (v, weight)
	{

	}

	public override Vector3 Update (  )
	{
		List<SteeringVehicle> neighborhoodVehicles = vehicle.GetNeighborhood();
		float neighborhoodRadius = vehicle.GetNeighborhoodRadius();
		Vector3 f= new Vector3();
		Vector3 avgVelocity = new Vector3();

		if (neighborhoodVehicles.Count>0)
		{
			for (int i=0;i<neighborhoodVehicles.Count;i++)
			{
				avgVelocity += neighborhoodVehicles[i].GetVelocity();
			}
			avgVelocity = avgVelocity/neighborhoodVehicles.Count;

			f = avgVelocity - vehicle.GetVelocity();
		}
		return f * weight;

	}
}


public class WallAvoid : SteeringBehavior {
	public WallAvoid (SteeringVehicle v, float weight) : base (v,weight)
	{ }

	public override Vector3 Update ( )
	{
		Vector3 f= new Vector3();

		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.forward, out hit, 2.0f))
		{
			f=Vector3.Cross(hit.normal,transform.up);   
			// f = hit.normal;  this just makes it stop and flip around
			// f=transform.right;  kind of makes it swim around the edges but gets away 

			//f=Vector3.zero - transform.position;
			//f.Normalize();

			//f = f * ( 1/ hit.distance*hit.distance);

			f=f-vehicle.GetVelocity();

		}
		return f * weight;

	}
	}

