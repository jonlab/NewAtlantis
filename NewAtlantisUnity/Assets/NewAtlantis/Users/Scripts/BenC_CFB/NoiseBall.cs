using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoiseBall : MonoBehaviour {
	[SerializeField]private float m_repulsiveForce;
	[SerializeField]private float m_ballMultiplier = 100.0f;
	[SerializeField]private LineRenderer m_lineRenderer;
	[SerializeField]private LineRenderer m_playerLineRenderer;
	[SerializeField]private Rigidbody m_rigidbody;
	[SerializeField]private LayerMask m_playerLayerMask;
	[SerializeField]private LayerMask m_layerMask;
	[SerializeField]private float m_playerModifier;
    [SerializeField]private AudioReverbZone m_audioReverbZone;
	[SerializeField]private float m_groupMultiplier;
	[SerializeField]private float m_maximumVelocity = 10;
	[SerializeField]private NoiseBall [] m_attractiveBalls;
	
	[SerializeField]private Vector3 origPosition;
	[SerializeField]private float origDistance;


	private const float mc_searchDistance = 15.0f;
	// Use this for initialization
	void Start () {
		origPosition = this.transform.position;
        System.Array values = AudioReverbPreset.GetValues(typeof(AudioReverbPreset));
        AudioReverbPreset randomBar = (AudioReverbPreset)values.GetValue(Random.Range(0,values.Length));

        try { m_audioReverbZone.reverbPreset = (AudioReverbPreset)randomBar;
        }
        catch
        {
            Debug.LogError(this.gameObject.name);
        }
		/*if (!NA.isClient()) 
		{
        	StartCoroutine (RepellBalls());
		}*/
	}
	
	public void FixedUpdate(){
		
		if(m_rigidbody.velocity.magnitude > this.m_maximumVelocity){
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * this.m_maximumVelocity;
		}
	}
	
	public void Update()
	{
		if(m_rigidbody.velocity.magnitude > this.m_maximumVelocity){
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * this.m_maximumVelocity;
		}
		RepellBalls();
	}

	private void RepellBalls(){
        m_rigidbody.AddTorque(new Vector3(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f)));
		//GameObject [] NoisyBalls = GameObject.FindGameObjectsWithTag ("NoiseBall");

		//JT : instead of getting the objects with tag "NoiseBall", we get the NoiseBall holders
		NoiseBall[] NoisyBalls = NoiseBall.FindObjectsOfType<NoiseBall>();

		//while (!NA.isClient()) 
		{
			Collider[] hits = Physics.OverlapSphere (this.transform.position, mc_searchDistance,m_playerLayerMask);

			List<Vector3> positions = new List<Vector3>();

            float averageDistance = 0.0f;
			//foreach hit, render a line
			foreach (Collider hit in hits) {
				positions.Add (this.transform.position);
				positions.Add (hit.transform.position);

                Vector3 direction = this.transform.position - hit.transform.position;

                float distance = Vector3.Distance(this.transform.position, hit.transform.position) + .1f;
                averageDistance += distance;
				if (!NA.isClient())
				{
					m_rigidbody.AddForce(direction.normalized * m_repulsiveForce * Time.deltaTime * -1.0f/distance);
				}
			}

            averageDistance = Mathf.Min(1.0f,averageDistance / (positions.Count * 10.0f));
            if (m_lineRenderer != null)
            {
                m_lineRenderer.SetVertexCount(positions.Count);
                m_lineRenderer.SetPositions(positions.ToArray());
                m_lineRenderer.SetWidth(1/(averageDistance*2.0f), 1/averageDistance);
            }

			if (!NA.isClient())
			{
				foreach(NoiseBall ball in this.m_attractiveBalls){
					Vector3 dir = transform.position - ball.transform.position;
					m_rigidbody.AddForce(dir * m_ballMultiplier * m_repulsiveForce * Time.deltaTime * m_groupMultiplier);
				}
			
				//foreach (GameObject ball in NoisyBalls) {}
				foreach (NoiseBall ball in NoisyBalls) {
					if (ball == this)
						continue; //no self test
					Vector3 direction = this.transform.position - ball.gameObject.transform.position;

	                //add a force inversely proportional to that distance to the ball
	                float distance = Vector3.Distance(this.transform.position, ball.gameObject.transform.position) + .1f;
	                if(distance > 20.0f)
	                {
	                    continue;
	                }

	                m_rigidbody.AddForce(direction * m_repulsiveForce * m_ballMultiplier * Time.deltaTime/distance);
				}
			}
			hits = Physics.OverlapSphere (this.transform.position, mc_searchDistance,m_layerMask);
			positions = new List<Vector3>();
			foreach (Collider hit in hits) {
				positions.Add (this.transform.position);
				positions.Add (hit.transform.position);
               

                Vector3 direction = this.transform.position - hit.transform.position;
				if (!NA.isClient())
				{
					m_rigidbody.AddForce(direction * m_repulsiveForce * m_playerModifier * Time.deltaTime/ (Vector3.Distance(this.transform.position,hit.transform.position)));
				}
			}

            if (m_playerLineRenderer != null)
            {
                m_playerLineRenderer.SetVertexCount(positions.Count);
                m_playerLineRenderer.SetPositions(positions.ToArray());
            }
			if (!NA.isClient())
			{
				origDistance = Vector3.Distance(origPosition,this.transform.position);
				m_rigidbody.AddForce ((origPosition - this.transform.position) * origDistance * m_ballMultiplier * Time.deltaTime);
			}
			
			//yield return new WaitForEndOfFrame ();
		}
	}
}
