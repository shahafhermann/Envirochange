using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatio : MonoBehaviour {

    public float targetWidth = 1920f;
    public float targetHeight = 1080f;
    
    void Start() {
        float windowAspect = (float) Screen.width / Screen.height;
        float targetAspect = targetWidth / targetHeight;
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = GetComponent<Camera>();

        if (windowAspect < targetAspect) {
            cam.orthographicSize = targetHeight / 200f / scaleHeight;
        } else {
            cam.orthographicSize = targetHeight / 200f;
        }

    }
}
