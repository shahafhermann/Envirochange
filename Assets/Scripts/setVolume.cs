using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class setVolume : MonoBehaviour
{
    public AudioMixer audioMixer;
    private PlayerInput controls;
    private float currVal = 1f;


    private void Awake() {
        controls = new PlayerInput();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    public void setLevel(float sliderValue)
    {
        currVal = sliderValue;
        audioMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    private void Update()
    {
        if (Gamepad.current != null)
        {
            Debug.Log(currVal);
            if (controls.UI.Vol_up.triggered && currVal + .1f <= 1f)
            {
                setLevel(currVal + .1f);
            }
            else if (controls.UI.Vol_down.triggered)
            {
                if (currVal - .1f <= 0f)
                {
                    setLevel(0.0001f);
                }
                else
                {
                  setLevel(currVal - .1f);  
                }
                
            }
            
        }
    }
}
