using UnityEngine;
using System.Collections;


public class NAAnimalBehavior : MonoBehaviour {

	private float 	timer 				= 0f;
	public float 	interval 			= 4;
	public float 	intervalVariance 	= 0.5f;
	public float 	moveProbability 	= 0.5f;
	public float 	speed 				= 10f; //scalar speed
	//public float 	StandTime   = 2;
	//public float 	StandTimeVariance = 0.2f;
	//public float 	MoveTime = 2;
	//public float 	MoveTimeVariance = 0.3f;
	public float 	acceleration 		= 20f; //scalar acc
	public float 	steering 			= 90;
	public AudioSource	audioSourceStand = null;
	public AudioSource	audioSourceMove = null;
	public float	originAttraction = 0.1f;


	//directionchangeje 
	//axes (1-2-3)

	//sounds

	//attractors and repulsors
	//collisions behaviors
	//light behavior (seek or hide from)
	//sound behavior
	//social behavior
	//stop
	//
	//fov

	//private Vector3 goal 	= Vector3.zero;
	private bool moving 	= false;
	private float currentInterval = 0;
	private Vector3 attractor = Vector3.zero;
	// Use this for initialization
	void Start () 
	{

		NARandomizeAudioSource ras;
		ras = audioSourceMove.GetComponent<NARandomizeAudioSource>();
		if (ras == null)
			audioSourceMove.gameObject.AddComponent<NARandomizeAudioSource>();
		ras = audioSourceStand.GetComponent<NARandomizeAudioSource>();
		if (ras == null)
			audioSourceStand.gameObject.AddComponent<NARandomizeAudioSource>();
			
		attractor = transform.position;
		currentInterval = interval;
		if (!NA.isClient()) //server and standalone
		{
			//attach Rigidbody if needed
			Rigidbody rb = GetComponent<Rigidbody>();
			if (!rb)
			{
				gameObject.AddComponent<Rigidbody>();
			}
		}
		PickNextGoal();
		moving = true;
	}



	[RPC]
	void Apply(bool stand, bool move, float volume, float pitch, int index)
	{
		LogManager.Log("NAAnimalBehavior::Apply received " + stand + " " + move + " " + volume + " " + pitch + " " + index);
		//this part must be deterministic
		audioSourceStand.Stop();
		audioSourceMove.Stop();
		NARandomizeAudioSource ras = null;
		if (stand)
		{
			ras = audioSourceStand.GetComponent<NARandomizeAudioSource>();
		}
		if (move)
		{
			ras = audioSourceMove.GetComponent<NARandomizeAudioSource>();
		}
		if (ras != null)
		{
			ras.Apply(index, volume, pitch);
		}
		if (stand)
		{
			audioSourceStand.Play();
		}
		if (move)
		{
			audioSourceMove.Play();
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (!NA.isClient()) //server and standalone
		{
			timer += Time.deltaTime;
			if (timer > currentInterval)
			{
				timer -= currentInterval;
				currentInterval = interval+interval*(Random.value-0.5f)*intervalVariance;
				//audioSourceStand.Stop();
				//audioSourceMove.Stop();
				float r = Random.value;

				NARandomizeAudioSource ras = null;
				if (r<moveProbability)
				{
					moving = true;
					PickNextGoal();

					ras = audioSourceMove.GetComponent<NARandomizeAudioSource>();
					if (ras != null)
						ras.Randomize();
					//audioSourceMove.Play();

				}
				else
				{
					moving = false;
					ras = audioSourceStand.GetComponent<NARandomizeAudioSource>();
					if (ras != null)
						ras.Randomize();
					//audioSourceStand.Play();
				}
				NetworkView nv = null;
				nv = GetComponent<NetworkView>();
				/*if (nv == null)
					nv = gameObject.transform.parent.gameObject.GetComponent<NetworkView>();
					*/
				if (nv != null)
				{
					nv.RPC("Apply", RPCMode.All, !moving, moving, ras.GetCurrentVolume(), ras.GetCurrentPitch(), ras.GetCurrentIndex());
				}
				else
				{
					Apply(!moving, moving, ras.GetCurrentVolume(), ras.GetCurrentPitch(), ras.GetCurrentIndex());
				}
			}


			Rigidbody rb = GetComponent<Rigidbody>();
			if (rb)
			{
				Vector3 velprojected = Vector3.Project(rb.velocity, transform.forward);
				//move
				if (moving)
				{
					if (velprojected.magnitude < speed)
					{
						rb.AddForce(transform.forward*acceleration);
					}

				//rb.AddForce(transform.forward*10f);
				}
				else
				{
					/*
					if (velprojected.magnitude > 0)
					{
						rb.AddForce(-transform.forward*100f);
					}
					*/
					//slow down
					//rb.AddForce(-rb.velocity*acceleration);
				}
		
			}
			

				
		}
		else
		{
			//nothing, the client will have his RPC called by the server
		}
	
	}

	void PickNextGoal()
	{
		Quaternion q = transform.rotation;
		Quaternion qmove = Quaternion.Euler(0/*Random.value*360*/,(Random.value-0.5f)*steering, 0/*Random.value*360*/);
		Vector3 dirtoorigin = attractor-transform.position;
		if (dirtoorigin.magnitude > 0)
		{
			Quaternion qLook = Quaternion.LookRotation(dirtoorigin);
			transform.rotation = Quaternion.Lerp(qmove*q, qLook, originAttraction);
		}
		else
		{
			transform.rotation = qmove*q;
		}


	}

	void OnCollisionEnter(Collision collision) 
	{
		//behavior when hit something
		/*
		Debug.Log("collision with " + collision.contacts.Length + " contacts");
		transform.rotation = Quaternion.FromToRotation(Vector3.forward, collision.contacts[0].normal);
		//transform.rotation = Quaternion.FromToRotation(Vector3.right, collision.contacts[0].normal);
		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb)
		{
			rb.AddForce(transform.forward*400);
		}
		moving = true;
		*/
	}
}
