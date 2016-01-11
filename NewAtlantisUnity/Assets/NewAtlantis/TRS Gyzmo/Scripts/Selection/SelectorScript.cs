using UnityEngine;
using System.Collections;

public class SelectorScript : MonoBehaviour {
	public GameObject gyzmo;

	GameObject point;
	public GameObject myCamera;

	GameObject selected;
	Vector3 saveObjectPosition;

	Vector2 saveMousePosition,currentMousePosition;

	float gapValue =0;

	public float XtranslationSpeed = 0.05f;
	public float YtranslationSpeed = 0.05f;
	public float ZtranslationSpeed = 0.05f;

	public float XrotationSpeed = 0.2f;
	public float YrotationSpeed = 0.2f;
	public float ZrotationSpeed = 0.2f;

	public float scalingSpeed = 0.5f;

	Vector3 previousEuler;
	Vector3 currentEuler;

	Vector3 currentScaling;
	Vector3 currentTranslation;

	Quaternion saveRotation;

	TRS_Gizmo trs;
    ScaleCamera scaleCamera;

    int modeValue;

    public void setPoint(GameObject o){
		point = o;
	}

	void Start(){

		saveRotation = transform.rotation;
		trs = (TRS_Gizmo)transform.parent.transform.parent.GetComponent (typeof(TRS_Gizmo));

        GameObject gizmoCamera = transform.parent.transform.parent.Find("Main Camera").gameObject;
        scaleCamera = gizmoCamera.GetComponent<ScaleCamera>();

	}

	float getXgap(){
		return currentMousePosition.x - saveMousePosition.x ;

	}

	float getYgap(){
		return currentMousePosition.y - saveMousePosition.y ;
		
	}
	// Update is called once per frame
	void Update () {

        if (point == null)
        {
            trs.stopTRSGizmo();
            return;
        }

		if (selected != null && Input.GetMouseButton (0)) {

			ChangeMaterialOnMouseOver change = (ChangeMaterialOnMouseOver)selected.GetComponent(typeof(ChangeMaterialOnMouseOver));
			change.displaySelected();
			currentMousePosition = Input.mousePosition;

			GetDimensionType dimension = (GetDimensionType)selected.GetComponent(typeof(GetDimensionType));

			//print (selected.name);


			if(dimension == null){

				GyzmoTransformScript gyzmoScript = (GyzmoTransformScript)transform.parent.GetComponent(typeof(GyzmoTransformScript));


				if(selected.name == "TranslateMode")gyzmoScript.setActiveMode(0);
				if(selected.name == "RotateMode")gyzmoScript.setActiveMode(1);
				if(selected.name == "ScaleMode")gyzmoScript.setActiveMode(2);

							return;
			}

			int dimensionType = dimension.returnDimension();

			//print ("current Dimension Type is "+dimensionType);

			if(dimensionType == 0)gapValue = getXgap();
			if(dimensionType == 1)gapValue = getYgap();
			if(dimensionType == 2)gapValue = getXgap();

			GetMode mode = (GetMode)selected.GetComponent(typeof(GetMode));
			modeValue = mode.returnMode();

			//TRANSLATE MODE
			if(modeValue == 0){

				Vector3 rightDirection = gyzmo.transform.right * XtranslationSpeed * gapValue ;
				Vector3 upDirection = gyzmo.transform.up * YtranslationSpeed * gapValue ;
				Vector3 forwardDirection = gyzmo.transform.forward * ZtranslationSpeed * gapValue ;

				if(myCamera.transform.position.z > point.transform.position.z){
					rightDirection*=-1;
					//orwardDirection*=-1;
				}
                float distanceScale = scaleCamera.returnDistance() / 2;
                 distanceScale = Mathf.Clamp(distanceScale, 1, 4);
                //distanceScale = 1;

				if(dimensionType == 0)TranslateObject(rightDirection * distanceScale);
				if(dimensionType == 1)TranslateObject(upDirection * distanceScale);
				if(dimensionType == 2)TranslateObject(-forwardDirection * distanceScale);


			}

			//ROTATE MODE
			if(modeValue == 1){
				currentTranslation = new Vector3(0,0,0);
				gapValue = getXgap();
				Vector3 rightDirection = gyzmo.transform.right * XrotationSpeed * gapValue ;
				Vector3 upDirection = gyzmo.transform.up * YrotationSpeed * gapValue ;
				Vector3 forwardDirection = gyzmo.transform.forward * ZrotationSpeed * gapValue ;

				previousEuler = currentEuler;

				if(dimensionType == 0)currentEuler = forwardDirection;
				if(dimensionType == 1)currentEuler = rightDirection;
				if(dimensionType == 2)currentEuler = upDirection;

				RotateObject(currentEuler-previousEuler);
				
				
			}


			//SCALING MODE
			if(modeValue == 2){
				currentTranslation = new Vector3(0,0,0);
				gapValue = getXgap();
				//Vector3 rightDirection = gyzmo.transform.right * XrotationSpeed * gapValue ;
				//Vector3 upDirection = gyzmo.transform.up * YrotationSpeed * gapValue ;
				//Vector3 forwardDirection = gyzmo.transform.forward * ZrotationSpeed * gapValue ;
				
				previousEuler = currentEuler;
				
				if(dimensionType == 0)ScaleObject((float)gapValue/(float)10*(float)scalingSpeed);
				if(dimensionType == 1)ScaleObject((float)gapValue/(float)10*(float)scalingSpeed);
				if(dimensionType == 2)ScaleObject((float)gapValue/(float)10*(float)scalingSpeed);
				

				
			}


		
		}

		if (Input.GetMouseButtonUp (0) && trs.isActive() ) {


			saveRotation = point.transform.rotation;
		
            if(modeValue == 2)
            {
                GyzmoTransformScript gyzmo = trs.returnGyzmoTransform();
                //trs
                gyzmo.NormalizeScaling();
            }


			if(selected!=null){
				//myCamera.transform.Translate(currentTranslation);
			currentTranslation = new Vector3(0,0,0);
			//	myCamera.transform.LookAt(point.transform.position);

				unselect();


			}
		}
	}



	public void setSelected(GameObject o){
	
		//print ("Set Selected");
		saveObjectPosition = point.transform.position;
		//point.transform.rotation = saveRotation;  // AA
		saveRotation = point.transform.rotation;
		currentEuler = transform.rotation.eulerAngles;
		currentScaling = point.transform.localScale;

		selected = o;
		saveMousePosition = Input.mousePosition;
	}

	public void unselect(){
		ChangeMaterialOnMouseOver change = (ChangeMaterialOnMouseOver)selected.GetComponent(typeof(ChangeMaterialOnMouseOver));
		change.displayUnselected();
		selected = null;
	}

	public GameObject returnSelected(){
		return selected;
	}



	void TranslateObject(Vector3 v ){

		point.transform.position = saveObjectPosition + v;
		currentTranslation = v;

	}

	void RotateObject(Vector3 r){


		point.transform.Rotate (r);

	
	}

	void ScaleObject(float r){
		//print ("SCALE OBJECT");
		Vector3 scaleValue = currentScaling * r;
		if (scaleValue.magnitude < 0.05f)
			return;
		point.transform.localScale = scaleValue;

	}

}
