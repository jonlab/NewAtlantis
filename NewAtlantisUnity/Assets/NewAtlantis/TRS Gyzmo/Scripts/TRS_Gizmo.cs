using UnityEngine;
using System.Collections;

public class TRS_Gizmo : MonoBehaviour {

////////
	public GameObject gizmoCamera;
	public GameObject gizmoChild;

	public GameObject selector;

	GyzmoTransformScript gizmoScript;
	bool active = false;


    public GameObject viewerCamera;
	//public GameObject gizmoLight ; 


	// Use this for initialization
	void Start () {

	
	
	}
	
	// Update is called once per frame
	void Update () {
	/*
		if (Input.GetKeyDown (KeyCode.Space)) {
			GameObject exemple = GameObject.Find ("Point");

			if(exemple!=null){
			startTRSGizmo(exemple);
			active = true;
			}

		}
*/
	}



	public void startTRSGizmo(GameObject o){

		gizmoScript = (GyzmoTransformScript)gizmoChild.GetComponent (typeof(GyzmoTransformScript));

	
		//gizmoLight.SetActive (true);
		gizmoChild.SetActive (true);
		gizmoCamera.SetActive (true);
		gizmoScript.startGizmoAttachment (o);
		Transform mainCamera = transform.FindChild ("Main Camera");

		ScaleCamera scale = (ScaleCamera)mainCamera.GetComponent (typeof(ScaleCamera));
		RotateCamera rotate = (RotateCamera)mainCamera.GetComponent (typeof(RotateCamera));
		rotate.setPoint (o);

		LinkGyzmoPositionToPointPosition link = (LinkGyzmoPositionToPointPosition)gizmoChild.GetComponent (typeof(LinkGyzmoPositionToPointPosition));
		link.SetPoint (o);


		SelectorScript selectorScript = (SelectorScript)selector.GetComponent (typeof(SelectorScript));
		selectorScript.setPoint (o);


		scale.SetFocusPoint (o);

	



		active = true; //JT

        gizmoCamera.transform.position = viewerCamera.transform.position;
        gizmoCamera.transform.rotation = viewerCamera.transform.rotation;


	}

	public void stopTRSGizmo(){
	
		gizmoScript.resetParentRotation ();

		//gizmoLight.SetActive (false);
		gizmoChild.SetActive (false);
		gizmoCamera.SetActive (false);

	

		active = false;

	}

	public bool isActive(){
		return active;
	}

    public GyzmoTransformScript returnGyzmoTransform()
    {
        GameObject gyzmo = transform.Find("Gyzmo").gameObject;
        GyzmoTransformScript gyzmoTransform = gyzmo.GetComponent<GyzmoTransformScript>();
        return gyzmoTransform;
    }
}
