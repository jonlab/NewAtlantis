using UnityEngine;
using System.Collections;

public class NAAudioStream : MonoBehaviour {

	WWW www = null;
	//public string url = "http://locus.creacast.com:9001/galilee_pennsylvania.ogg";
	private string url = "http://locus.creacast.com:9001/liverpool_ormskirk";
	float t = 0;
	bool playing = false;
	// Use this for initialization
	void Start () 
	{
		www = new WWW(url);
		//WWW.LoadFromCacheOrDownload(

		//audio.Play();


	}
	
	// Update is called once per frame
	void Update () 
	{
		AudioSource audio = GetComponent<AudioSource>();

		AudioClip clip = www.GetAudioClip(false, true, AudioType.OGGVORBIS);
		if (clip != null && audio.clip == null)
		{
			//AudioSource audio = GetComponent<AudioSource>();
			audio.clip = clip;
		}

		if(!audio.isPlaying && audio.clip.isReadyToPlay)
		{
			audio.Play();
		}


		/*if (!playing && audio.clip.isReadyToPlay)
		{
			audio.PlayOneShot(audio.clip);
		}*/
		//Debug.Log ("www.bytesDownloaded=" + www.bytesDownloaded);
		/*t += Time.deltaTime;

		if (audio.clip == null && t > 3)
		{
			audio.clip = www.GetAudioClip(false, true);//, AudioType.OGGVORBIS); // 2D, streaming
			audio.Play ();
		}
		*/
        //audio.clip = www.GetAudioClip(false, true);//, AudioType.OGGVORBIS); // 2D, streaming
		//Debug.Log ("progress = " + www.progress);
		//Debug.Log ("bytes downloaded = " + www.bytesDownloaded);
		/*if (www.progress > 0 && audio.clip == null)
		{
			Debug.Log("play stream");
			audio.clip = www.GetAudioClip(false, true);//, AudioType.OGGVORBIS); // 2D, streaming
			audio.loop = true;
			audio.Play();

		}
		*/
		/*if (audio.clip == null && www.isDone)
		{
			audio.clip = www.GetAudioClip(false, true);//, AudioType.OGGVORBIS); // 2D, streaming
		}
		if (!audio.isPlaying && audio.clip.isReadyToPlay)
		{
			Debug.Log("play stream");
			audio.loop = true;
			audio.Play();
		}
		*/
	
	}
}
