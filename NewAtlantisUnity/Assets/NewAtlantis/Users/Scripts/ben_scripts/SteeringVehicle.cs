using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SteeringVehicle : MonoBehaviour {

	Vector3 velocity;
	Vector3 acceleration;

	public FlockManager flockManager;
	public Transform target;
	public Transform[] perches; 
	public float newTargetFrequency;	
	public float neighborhoodRadius = 100.0f;
	public float maxAcceleration=5.0f;
	public float maxSpeed=10.0f;

	public float steeringWeightSeparation = 0.8f;
	public float steeringWeightCohesion = 0.8f;
	public float steeringWeightAlignment = 0.5f;
	public float steeringWeightHoming = 5.0f;

	Arrival steerPerch;
	List<SteeringVehicle> neighborhood;

	List<SteeringBehavior> behaviors;

	public float fieldOfView=180.0f;

	WallAvoid wallAvoid;

	// Use this for initialization
	void Start () {

		//if (!NA.isClient())
		//local mode
		{
			velocity = new Vector3();
			//velocity = UnityEngine.Random.onUnitSphere * 2.0f;
			//transform.forward = velocity;


			acceleration = new Vector3();


			neighborhood = new List<SteeringVehicle>();

			behaviors = new List<SteeringBehavior>();

			behaviors.Add (new Separation (this,steeringWeightSeparation));
			behaviors.Add (new Cohesion (this,steeringWeightCohesion));
			behaviors.Add (new Alignment (this,steeringWeightAlignment));

			steerPerch = new Arrival(this,steeringWeightHoming,target,2.0f);
			PickNewPerch();

			behaviors.Add (steerPerch);
			if (flockManager != null)
			{
				flockManager.AddVehicle(this);
			}
		}
	}
	
	void Update () {

		//local mode
		//if (!NA.isClient())
		{

			if (MyUtils.RandomFrequency (newTargetFrequency))
			{
				PickNewPerch();
			}
			
			CalculateNeighborhood();
			Vector3 steeringForce = new Vector3();
			for (int i=0; i<behaviors.Count;i++)
			{
				Vector3 f = behaviors[i].Update();
				steeringForce += f;

			}

			acceleration = Vector3.ClampMagnitude(steeringForce, maxAcceleration);
			velocity += acceleration * Time.deltaTime;

			velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
			transform.position += velocity * Time.deltaTime;

			// rotate to follow velocity vector

			if (velocity != Vector3.zero)
				transform.forward = velocity.normalized;
		}

	}

	void CalculateNeighborhood()
	{
		if (flockManager)
		{
			neighborhood.Clear();
			for (int i=0;i<flockManager.flock.Count;i++)
			{
				SteeringVehicle vehicle = flockManager.flock[i];

				if (vehicle==this)
				{
					continue;
				}

				if (! inView (vehicle))
				{
					continue;
				}

				if (Vector3.Distance(transform.position, vehicle.transform.position) < neighborhoodRadius)
				{
					neighborhood.Add(flockManager.flock[i]);
				}
			}

		}

	}

	bool inView (SteeringVehicle v)
	{
		float angle = Quaternion.Angle(transform.rotation,v.transform.rotation);
		if (angle<fieldOfView)
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	public float GetNeighborhoodRadius()
	{
		return neighborhoodRadius;
	}

	public List<SteeringVehicle> GetNeighborhood ()
	{
		return neighborhood;
	}

	public Vector3 GetVelocity ()
	{
		return velocity;
	}

	void PickNewPerch ()
	{
		int r;
		if (perches.Length>0)
		{
			r=Random.Range(0,perches.Length + 1);
			if (r >= perches.Length)
			{
				// no perch! just flock
				target=null;
			}
			else
			{
				target=perches[r];
				steerPerch.SetTarget(target);
			}
		}

	}

}
