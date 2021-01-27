using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ButtonBehaviour : MonoBehaviour {
    public Animator transition;
    [Range(0.1f, 2f)]
    public float transitionTime = 0.5f;

    private AudioSource soundFX;

    private PlayerInput controls;


    private void Awake() {
        soundFX = gameObject.GetComponent<AudioSource>();
        controls = new PlayerInput();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update() {
        if (controls.Creature.jump.triggered)
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
                restartGame();
            }
            else {
                playGame();
            }
        }
        else if (controls.Creature.dash.triggered)
        {
            exitGame();
        }
    }

    public void playGame() {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void restartGame() {
        StartCoroutine(loadLevel(0));
    }

    public void exitGame()
    {
        Application.Quit();
    }
    
    IEnumerator loadLevel(int levelIndex) {
        if (soundFX.isPlaying) {
            soundFX.Stop();
        }
        soundFX.Play();
        
        yield return new WaitForSeconds(0.2f);
        
        // Used for scene transition
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime - 0.2f);
        
        SceneManager.LoadScene(levelIndex);
    }
}
