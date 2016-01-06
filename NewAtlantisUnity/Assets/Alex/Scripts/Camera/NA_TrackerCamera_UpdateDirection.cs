using UnityEngine;
using System.Collections;
//camera settings x 36   7    -43
public class NA_TrackerCamera_UpdateDirection : MonoBehaviour
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

    void Awake()
    {

        myTransform = transform;

    }
    // Use this for initialization
    void Start()
    {

        if (autoInit) AssignObjects();
    }

    void AssignObjects()
    {
        GameObject mainViewer = GameObject.Find("Main Viewer");
        GameObject mainViewerCamera = mainViewer.transform.Find("Camera").gameObject;
        // myTransform = 
        target = mainViewer.transform;
        myTransform = mainViewerCamera.transform;

    }

    void Update()
    {


   

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
     
        backwardSolution = false;
    }

    void OnTriggerExit(Collider e)
    {
        backwardSolution = true;
    }

}



