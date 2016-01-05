using UnityEngine;
using System.Collections;

public class NAaudio_volume_scaler : MonoBehaviour {
	
	public int detail = 500;
	public float minValue = 1.0f;
	public float amplitude = 0.1f;
	

	private Vector3 startScale;
	
	void  Start ()
	{
		startScale = transform.localScale;
	}
	
	void  Update (){
		float[] info = new float[detail];
		
		AudioListener.GetOutputData(info, 0);
		float packagedData = 0.0f;
		
		for(int j = 0; j < info.Length; j++)
		{
			packagedData += System.Math.Abs(info[j]);  
		}
		
		transform.localScale = new Vector3((packagedData * amplitude) + startScale.y, (packagedData * amplitude) + startScale.y, (packagedData * amplitude) + startScale.z);
	}
}