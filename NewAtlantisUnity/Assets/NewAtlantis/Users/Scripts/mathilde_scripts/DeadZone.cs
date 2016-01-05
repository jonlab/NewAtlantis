using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {


	
	void OnTriggerEnter (Collider col)
	{
		if (!NA.isClient ()) {
			if (col.gameObject.name == "Ball") {
				GM.instance.LoseLife ();
			}
		}
	}
}