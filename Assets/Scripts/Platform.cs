using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Platform {
    public GameObject platform;
    public Vector3[] positions;
    public float[] zRotations;
    public GameManager.ChangeType changeType;
    private Shakeit _shake;

    private int curIndex = 0;

    public void Start()
    {
        _shake = platform.GetComponent<Shakeit>();
    } 

    public void move() {
        int maxLength = Mathf.Max(positions.Length, zRotations.Length);
        if (maxLength > 0) {
            
            curIndex = (curIndex + 1) % maxLength;  // Advance to next index
                
            if (curIndex < positions.Length) {  // Change position
                Vector3 position = positions[curIndex];
                platform.transform.position = position;
                _shake.setPosition(position);
            }

            if (curIndex < zRotations.Length) {  // Change rotation
                platform.transform.eulerAngles = new Vector3(0, 0, zRotations[curIndex]);
            }

        }
    }
}
