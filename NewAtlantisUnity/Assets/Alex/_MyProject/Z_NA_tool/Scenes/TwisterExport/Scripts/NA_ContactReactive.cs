using UnityEngine;
using System.Collections;

public class NA_ContactReactive : MonoBehaviour {

    public GameObject sphere;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision e)
    {
      sphere.transform.position = e.contacts[0].point;

    }
}
