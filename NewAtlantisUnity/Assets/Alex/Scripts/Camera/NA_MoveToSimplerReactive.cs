using UnityEngine;
using System.Collections;
//camera settings x 36   7    -43
public class NA_MoveToSimplerReactive : MonoBehaviour
{

    public Transform myTransform;
    public Transform target;
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 3.0f;
    public float range = 1000.0f;
    public float range2 = 1000.0f;
    public float stop = 10.0f;
    public float stop2 = 8.0f;

    //private FindClosestWithThisTag getClosestScript;
    //DisplayLife timeScript;


    //private GameObject firstPoint;
    //private GameObject secondPoint;

    public bool autoInit = true;

    void Awake()
    {

        myTransform = transform;

    }
    // Use this for initialization
    void Start()
    {

        if (!autoInit) AssignObjects();
    }

    void AssignObjects()
    {/*
        GameObject mainViewer = GameObject.Find("Main Viewer");
        GameObject mainViewerCamera = mainViewer.Find("Camera");
        // myTransform = 
        target = mainViewer.transform;
        */

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


        if (distance <= stop && distance > stop2)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                                                   Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

        }

        if(distance <= stop2)
        {

            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                                                           Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

            Vector3 diff = target.position - myTransform.position;
            Vector3 diffXZ = new Vector3(diff.x, 0, diff.z);

            myTransform.position -= diffXZ * moveSpeed / 10 * Time.deltaTime;

        }




    }



}



