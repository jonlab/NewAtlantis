using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;



public class ShepardRissetBarberpole_Eau2 : MonoBehaviour
{
    // Methodes du plugin

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_init(int bufferSize, int _bandWidth, float _fMin, int _chord, float _velocity, int _direction, float _damping, float _focus, string _fileName);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_process(float[] data, int size);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_setBandwidth(int _bandwidth);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_setFmin(float _fMin);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_setChord(int _chord);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_setVelocity(float _velocity);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_setDirection(int _direction);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_setDamping(float _damping);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_setFocus(float _focus);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_setInputWave(string _fileName);

    [DllImport("BarberPoleFilter_UnityPlugin_Eau2")]
    private static extern void synth_allocateSignalBuffers(int _bufferSize);

    String resourcesPath;
    String filePath;

    // Tweaking parameters

    public enum InputWaveFiles
    {
        Water = 0,
        Fountain = 1,
        HydroWon = 2,
        SPAD = 3,
        Marbles = 4,
        HailStorm = 5,
        Earthquake = 6
    }

	public InputWaveFiles inputWaveFile = InputWaveFiles.Fountain;

    [Range(1, 9)]
    public int bandwidth = 2;

    [Range(13.75f, 440.0f)]
    public float lowerBound = 13.75f;

    public enum ChordTypes
    {
        None = 0,
        Minor = 1,
        Major = 2
    }

    public ChordTypes chord = ChordTypes.Minor;

    [Range(0.1f, 1.0f)]
    public float velocity = 0.1f;

    public enum Directions
    {
        Upward = 0,
        Downward = 1
    }

    public Directions direction = Directions.Downward;

    // Pour un damping constant selon la frequence
    //	[Range(0.01f, 500.0f)]
    //	public float damping = 100.0f;

    // Pour un facteur de qualité constant selon la frequence (donc un damping proportionnel à la fréquence)
    //float min = 1.0023f; // 10^(0.001)
    //float max = 10.f; // 10^1
    [Range(0f, 1f)]
    public float tonalVsNoisy = 1f;

    [Range(0.1f, 100.0f)]
    public float focus = 0.1f;



    // Memories (to check if the user requested some changes in the parameters)

    int prev_bandwidth;
    float prev_lowerBound;
    ChordTypes prev_chord;
    float prev_velocity;
    Directions prev_direction;
    float prev_tonalVsNoisy;
    float prev_focus;
    InputWaveFiles prev_inputWaveFile;

    int bufferSize;


    void OnAudioFilterRead(float[] data, int channels)
    {
        //Debug.Log("data.Length = " + data.Length);
        if (data.Length != bufferSize)
        {
            bufferSize = data.Length;
            synth_allocateSignalBuffers(bufferSize);
        }

        synth_process(data, data.Length);
    }


    // Use this for initialization
    void Start()
    {
        bufferSize = 2048;

        prev_bandwidth = bandwidth;
        prev_lowerBound = lowerBound;
        prev_chord = chord;
        prev_velocity = velocity;
        prev_direction = direction;
        prev_tonalVsNoisy = tonalVsNoisy;
        prev_focus = focus;
        prev_inputWaveFile = inputWaveFile;

        int myChord = get_currentChord(chord);
        int myDirection = get_currentDirection(direction);

//#if UNITY_EDITOR
//        resourcesPath = Application.dataPath + "/InputWaves/";
//#elif UNITY_STANDALONE
//            resourcesPath = "./InputWaves/";
//#else
//            Debug.Log("Any other platform!");
//#endif

		resourcesPath = "./ShepardRisset_InputWaves/";

        filePath = get_currentFilePath(inputWaveFile);

        float damping = 0.001f + (1f - (float)Math.Exp(4 * tonalVsNoisy)) / (1f - (float)Math.Exp(4)) * (1f - 0.001f);

        synth_init(bufferSize, bandwidth, lowerBound, myChord, velocity, myDirection, damping, focus, filePath);
    }


    // Update is called once per frame
    void Update()
    {
        if (bandwidth != prev_bandwidth)
        {
            synth_setBandwidth(bandwidth);
            prev_bandwidth = bandwidth;
        }

        if (lowerBound != prev_lowerBound)
        {
            synth_setFmin(lowerBound);
            prev_lowerBound = lowerBound;
        }

        if (chord != prev_chord)
        {
            int myChord = get_currentChord(chord);
            synth_setChord(myChord);
            prev_chord = chord;
        }

        if (velocity != prev_velocity)
        {
            synth_setVelocity(velocity);
            prev_velocity = velocity;
        }

        if (direction != prev_direction)
        {
            int myDirection = get_currentDirection(direction);
            synth_setDirection(myDirection);
            prev_direction = direction;
        }

        if (tonalVsNoisy != prev_tonalVsNoisy)
        {
            float damping = 0.001f + (1f - (float)Math.Exp(4 * tonalVsNoisy)) / (1f - (float)Math.Exp(4)) * (1f - 0.001f);
            synth_setDamping(damping);
            prev_tonalVsNoisy = tonalVsNoisy;
        }

        if (focus != prev_focus)
        {
            synth_setFocus(focus);
            prev_focus = focus;
        }

        if (inputWaveFile != prev_inputWaveFile)
        {

            filePath = get_currentFilePath(inputWaveFile);
            synth_setInputWave(filePath);
            prev_inputWaveFile = inputWaveFile;
        }
    }


    String get_currentFilePath(InputWaveFiles _inputWaveFile)
    {

        String fileName;

        switch (_inputWaveFile)
        {
            case InputWaveFiles.Water:
                fileName = "WaterNoise_OKforLoop.bin";
                break;
            case InputWaveFiles.Fountain:
                fileName = "WaterFountain_OKforLoop.bin";
                break;
            case InputWaveFiles.HydroWon:
                fileName = "hydrophone_Won_OKforLoop.bin";
                break;
            case InputWaveFiles.SPAD:
                fileName = "noiseFromSpad_OKforLoop.bin";
                break;
            case InputWaveFiles.Marbles:
                fileName = "marbles_OKforLoop.bin";
                break;
            case InputWaveFiles.HailStorm:
                fileName = "HailStorm_OKforLoop.bin";
                break;
            case InputWaveFiles.Earthquake:
                fileName = "Earthquake_OKforLoop.bin";
                break;
            default:
                fileName = "marbles_OKforLoop.bin";
                break;
        }

        String filePath = resourcesPath + fileName;

        return filePath;
    }


    int get_currentChord(ChordTypes _chord)
    {

        int chordNum;

        switch (_chord)
        {
            case ChordTypes.None:
                chordNum = 0;
                break;
            case ChordTypes.Minor:
                chordNum = 1;
                break;
            case ChordTypes.Major:
                chordNum = 2;
                break;
            default:
                chordNum = 0;
                break;
        }

        return chordNum;
    }


    int get_currentDirection(Directions _direction)
    {

        int directionNum;

        switch (_direction)
        {
            case Directions.Upward:
                directionNum = 1;
                break;
            case Directions.Downward:
                directionNum = -1;
                break;
            default:
                directionNum = 1;
                break;
        }

        return directionNum;
    }

	public void reloadInputWave()
	{
		synth_setInputWave(filePath);
	}
}
