using UnityEngine;
using System.Collections;

public class NA_AvatarEditor : MonoBehaviour {

    App app;
    public GameObject avatarModel, canvas, eventSystem, view;
    DesignAvatar designAvatar;

	// Use this for initialization
	void Start () {
        app = GameObject.Find("Main Viewer").GetComponent<App>();
     
	}
	
	// Update is called once per frame
	void Update () {



        if (app.getTabOpen()) StopAvatarEdition();
	}

    public void StartAvatarEdition()
    {
        //if(desi)
       
      

        avatarModel.SetActive(true);
        canvas.SetActive(true);
        eventSystem.SetActive(true);
        view.SetActive(true);

        print(avatarModel.name);
        designAvatar = (DesignAvatar)avatarModel.GetComponent(typeof(DesignAvatar));

      
        designAvatar.loadValues();

    }

    public void StopAvatarEdition()
    {
        avatarModel.SetActive(false);
        canvas.SetActive(false);
        eventSystem.SetActive(false);
        view.SetActive(false);

    }
}
