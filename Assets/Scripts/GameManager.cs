using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {

    public float startChangeAfterSeconds = 3f;
    public float changeRate = 3f;

    // Will be refactored later
    public Transform platform1;
    public Transform platform2;

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
        Vector3 temp_pos = platform1.position;
        platform1.position = new Vector3(platform2.position.x, temp_pos.y, temp_pos.z);
        platform2.position = new Vector3(temp_pos.x, platform2.position.y, platform2.position.z);
    }
}
