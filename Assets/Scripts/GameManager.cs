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

    public float goalAnimationTime = 1f;
    public Animator goalAnimator;

    // Will be refactored later
    public Level[] levels;
    private int curLevel = 0;

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
        foreach (Platform platform in levels[curLevel].GetPlatforms()) {
            platform.move();
        }
    }

    public void completeLevel() {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        curLevel++;
    }
    
    IEnumerator loadLevel(int levelIndex) {
        // TODO here we can insert all kinds of operations that will happen after waiting a certain amount of time

        goalAnimator.SetTrigger("GoalReached");
        yield return new WaitForSeconds(goalAnimationTime);

        SceneManager.LoadScene(levelIndex);
    }
}
