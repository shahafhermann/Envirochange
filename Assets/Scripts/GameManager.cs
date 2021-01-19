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

    private float goalAnimationTime = 0.25f;
    // public Animator goalAnimator;
    private Animator endPlatformAnimator;

    public Animator transitionAnimator;
    [Range(0.1f, 2f)]
    public float transitionTime = 1f;

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
        // if (curLevel < levels.Length - 1) { 
        //     curLevel++;
        // }
        
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        
    }
    
    IEnumerator loadLevel(int levelIndex) {
        endPlatformAnimator.SetTrigger("End");
        yield return new WaitForSeconds(goalAnimationTime);
        
        // Level transition
        transitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        
        SceneManager.LoadScene(levelIndex);
    }

    public Level getCurrentLevel() {
        return levels[curLevel];
    }
}
