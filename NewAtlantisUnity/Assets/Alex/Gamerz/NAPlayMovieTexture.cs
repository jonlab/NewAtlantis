using UnityEngine;
using System.Collections;

public class NAPlayMovieTexture : MonoBehaviour {

    MovieTexture movie;
    public MovieTexture[] movies;
    int step = 0;
    public GameObject tutoTextMesh;
    NADisplayTutoStep dts;

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.mainTexture = movies[step];
        movies[step].Play();

        dts = tutoTextMesh.GetComponent<NADisplayTutoStep>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!movies[step % movies.Length].isPlaying)
        {
            StopCurrentTuto();
            //print("Load Next tuto");
            step++;
            PlayCurrentTuto();

        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            StopCurrentTuto();
            //GO TO PREVIOUS TUTO
            step--;
            if (step < 0) step += movies.Length;
            PlayCurrentTuto();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StopCurrentTuto();
            //GO TO PREVIOUS TUTO
            step++;
           // if (step < 0) step += movies.Length;
            PlayCurrentTuto();
        }

    }

    public void resetTuto()
    {
        step = 0;
        PlayCurrentTuto();

    }

    void StopCurrentTuto()
    {

        GetComponent<Renderer>().material.mainTexture = movies[step % movies.Length];
        movies[step % movies.Length].Stop();


    }
  
    void PlayCurrentTuto()
    {
        GetComponent<Renderer>().material.mainTexture = movies[step % movies.Length];
        movies[step % movies.Length].Play();
        if(dts == null) dts = tutoTextMesh.GetComponent<NADisplayTutoStep>();
        dts.setCurrentStep(step % movies.Length);
    }


}
