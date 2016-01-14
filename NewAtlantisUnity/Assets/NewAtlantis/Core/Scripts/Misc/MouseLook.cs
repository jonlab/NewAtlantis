using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

    //lissage
    private float cx = 0f;
    private float cy = 0f;

	void Update ()
	{
        
        GameObject trsGyzmo = GameObject.Find("TRS Gizmo");
        if(trsGyzmo!= null)
        {
            GameObject gizmoCamera = trsGyzmo.transform.Find("Main Camera").gameObject;
			if (gizmoCamera != null)
			{
            	if (gizmoCamera.active == true) 
					return;
			}
        }



        if (NAInput.GetControl(NAControl.NextTool) || NAInput.GetControl(NAControl.PreviousTool))
		{
			return;
		}
		//joystick
		/*
		float rotationJX = transform.localEulerAngles.y + Input.GetAxis("ViewX") * sensitivityX;
		
		rotationY += Input.GetAxis("ViewY") * sensitivityY * -1f;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		transform.localEulerAngles = new Vector3(-rotationY, rotationJX, 0);
		*/
		float JoySensitivityX = 2f * 60f * Time.deltaTime;
		float JoySensitivityY = 2f * 60f * Time.deltaTime;
		float jx = NAInput.GetAxis(NAControl.ViewHorizontal);
		float jy = NAInput.GetAxis(NAControl.ViewVertical);

		float x = 0.1f*jx+(jx*jx*jx)*0.9f;
		float y = 0.1f*jy+(jy*jy*jy)*0.9f;

        float k = NA.JoystickSmoothing;
        cx = cx*k+x*(1f-k);
        cy = cy*k+y*(1f-k);
		float rotationJX = transform.localEulerAngles.y + cx * JoySensitivityX;
		rotationY += cy * JoySensitivityY * -1f;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		transform.localEulerAngles = new Vector3(-rotationY, rotationJX, 0);




		if (!Input.GetMouseButton (0))
			return;
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}



	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}