using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {

    // Set behaviour types for the platforms
    public enum ChangeType {
        Static,
        Movement,
        Vanish,
        Magnetic
    }

    public float startChangeAfterSeconds = 3f;
    public float changeRate = 3f;

    // Will be refactored later
    public Platform[] platforms;

    void Start()
    {
        // Repeat change every 3 seconds
        InvokeRepeating(nameof(changeEnvironment), startChangeAfterSeconds, changeRate);
    }
    
    void Update()
    {
        
    }

    /**
     * Insert all environmental changes that should happen here
     */
    private void changeEnvironment() {
        foreach (Platform platform in platforms) {
            platform.move();
        }
    }
}
