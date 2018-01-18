using UnityEngine;

using System.Collections;



public class env_AudioSpect : MonoBehaviour
{



    // Public Variables

    public AudioListener Audio;

    public int numSamples = 64;

    public GameObject abar;


    // Private Varaibles

    float[] numberleft = new float[64];

    float[] numberright = new float[64];

    GameObject[] thebarsleft;

    GameObject[] thebarsright;

    float spacing;

    float width;




    void Start()
    {

        thebarsleft = new GameObject[numSamples];

        thebarsright = new GameObject[numSamples];

        spacing = 0.4f - (numSamples * 0.001f);

        width = 0.3f - (numSamples * 0.001f);

        for (int i = 0; i < numSamples; i++)
        {

            float xpos = i * spacing - 8.0f;

            Vector3 positionleft = new Vector3(xpos, 3, 0);

            thebarsleft[i] = (GameObject)Instantiate(abar, positionleft, Quaternion.identity) as GameObject;

            thebarsleft[i].transform.localScale = new Vector3(width, 1, 0.2f);



            Vector3 positionright = new Vector3(xpos, -3, 0);

            thebarsright[i] = (GameObject)Instantiate(abar, positionright, Quaternion.identity) as GameObject;

            thebarsright[i].transform.localScale = new Vector3(width, 1, 0.2f);

        }

    }



    void Update()
    {



        AudioListener.GetOutputData(numberleft, 0);

        AudioListener.GetOutputData(numberright, 1);



        //numberleft = Audio.GetSpectrumData(numSamples, 0, FFTWindow.BlackmanHarris);

        //numberright = Audio.GetSpectrumData(numSamples, 1, FFTWindow.BlackmanHarris);



        for (int i = 0; i < numSamples; i++)
        {

            if (float.IsInfinity(numberleft[i] * 30) || float.IsNaN(numberleft[i] * 30))
            {

            }
            else
            {

                thebarsleft[i].transform.localScale = new Vector3(width, numberleft[i] * 30, 0.2f);

                thebarsright[i].transform.localScale = new Vector3(width, numberright[i] * 30, 0.2f);

            }
        }
    }
}