using UnityEngine;
using System.Collections;
//camera settings x 36   7    -43
public class NA_TrackerCamera : NACamera
{

    public Transform myTransform;
    public Transform target;
    float moveSpeed = 10.0f;
    float rotationSpeed = 3.0f;
    float range = 1000.0f;
    float range2 = 1000.0f;
    float stop = 10.0f;
    float stop2 = 8.0f;

    bool backwardSolution = true;

    //public bool autoInit = true;

    public GameObject backwardPoint;

   //public GameObject mainViewer, mainViewerCamera;

    GameObject addBackwardPoint;

   // public GameObject cameraViewer;

    NA_CameraBackwardSolution backwardSolutionScript;
    //   public NA_CameraBackwardSolution backwardScript;

   
  
    void Awake()
    {

      //  myTransform = transform;
      //myTransform = 

    }
    // Use this for initialization
    void Start()
    {
       AssignObjects();
    }


    void OnEnable()
    {
        //if (Time.frameCount < 10) return;
        // target = myTransform;
        //   Debug.LogError("return , main viewer camera is null");
        GameObject mainViewerCamera = myTransform.gameObject;


        addBackwardPoint = GameObject.Instantiate(backwardPoint);
       // if (mainViewerCamera == null) Debug.LogError("main viewer camera is null");

        addBackwardPoint.transform.parent = mainViewerCamera.transform;
        addBackwardPoint.transform.rotation = mainViewerCamera.transform.rotation;
        addBackwardPoint.transform.position = mainViewerCamera.transform.position;

        Rigidbody rg = mainViewerCamera.AddComponent<Rigidbody>();
        rg.useGravity = false;
        rg.isKinematic = true;

        backwardSolutionScript = addBackwardPoint.GetComponent<NA_CameraBackwardSolution>();

        mainViewerCamera.AddComponent<NA_CameraBackwardSolution>();

        // parentNode = GameObject.Instantiate(aParentNode);
        mainViewerCamera.transform.parent = null;

    }

    void OnDisable()
    {
        Destroy(addBackwardPoint);
        Destroy(myTransform.GetComponent<Rigidbody>());
        Destroy(myTransform.GetComponent<NA_CameraBackwardSolution>());
        myTransform.parent = target.transform;

      //  Destroy(parentNode);
    }

    void AssignObjects()
    {
        target = GameObject.Find("Main Viewer").transform;
        if (target == null) Debug.LogError("Main Viewer not Found.");



        myTransform = target.Find("Camera");

        if(myTransform == null) Debug.LogError("Main Viewer Camera not Found.");


        // myTransform = 
        //target = mainViewer.transform;
        //myTransform = mainViewerCamera.transform;

    }

    void Update()
    {


        if (backwardSolutionScript != null) backwardSolution = backwardSolutionScript.returnBackwardSolution();
    
        //rotate to look at the player
        float distance = Vector3.Distance(myTransform.position, target.position);
       

       if (distance <= range && distance > stop)
        {

            //move towards the player // aller a l'objectif
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                                                     Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }

       // ON EST DANS LA BONNE DISTANCE POUR REGARDER L'OBJET
        if (distance <= stop && distance > stop2)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                                                   Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

        }
        // SI ON EST TROP PRES ON RECULE
        if(distance <= stop2)
        {

            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                                                           Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

            Vector3 diff = target.position - myTransform.position;
            Vector3 diffXZ = new Vector3(diff.x, 0, diff.z);

           if(backwardSolution)myTransform.position -= diffXZ * moveSpeed / 10 * Time.deltaTime;

        }




    }


    void OnTriggerEnter(Collider e)
    {
      // Debug.LogError("Trigger enter");

        backwardSolution = false;
    }

    void OnTriggerExit(Collider e)
    {
        backwardSolution = true;
    }

}



