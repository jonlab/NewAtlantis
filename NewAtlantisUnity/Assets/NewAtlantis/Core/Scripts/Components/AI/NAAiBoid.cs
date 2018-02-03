using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// adapted from 
// Boids - Flocking behavior simulation.
// Copyright (C) 2014 Keijiro Takahashi
public class NAAiBoid : NAAiBase 
{
	//parameters
	[Range(0.1f, 100.0f)]
	public float NeighborDistance = 1f;

	[Range(0f, 20.0f)]
	public float Velocity = 4;

	[Range(0.0f, 0.9f)]
	public float VelocityVariation = 0.9f;

	[Range(0.01f, 20.0f)]
	//[Range(0.01f, 0.99f)]
	public float RotationCoeff = 4f; //4f

	public GameObject attractor;

	public LayerMask SearchLayer;

	[Range(0f, 1.0f)]
	public float CohesionFactor = 0.5f;

	[Range(0f, 1.0f)]
	public float AlignmentFactor = 0.5f;

	[Range(0f, 1.0f)]
	public float SeparationFactor = 0.5f;

	// Options for animation playback.
	public float animationSpeedVariation = 0.2f;

	// Random seed.
	float noiseOffset;

	// parameters to stay within a certain radius of the attractor
	public bool limitDistanceFromAttractor=true;
	public float distanceFromAttractorLimit = 10.0f;

	// Caluculates the separation vector with a target.
	Vector3 GetSeparationVector(Transform target)
	{
		var diff = transform.position - target.transform.position;
		var diffLen = diff.magnitude;
		var scaler = Mathf.Clamp01(1.0f - diffLen / NeighborDistance);
		return diff * (scaler / diffLen);
	}

	void Start()
	{
		noiseOffset = Random.value * 10.0f;

		Animator animator = GetComponent<Animator>();
		if (animator)
			animator.speed = Random.Range(-1.0f, 1.0f) * animationSpeedVariation + 1.0f;
	}

	void Update()
	{
		if (!NA.isClient ())
		{
			Vector3 currentPosition = transform.position;
			Quaternion currentRotation = transform.rotation;

			// Current velocity randomized with noise.
			float noise = Mathf.PerlinNoise (Time.time, noiseOffset) * 2.0f - 1.0f;
			float velocity = Velocity * (1.0f + noise * VelocityVariation);

			// Initializes the vectors.
			Vector3 separation = Vector3.zero;
			Vector3 alignment;
			Vector3 cohesion;
			if (attractor != null)
			{
				alignment = attractor.transform.forward;
				cohesion = attractor.transform.position;
			} else
			{
				alignment = transform.forward;
				cohesion = transform.position;
			}

			// Looks up nearby boids.
			Collider[] nearbyBoids = Physics.OverlapSphere (currentPosition, NeighborDistance);

			// Accumulates the vectors.
			foreach (Collider boid in nearbyBoids)
			{
				if (boid.gameObject == gameObject)
					continue; //no self test
				Transform t = boid.transform;
				separation += GetSeparationVector (t);
				alignment += t.forward;
				cohesion += t.position;
			}

			float avg = 1.0f / nearbyBoids.Length;
			alignment *= avg;
			cohesion *= avg;
			cohesion = (cohesion - currentPosition).normalized;


			// Calculates a rotation from the vectors.
			Vector3 direction = separation * SeparationFactor + alignment * AlignmentFactor + cohesion * CohesionFactor;
			Quaternion rotation = Quaternion.FromToRotation (Vector3.forward, direction.normalized);

			// Applys the rotation with interpolation.
			if (rotation != currentRotation)
			{
				float ip = Mathf.Exp (-RotationCoeff * Time.deltaTime);
				//float ip = RotationCoeff*RotationCoeff;
				transform.rotation = Quaternion.Slerp (rotation, currentRotation, ip);
			}

			// Moves forward.

			Vector3 newPosition = currentPosition + transform.forward * (velocity * Time.deltaTime);

			if (attractor != null && limitDistanceFromAttractor) {
				if (Vector3.Distance (newPosition, attractor.transform.position) < distanceFromAttractorLimit) {
					transform.position = newPosition;
				}
				else {
					transform.LookAt(attractor.transform);	
				}
			
			}
		}
	}
}
