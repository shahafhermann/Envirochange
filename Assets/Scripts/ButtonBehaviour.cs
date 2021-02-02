using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ButtonBehaviour : MonoBehaviour {
    public Animator transition;
    [Range(0.1f, 2f)]
    public float transitionTime = 0.5f;

    private MusicControl musicControl;

    private PlayerInput controls;

    // public Slider volController;
    

    private void Awake() {
        controls = new PlayerInput();
        musicControl = GameObject.Find("SoundManager").GetComponent<MusicControl>();
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
        if (Gamepad.current == null && controls.Creature.jump.triggered)
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
                restartGame();
            }
            else {
                playGame();
            }
        }

        if (Gamepad.current != null)
        {
            if (controls.Creature.jump.triggered)
            {
                playGame();
            }
            else if (controls.Creature.dash.triggered)
            {
                continueGame();
            }
            else if (controls.UI.EXIT.triggered)
            {
                exitGame();
            }
        }

    }

    public void playGame() {
        musicControl.playSoundFX(MusicControl.SoundFX.MenuButton);
        SaveSystem.SaveLevel(1, 1);
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1, 1));
    }

    public void continueGame() {
        GameData data = SaveSystem.LoadLevel();
        musicControl.playSoundFX(MusicControl.SoundFX.MenuButton);
        if (data == null) {
            playGame();
        }
        else {
            StartCoroutine(loadLevel(data.level, data.snapshot));
        }
    }

    public void restartGame() {
        StartCoroutine(loadLevel(0, 0));
    }

    public void exitGame()
    {
        Application.Quit();
    }
    
    IEnumerator loadLevel(int levelIndex, int snapshot) {
        musicControl.transitionTo(snapshot);
        
        yield return new WaitForSeconds(0.2f);
        
        // Used for scene transition
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime - 0.2f);
        
        SceneManager.LoadScene(levelIndex);
    }
}
