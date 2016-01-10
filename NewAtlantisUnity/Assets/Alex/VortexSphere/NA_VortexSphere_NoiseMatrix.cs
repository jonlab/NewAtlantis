using UnityEngine;
using System.Collections;

public class NA_VortexSphere_NoiseMatrix : MonoBehaviour {

    Matrix4x4 initProjection;

    Camera cam;
    public float epsilon = 0.01f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider e)
    {
        if (e.name == "Main Viewer")
        {
            Transform camera;
            camera = e.transform.Find("Camera");

            cam = camera.GetComponent<Camera>();
            initProjection = cam.projectionMatrix;

            Debug.LogError("on trigger enter" + e.name);

            //   RandomizeMatrixCamera();
            //InvertMatrixCamera();
        }
       // Camera camera = e

    }

    void OnTriggerStay(Collider e)
    {
        if(e.name == "Main Viewer"){
            RandomizeMatrixCamera();
           
        }


    }



    void OnTriggerExit(Collider e)
    {
        // print("on trigger exit");
        if (e.name == "Main Viewer")
        {
            Transform camera;
            camera = e.transform.Find("Camera");

            cam = camera.GetComponent<Camera>();
            cam.projectionMatrix = initProjection;
        }

    }


    void InvertMatrixCamera()
    {
        cam.projectionMatrix = cam.projectionMatrix.inverse ;

    }


        void RandomizeMatrixCamera()
{
           cam.projectionMatrix = rdProjection();

        //cam.projectionMatrix = posProjection();
    }

Matrix4x4 rdProjection()
{
    Matrix4x4 newProjection = new Matrix4x4();
        
        for (int j = 0; j < 4; j++)
        {

        newProjection.SetColumn(j, rdV(initProjection,j)  );
        }
    
        return newProjection;
}

    Matrix4x4 posProjection()
    {
        Matrix4x4 matrix = new Matrix4x4();

        GameObject camGO = cam.gameObject;

        matrix.SetColumn(0, transform.right);
        matrix.SetColumn(1, transform.up);
        matrix.SetColumn(2, transform.forward);
        Vector3 p = transform.position;
        matrix.SetColumn(3, new Vector4(p.x, p.y, p.z, 1));


        return matrix;

    }

    Vector4 rdV(Matrix4x4 mat, int index)
{
        Vector4 v = mat.GetColumn(index);
            float e = epsilon;
    Vector4 addNoise =  new Vector4(Random.Range(-e,e), Random.Range(-e,e), Random.Range(-e,e), Random.Range(-e, e));

        return v + addNoise;
}



}
