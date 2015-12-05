using UnityEngine;
using System.Collections;
//using System.Windows.Forms;
//using DLLTest;


public class GyzmoTransformScript : MonoBehaviour {

	public GameObject point;
	public GameObject myCamera;

	public GameObject[] gyzmos;
	GameObject[] saveGyzmos;
	//string[] values = { "Transform","Rotate","Scaling" };

	int[] translateValues = { 0 , 1 , 2 };
	int[] rotationValues = { 3 , 4 , 5 };
	int[] scalingValues = { 6 , 7 , 8 };

	public GameObject xRotation;
	public GameObject yRotation;
	public GameObject zRotation;


	// Use this for initialization
	void Start () {
	
		saveGyzmos = gyzmos;

		endGizmoAttachement ();

	
	}

	public void resetParentRotation(){

		for (int i = 3; i < 6; i++) {
			gyzmos [i].transform.parent = transform;

		}


	}

	void Update(){

		for (int i = 0; i < gyzmos.Length; i++) {

			if(gyzmos[i] == null){
			

				if(i==3){
					gyzmos[i] = GameObject.Instantiate(xRotation);


				}

				if(i==4){
					gyzmos[i] = GameObject.Instantiate(yRotation);


				}

				if(i==5){
					gyzmos[i] = GameObject.Instantiate(zRotation);
				

				}

				gyzmos[i].transform.parent = transform;
				gyzmos[i].transform.position = transform.position;



			}


		}

		/*
		if (Input.GetKeyDown (KeyCode.Space)) {
			startGizmoAttachment(point);

		}

		if (Input.GetKeyDown (KeyCode.S)) {
			endGizmoAttachement();
			
		}*/

	}



	public void startGizmoAttachment(GameObject o){

		// la camera 
		point = o;
	
		//point.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		transform.position = point.transform.position;
		myCamera.transform.position = point.transform.position - new Vector3(0,0,1)* 8;
		myCamera.transform.LookAt (point.transform.position);

		setActive (translateValues);

		//transform.GetChild (9).gameObject.SetActive (true);
		//transform.GetChild (10).gameObject.SetActive (true);
		transform.FindChild ("Modes").gameObject.SetActive (true);
		transform.FindChild ("Selector").gameObject.SetActive (true);


		//METTRE LES GYZMOS DE ROTATION EN ENFANT DU POINT
		gyzmos [3].transform.parent = point.transform;
		gyzmos [4].transform.parent = point.transform;
		gyzmos [5].transform.parent = point.transform;



	}

	public void endGizmoAttachement(){

		// RECUPERER LES ROTATIONS VALUES DANS LE GIZMO

		// DESACTIVER TOUS LES ENFANTS DU GIZMO

		gyzmos [3].transform.parent = transform;
		gyzmos [4].transform.parent = transform;
		gyzmos [5].transform.parent = transform;


		for(int i = 0 ; i < transform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(false);

		}


	}




	void setActive(int[] valuesToActive){

		for (int i = 0; i < gyzmos.Length; i++)	gyzmos[i].SetActive(false);
		for (int i = 0; i < valuesToActive.Length; i++) gyzmos[valuesToActive[i]].SetActive(true);
	}

	public void setActiveMode(int mode){
		int[] modeArg = new int[0];
		if (mode == 0)
			modeArg = translateValues;
		if (mode == 1)
			modeArg = rotationValues;
		if (mode == 2)
			modeArg = scalingValues;

		setActive (modeArg);

	}




	public GameObject[] returnRotationEntities(){

		GameObject[] a = new GameObject[3];
		a [0] = gyzmos [3];
		a [1] = gyzmos [4];
		a [2] = gyzmos [5];

		return a;
	}


}
