using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class deathEffect : MonoBehaviour
{
    public float duration = 0.3f;

    private DigitalGlitch digital_glitch;
    private AnalogGlitch analog_glitch;
    private Vision vision;

    private void Start()
    {
        digital_glitch = GetComponent<DigitalGlitch>();
        analog_glitch = GetComponent<AnalogGlitch>();
        vision = GetComponent<Vision>();
    }

    public void death()
    {
        StartCoroutine(applyEffect());
    }

    IEnumerator applyEffect()
    {
        digital_glitch.enabled = true;
        analog_glitch.enabled = true;
        vision.enabled = true;
        
        
        yield return new WaitForSeconds(duration);
        
        digital_glitch.enabled = false;
        analog_glitch.enabled = false;
        vision.enabled = false;
    }
}
