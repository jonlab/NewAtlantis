using UnityEngine;
using System.Collections;

public class NA_ModulateProjectionCamera : MonoBehaviour
{

    public Matrix4x4 projection;

    Matrix4x4 initProjection;

    Camera cam;
    public float epsilon = 0.01f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            SetProjection(projection);
        }
        // Camera camera = e

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


    void SetProjection(Matrix4x4 a)
    {

        cam.projectionMatrix = a;

    }

}
