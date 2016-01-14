using UnityEngine;
using System.Collections;

public static class TransitionManager
{
	public const  int				FadeOut = 1;
	public const  int				FadeIn = 2;
	public const  int				CrossFade = 3;
	//===============================================
	private static Texture2D 		texWhite = null;
	private static RenderTexture	mRT = null;
	private static bool 			mPlaying = false;
	private static float			mTimer = 0f;
	private static float			mDuration = 1f;
	private static int 				mCurrent = 0;
	private static Color			mColor = Color.white;
	public delegate void 			TransitionCallback();
	private static TransitionCallback	mCallback;
	private static bool				bRenderTexture = false;
	public static void Init()
	{
		texWhite = new Texture2D(32, 32, TextureFormat.RGBA32, false);	
		Color[] colors = new Color[32*32];
		for (int i=0;i<32*32;++i)
		{
			colors[i] = Color.white;
		}
		texWhite.SetPixels(colors);
		texWhite.Apply();
		//mRT = new RenderTexture(1024, 768, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		
		if (bRenderTexture)
		{
			mRT = new RenderTexture(1024, 768, 24, RenderTextureFormat.ARGB4444, RenderTextureReadWrite.Default);
			mRT.name = "TransitionRT";
		}
	}
	
	public static void Process()
	{
		//return;
		if (mCurrent != 0 && mPlaying)
		{
			mTimer += Time.deltaTime;
			if (mTimer > mDuration)
			{
				mPlaying = false;
				mTimer = mDuration;
				if (mCallback != null)
					mCallback();
				//Debug.Log("TransitionManager : end of transition !");
			}
		}
	}
	
	public static void DrawGUI()
	{
		//return;
		Matrix4x4 bak = GUI.matrix; //on passe en pixels physiques
		GUI.matrix = Matrix4x4.identity;
		int w = Screen.width;
		int h = Screen.height;
		//int w = 2048;
		//int h = 1536;
		//fwGUIRatio.SetupMatrix(true);
		
		if (mCurrent == FadeOut)
		{
			Color c = mColor;
			c.a = mTimer/mDuration;
			GUI.color = c;
			GUI.DrawTexture(new Rect(0,0,w,h), texWhite);
		}
		else if (mCurrent == FadeIn)
		{
			Color c = mColor;
			c.a = 1-mTimer/mDuration;
			GUI.color = c;
			GUI.DrawTexture(new Rect(0,0,w,h), texWhite);
		}
		else if (mCurrent == 3 && mRT != null)
		{
			Color c = mColor;
			c.a = 1-mTimer/mDuration;
			GUI.color = c;
			GUI.DrawTexture(new Rect(0,0,w,h), mRT);
		}
		GUI.color = Color.white;
		GUI.matrix = bak;
	}
	
	
	public static void Start(int mode, float duration, Color color, TransitionCallback callback)
	{
		mCurrent = mode;
		mDuration = duration;
		mTimer = 0;
		mCallback = callback;
		mPlaying = true;
		mColor = color;
		//Debug.Log ("TransitionManager : start Transition mode="+mode);
		if (mCurrent == FadeIn || mCurrent == FadeOut)
		{
			//fondu à la couleur
		}
		if (mCurrent == CrossFade && mRT != null)
		{
			//CrossFade
			Camera.main.targetTexture 	= mRT;
			Camera.main.Render();
			Camera.main.targetTexture 	= null;
		}
	}
	public static void Stop()
	{
		mPlaying = false;
		mCurrent = 0;
	}
}
