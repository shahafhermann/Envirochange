using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Platform {
    public GameObject platform;
    public Vector3 basePosition;
    public Vector3 movedPosition;
    public GameManager.ChangeType changeType;

    private bool moved = false;

    public void move() {
        switch (changeType) {
            case GameManager.ChangeType.Movement:
                if (moved) {
                    platform.transform.position = basePosition;
                }
                else {
                    // basePosition = platform.transform.position;
                    platform.transform.position = movedPosition;
                }
                moved = !moved;
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
