using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Platform {
    public GameObject platform;
    // public Vector3 basePosition;
    // public Vector3 movedPosition;
    public Vector3[] positions;
    public GameManager.ChangeType changeType;

    private int curPosition = 0;
    // private bool moved = false;

    public void move() {
        switch (changeType) {
            case GameManager.ChangeType.Movement:
                curPosition = (curPosition + 1) % positions.Length;
                platform.transform.position = positions[curPosition];
                
                // if (moved) {
                //     platform.transform.position = basePosition;
                // }
                // else {
                //     // basePosition = platform.transform.position;
                //     platform.transform.position = movedPosition;
                // }
                // moved = !moved;
                break;
            
            case GameManager.ChangeType.Vanish:
                platform.SetActive(!platform.activeSelf);
                break;
            
            case GameManager.ChangeType.Magnetic:
                // TODO nothing?
                break;
            
            default:  // Static
                break;
        }
    }
}
