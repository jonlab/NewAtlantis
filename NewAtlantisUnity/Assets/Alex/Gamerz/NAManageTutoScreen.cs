using UnityEngine;
using System.Collections;

public class NAManageTutoScreen : MonoBehaviour {
    public GameObject tuto;
    NAPlayMovieTexture pmt;
    //public GameObject cameraGO;

    //Camera camera;

    public GameObject mainViewer;

    bool state = false;
	// Use this for initialization
	void Start () {
        pmt = tuto.GetComponent<NAPlayMovieTexture>();
       // camera = cameraGO.GetComponent<Camera>();
        
	}
	
	// Update is called once per frame
	void Update () {


        //print(cameras.Length);

        if (Input.GetKeyDown(KeyCode.N))
        {
            state = !state;
    
            tuto.SetActive(state);

            if (state)
            {
                //Camera.main = camera;
                pmt.resetTuto();

            }
            else
            {



            }

              //  mainViewer.SetActive(!state);

            
              //  camera
                    
            }
        }
	
}
