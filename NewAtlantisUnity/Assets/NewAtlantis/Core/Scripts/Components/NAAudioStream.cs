using UnityEngine;
using System.Collections;

public class NAAudioStream : MonoBehaviour {

	WWW www = null;
	//public string url = "http://locus.creacast.com:9001/galilee_pennsylvania.ogg";
	//private string url = "http://locus.creacast.com:9001/liverpool_ormskirk";
	//private string url = "http://locus.creacast.com:9001/wave_farm_pond_station_new_york.mp3"; //not supported
	//private string url = "http://locus.creacast.com:9001/mrs_splitsoundscape1.ogg";
	//private string url = "http://locus.creacast.com:9001/london_camberwell.ogg";
	private string url = "http://ia902707.us.archive.org/11/items/NearlyCompleteHPLovecraftCollection/01_The_Whisperer_in_Darkness_01.mp3";
	float t = 0;
	bool playing = false;
	// Use this for initialization
	void Start () 
	{
		//StartCoroutine(DownloadAndPlay("http://api.ispeech.org/api/rest?apikey=...&action=convert&voice=eurspanishfemale&text=Hola+que+tal"));  
		StartCoroutine(DownloadAndPlay(url));  
		/*www = new WWW(url);
		yield www;

		AudioClip clip = www.GetAudioClip(false, true);//, AudioType.OGGVORBIS);
		AudioSource audio = GetComponent<AudioSource>();
		audio.clip = clip;
		*/
		//WWW.LoadFromCacheOrDownload(

		//audio.Play();


	}

	/*void Start(){
		StartCoroutine(DownloadAndPlay("http://api.ispeech.org/api/rest?apikey=...&action=convert&voice=eurspanishfemale&text=Hola+que+tal"));  
	}
	*/
	
	IEnumerator DownloadAndPlay(string url)
	{
		www = new WWW(url);
		yield return www;
		AudioSource audio = GetComponent<AudioSource>();
		Debug.Log ("GetAudioClip");
		audio.clip = www.GetAudioClip(false, true);//, AudioType.MPEG);
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log("error=" + www.error);
		AudioSource audio = GetComponent<AudioSource>();
		Debug.Log("audio playing=" + audio.isPlaying);
		if (!audio.isPlaying)
			audio.Play ();
		
		//Debug.Log ("bytes=" + www.bytesDownloaded);
		/*
		AudioSource audio = GetComponent<AudioSource>();
		Debug.Log ("progress=" + www.progress);
		Debug.Log ("error=" + www.error);
		Debug.Log ("isDone=" + www.isDone);
		//Debug.Log ("header = " + www.responseHeaders);
		Debug.Log ("size = " + www.size);
		*/
		//Debug.Log ("bytes="+www.bytesDownloaded);
		/*AudioClip clip = www.GetAudioClip(false, true);//, AudioType.OGGVORBIS);
		if (clip != null && audio.clip == null)
		{
			//AudioSource audio = GetComponent<AudioSource>();
			audio.clip = clip;
		}

		if(!audio.isPlaying && audio.clip.isReadyToPlay)
		{
			audio.Play();
		}

		*/
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
