using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public Color[] backgroundColors;
    public float colorChangeDuration = 5f;
    private float lerpControl = 0f;
    private SpriteRenderer background;
    private int curColor = 0;

    private void Awake() {
        background = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        background.color = Color.Lerp(backgroundColors[curColor % backgroundColors.Length], 
                                      backgroundColors[(curColor + 1) % backgroundColors.Length], 
                                      lerpControl);
        if (lerpControl < 1) {
            lerpControl += Time.deltaTime / colorChangeDuration;
        } else {
            lerpControl = 0f;
            curColor++;
        }
    }
}
