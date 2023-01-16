using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu_TitleGlitch : MonoBehaviour
{

    [SerializeField] private GameObject titleBase, titleGlitch1, titleGlitch2;

    private bool canChangeGlitch, glitch1, glitch2;
    private int whichGlitch;
    private float glitchDelayTime = 0.07f;

    
    void Start()
    {
        titleGlitch1.SetActive(false);
        titleGlitch2.SetActive(false);
        titleBase.SetActive(true);

        StartCoroutine(GlitchDelay());
    }



    private IEnumerator GlitchDelay()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 3f));

        float glitchChooser = Random.Range (0f, 4f);

        if (glitchChooser < 1f)
        {
            StartCoroutine(Glitch1());
        }
        else if (glitchChooser < 2f)
        {
            StartCoroutine(Glitch2());
        }
        else if (glitchChooser < 3.5f)
        {
            BothGlitches();
        }
        else
        {
            StartCoroutine(GlitchDelay());
        }
        
    }



    private void BothGlitches()
    {

        float glitchOrder = Random.Range(0f, 2f);


        if (glitchOrder < 1f)
        {
            whichGlitch = 1;
            StartCoroutine(Glitch1Both());
            //StartCoroutine(GlitchDelay());
        }

        else
        {
            whichGlitch = 2;
            StartCoroutine(Glitch2Both());
            //StartCoroutine(GlitchDelay());
        }

    }



    private IEnumerator Glitch1Both()
    {

        titleBase.SetActive(false);
        titleGlitch1.SetActive(true);

        yield return new WaitForSeconds(glitchDelayTime);

        titleGlitch1.SetActive(false);
        titleBase.SetActive(true);

        if (whichGlitch == 1)
        {
            StartCoroutine(Glitch2Both());
        }
        else
        {
            StartCoroutine(GlitchDelay());
        }

    }



    private IEnumerator Glitch2Both()
    {

        titleBase.SetActive(false);
        titleGlitch2.SetActive(true);

        yield return new WaitForSeconds(glitchDelayTime);

        titleGlitch2.SetActive(false);
        titleBase.SetActive(true);

        if (whichGlitch == 2)
        {
            StartCoroutine(Glitch1Both());
        }
        else
        {
            StartCoroutine(GlitchDelay());
        }

    }



    private IEnumerator Glitch1()
    {

        titleBase.SetActive(false);
        titleGlitch1.SetActive(true);

        yield return new WaitForSeconds(glitchDelayTime);

        titleGlitch1.SetActive(false);
        titleBase.SetActive(true);

        StartCoroutine(GlitchDelay());

    }



    private IEnumerator Glitch2()
    {

        titleBase.SetActive(false);
        titleGlitch2.SetActive(true);

        yield return new WaitForSeconds(glitchDelayTime);

        titleGlitch2.SetActive(false);
        titleBase.SetActive(true);

        StartCoroutine(GlitchDelay());

    }
}
