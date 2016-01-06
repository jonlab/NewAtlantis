using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class RotateOnRightClick : MonoBehaviour {


	public float speed = 1;


	void Update () {

		if(Input.GetMouseButton(1))transform.Rotate(0,speed,0);

	}
}
