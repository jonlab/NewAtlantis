using UnityEngine;
using System.Collections;

public class MoveAvatar : MonoBehaviour {

    public float speed = 1;
    public float rotateSpeed = 1;

    Vector3 forward, backward, left, right;

	// Use this for initialization
	void Start () {
    
	}
	
    public void SetDirection(Vector3 f,Vector3 b,Vector3 l,Vector3 r)
    {
        left = l;right = r;forward = f;backward = b;
    }
	// Update is called once per frame
	void Update () {

        forward = transform.forward;
        backward = -transform.forward;
        left = -transform.right;
        right = transform.right;

        forward = new Vector3(0, 0, 1);
        backward = new Vector3(0, 0, -1);
        left = new Vector3(-1, 0, 0);
        right = new Vector3(1, 0, 0);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(forward*speed);
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(backward*speed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(left*speed);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(right*speed);
        }

        if (Input.GetKey(KeyCode.A)) transform.Rotate(new Vector3(0, -rotateSpeed, 0));
        if (Input.GetKey(KeyCode.E)) transform.Rotate(new Vector3(0, rotateSpeed, 0));


    }
}
