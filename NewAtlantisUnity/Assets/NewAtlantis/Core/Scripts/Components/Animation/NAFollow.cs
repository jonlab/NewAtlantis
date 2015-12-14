using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class NAFollow: MonoBehaviour {
	
	/// VARIABLES DE DEPLACEMENT
	public Transform myTransform;
	public Transform target;
	public float moveSpeed = 3.0f;
	public float rotationSpeed = 3.0f; 
	public float range = 10.0f;
	public float range2 = 10.0f;
	public float stop = 0.0f;
	

	
	void Awake(){
		
		myTransform = transform;
		
	}
	
	
	// Use this for initialization
	void Start () {
		if (target != null)
						print ("ok");

		
	}
	
	

	
	
	void FixedUpdate(){
		
		
				float distance = Vector3.Distance (myTransform.position, target.position);
				if (distance <= range2 && distance >= range) {
			
						myTransform.rotation = Quaternion.Slerp (myTransform.rotation,
			                                         Quaternion.LookRotation (target.position - myTransform.position), rotationSpeed * Time.deltaTime);
				} else if (distance <= range && distance > stop) {
			
						//move towards the player // aller a l'objectif
						myTransform.rotation = Quaternion.Slerp (myTransform.rotation,
			                                         Quaternion.LookRotation (target.position - myTransform.position), 
			                                         rotationSpeed * Time.deltaTime);
					myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
				} else if (distance <= stop) {
			//move towards the player // aller a l'objectif
			myTransform.rotation = Quaternion.Slerp (myTransform.rotation,
			                                         Quaternion.LookRotation (target.position - myTransform.position), 
			                                         rotationSpeed * Time.deltaTime);
		
				}

		}
	
	
	
	
	
}
