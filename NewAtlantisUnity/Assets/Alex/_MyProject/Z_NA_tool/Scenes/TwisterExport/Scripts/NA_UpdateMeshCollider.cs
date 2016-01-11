using UnityEngine;
using System.Collections;

public class NA_UpdateMeshCollider : MonoBehaviour
{

    //  public Rigidbody rigidbody;


    MeshCollider mc;
    // Use this for initialization
    void Start()
    {
        mc = GetComponent<MeshCollider>();
        Rigidbody rg = transform.gameObject.AddComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {


        mc.sharedMesh = GetComponent<MeshFilter>().mesh;


    }

}
