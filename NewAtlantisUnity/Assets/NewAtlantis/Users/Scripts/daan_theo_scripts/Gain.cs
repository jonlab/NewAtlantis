using UnityEngine;
using System.Collections;

public class Gain : MonoBehaviour
{
	public float gain;

	void OnAudioFilterRead(float[] data, int channels) {
		gain = Mathf.Round(gain * 100f) / 100f;

		for (var j = 0; j < data.Length; ++j)
		data[j] = data[j] * gain;			
	}
}


