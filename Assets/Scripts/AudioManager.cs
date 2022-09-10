using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    float[] samples = new float[512];
    float[] frequencyBands = new float[8];
    float[] frequencyBandsBuffer = new float[8];
    float[] frequencyBandsBufferDecrease = new float[8];

    //for normalized values audio bands
    float[] frequencyBandsHighest = new float[8];
    public float[] audioBandsNormalized = new float[8];
    public float[] audioBandsBufferNormalized = new float[8];

    public GameObject SampleCubePrefab;
    GameObject[] middleCubes = new GameObject[8];
    public float maxScale = 1000, maxScaleMiddle = 50;

    private GameObject[] planets;

    // Start is called before the first frame update
    void Start()
    {
        //set audio source
        audioSource = GetComponent<AudioSource>();
        planets = GameObject.FindGameObjectsWithTag("Planet");

        for (int i = 0; i < 8; i++)
        {
            var parentTransform = transform;
            GameObject instanceMiddleCube = Instantiate(SampleCubePrefab, parentTransform, true);
            Vector3 pos = parentTransform.position;
            pos.x += 2 * i;
            pos.z += 50;
            instanceMiddleCube.transform.position = pos;
            instanceMiddleCube.name = "MiddleCube" + i;
            middleCubes[i] = instanceMiddleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        CreateFrequencyBands();
        GetFrequencyBandsBuffer();
        CreateAudioBandsWithBufferNormalized();

        for (int i = 0; i < planets.Length; i++)
        {
            var value = frequencyBandsBuffer[i] * maxScaleMiddle * 0.8f;
            planets[i].transform.localScale = new Vector3(value, value, value);
        }
    }

    void GetSpectrumAudioSource()
    {
        //get the spectrum from audio source and update samples array
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
    }

    void CreateFrequencyBands()
    {
        /*
        * 22050 / 512 = 43 hertz per sample
        *
        * 20 - 60 hertz
        * 60 - 250
        * 250 - 500
        * 500 - 2000
        * 2000 - 4000
        * 4000 - 6000
        * 6000 - 20000
        *
        * 0 - 2 = 86 hertz
        * 1 - 4 = 172 hertz - 87-258
        * 2 - 8 = 344 hertz - 259-602
        * 3 - 16 = 688 hertz - 603-1290
        * 4 - 32 = 1376 hertz - 1291-2666
        * 5 - 64 = 2752 hertz - 2667-5418
        * 6 - 128 = 5504 hertz - 5419-10922
        * 7 - 256 = 11008 hertz - 10923-21930
        * 510
        */

        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            float average = 0;

            //added for fixing to have 5120 values covered instead of 510 only
            if (i == 7)
                sampleCount += 2;

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            frequencyBands[i] = average/count * 10;
        }
    }

    void GetFrequencyBandsBuffer()
    {
        for (int i = 0; i < frequencyBands.Length; i++)
        {
            if (frequencyBands[i] > frequencyBandsBuffer[i])
            {
                frequencyBandsBuffer[i] = frequencyBands[i];
                frequencyBandsBufferDecrease[i] = 0.01f;
            }

            if (frequencyBands[i] < frequencyBandsBuffer[i])
            {
                frequencyBandsBuffer[i] -= frequencyBandsBufferDecrease[i];
                frequencyBandsBufferDecrease[i] *= 1.2f;
            }
        }
    }

    void CreateAudioBandsWithBufferNormalized()
    {
        for (int i = 0; i < frequencyBands.Length; i++)
        {
            if (frequencyBands[i] > frequencyBandsHighest[i])
                frequencyBandsHighest[i] = frequencyBands[i];

            audioBandsNormalized[i] = (frequencyBands[i] / frequencyBandsHighest[i]);
            audioBandsBufferNormalized[i] = (frequencyBandsBuffer[i] / frequencyBandsHighest[i]);
        }
    }
}