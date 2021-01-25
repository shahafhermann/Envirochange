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

    private float goalAnimationTime = 2.26f;
    // public Animator goalAnimator;
    private Animator endPlatformAnimator;

    [HideInInspector]
    public static bool exploded = false;
    public static Animator transitionAnimator;
    // [Range(0.1f, 2f)]
    // private float transitionTime = 0.1f;

    // Will be refactored later
    public Level[] levels;
    private int curLevel = 0;
    
    // 0 = dash, 1 = death, 2 = nextLevel, 3 = respawn, 4 = magneticField
    public AudioClip[] sounds;
    private AudioSource soundFX;

    private void Awake() {
        transitionAnimator = GameObject.Find("Crossfade").gameObject.GetComponent<Animator>();
        soundFX = gameObject.GetComponent<AudioSource>();
    }

    void Start() {
        Time.timeScale = 1;
        // Repeat change every 3 seconds
        InvokeRepeating(nameof(changeEnvironment), startChangeAfterSeconds, changeRate);
        endPlatformAnimator = levels[curLevel].GetPlatforms()[levels[curLevel].GetPlatforms().Length - 1]
                                .platform.GetComponent<Animator>();
    }
    
    void Update()
    {
        // if (exploded) {
        //     StartCoroutine(levelTransitionHelper());
        //     exploded = false;
        // }
    }

    /**
     * Insert all environmental changes that should happen here
     */
    private void changeEnvironment() {
        foreach (Platform platform in levels[curLevel].GetPlatforms()) {
            // if (platform.changeType == ChangeType.Movement) {
            //     StartCoroutine(animateMove(platform.platform));
            // }
            platform.move();
        }
    }
    
    // IEnumerator animateMove(GameObject platform) {
    //     platform.GetComponent<Animator>().SetTrigger("Move");
    //     yield return new WaitForSeconds(2f);
    // }

    public void completeLevel() {
        // if (curLevel < levels.Length - 1) { 
        //     curLevel++;
        // }

        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        
    }

    public void playSound(int index) {
        soundFX.clip = sounds[index];
        if (soundFX.isPlaying) {
            soundFX.Stop();
        }
        soundFX.Play();
    }
    
    IEnumerator loadLevel(int levelIndex) {
        endPlatformAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        
        playSound(2);
        yield return new WaitForSeconds(goalAnimationTime - 1f);

        SceneManager.LoadScene(levelIndex);
    }

    // IEnumerator levelTransitionHelper() {
    //     // Level transition
    //     transitionAnimator.SetTrigger("Start");
    //     yield return new WaitForSeconds(transitionTime);
    // }

    public Level getCurrentLevel() {
        return levels[curLevel];
    }
}
