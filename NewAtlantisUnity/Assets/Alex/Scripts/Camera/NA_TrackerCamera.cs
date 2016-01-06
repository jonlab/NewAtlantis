using UnityEngine;
using System.Collections;
//camera settings x 36   7    -43
public class NA_TrackerCamera : NACamera
{

    public Transform myTransform;
    public Transform target;
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 3.0f;
    public float range = 1000.0f;
    public float range2 = 1000.0f;
    public float stop = 10.0f;
    public float stop2 = 8.0f;

    bool backwardSolution = true;

    public bool autoInit = true;

    public GameObject backwardPoint;

   public GameObject mainViewer, mainViewerCamera;

    GameObject addBackwardPoint;

    public GameObject cameraViewer;

    NA_CameraBackwardSolution backwardSolutionScript;
    //   public NA_CameraBackwardSolution backwardScript;

    public GameObject aParentNode;
    GameObject parentNode;

  
    void Awake()
    {

      //  myTransform = transform;
      //myTransform = 

    }
    // Use this for initialization
    void Start()
    {
        if (autoInit) AssignObjects();
    }


    void OnEnable()
    {
        //if (Time.frameCount < 10) return;
        target = mainViewer.transform;
     //   Debug.LogError("return , main viewer camera is null");



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

        parentNode = GameObject.Instantiate(aParentNode);
        mainViewerCamera.transform.parent = parentNode.transform;

    }

    void OnDisable()
    {
        Destroy(addBackwardPoint);
        Destroy(mainViewerCamera.GetComponent<Rigidbody>());
        Destroy(mainViewerCamera.GetComponent<NA_CameraBackwardSolution>());
        mainViewerCamera.transform.parent = mainViewer.transform;

        Destroy(parentNode);
    }

    void AssignObjects()
    {
         mainViewer = GameObject.Find("Main Viewer");
        if (mainViewer == null) Debug.LogError("Main Viewer not Found.");



         mainViewerCamera = mainViewer.transform.Find("Camera").gameObject;
        if(mainViewerCamera == null) Debug.LogError("Main Viewer Camera not Found.");


        // myTransform = 
        target = mainViewer.transform;
        myTransform = mainViewerCamera.transform;

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



