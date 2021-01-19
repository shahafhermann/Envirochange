using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private float goalAnimationTime = 0.5f;
    // public Animator goalAnimator;
    private Animator endPlatformAnimator;

    // Will be refactored later
    public Level[] levels;
    private int curLevel = 0;
    
    void Start() {
        Time.timeScale = 1;
        // Repeat change every 3 seconds
        InvokeRepeating(nameof(changeEnvironment), startChangeAfterSeconds, changeRate);
        endPlatformAnimator = levels[curLevel].GetPlatforms()[levels[curLevel].GetPlatforms().Length - 1]
                                .platform.GetComponent<Animator>();
    }
    
    void Update()
    {
        
    }

    /**
     * Insert all environmental changes that should happen here
     */
    private void changeEnvironment() {
        foreach (Platform platform in levels[curLevel].GetPlatforms()) {
            platform.move();
        }
    }

    public void completeLevel() {
        
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        // curLevel++;
    }
    
    IEnumerator loadLevel(int levelIndex) {
        // TODO here we can insert all kinds of operations that will happen after waiting a certain amount of time

        // goalAnimator.SetTrigger("GoalReached");
        endPlatformAnimator.SetTrigger("End");
        yield return new WaitForSeconds(goalAnimationTime);
        
        SceneManager.LoadScene(levelIndex);
    }

    public Level getCurrentLevel() {
        return levels[curLevel];
    }
}
