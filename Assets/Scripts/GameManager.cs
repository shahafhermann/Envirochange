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
        Movement
    }

    public float startChangeAfterSeconds = 3f;
    public float changeRate = 3f;
    public float platformEndAnimationDuration = 0.35f;

    private float goalAnimationTime = 2.26f;
    private Animator endPlatformAnimator;

    public static Animator transitionAnimator;

    // Will be refactored later
    public Level[] levels;
    private int curLevel = 0;
    
    private deathEffect death_effect;

    private MusicControl musicControl;
    public int nextLevelMusicNumber;

    public bool useSounds = false;

    private void Awake() {
        transitionAnimator = GameObject.Find("Crossfade").gameObject.GetComponent<Animator>();
        if (useSounds) musicControl = GameObject.Find("SoundManager").GetComponent<MusicControl>();
    }

    void Start() {
        Time.timeScale = 1;
        // Repeat change every 3 seconds
        InvokeRepeating(nameof(changeEnvironment), startChangeAfterSeconds, changeRate);
        endPlatformAnimator = levels[curLevel].GetPlatforms()[levels[curLevel].GetPlatforms().Length - 1]
                                .platform.GetComponent<Animator>();
        death_effect = Camera.main.GetComponent<deathEffect>();
    }

    /**
     * Insert all environmental changes that should happen here
     */
    private void changeEnvironment() {
        foreach (Platform platform in levels[curLevel].GetPlatforms()) {
            StartCoroutine(move(platform));
        }
    }
    
    IEnumerator move(Platform platform) {
        if (platform.changeType == ChangeType.Movement) {
            platform.platform.GetComponent<Animator>().SetTrigger("StartMove");
            yield return new WaitForSeconds(platformEndAnimationDuration);
        }
        
        platform.move();

        if (platform.changeType == ChangeType.Movement) {
            platform.platform.GetComponent<Animator>().SetTrigger("StopMove");
        }
    }

    public void completeLevel() {
        // if (curLevel < levels.Length - 1) { 
        //     curLevel++;
        // }

        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        
    }

    /**
     * Delegation method
     */
    public void playSound(MusicControl.SoundFX sound) {
        if (useSounds) musicControl.playSoundFX(sound);
    }
    
    IEnumerator loadLevel(int levelIndex) {
        endPlatformAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        
        playSound(MusicControl.SoundFX.CompleteLevel);
        yield return new WaitForSeconds(0.9f);
        if (useSounds) musicControl.transitionTo(nextLevelMusicNumber);
        yield return new WaitForSeconds(goalAnimationTime - 1.5f);

        SceneManager.LoadScene(levelIndex);
    }

    public Level getCurrentLevel() {
        return levels[curLevel];
    }

    public void apply_death_effect()
    {
        death_effect.death();
    }
}
