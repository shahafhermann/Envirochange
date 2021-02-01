using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ButtonBehaviour : MonoBehaviour {
    public Animator transition;
    [Range(0.1f, 2f)]
    public float transitionTime = 0.5f;

    // private AudioSource soundFX;
    private MusicControl musicControl;

    private PlayerInput controls;

    private GameObject joystic_instructions;
    private GameObject keyboard_instructions;

    private void Awake() {
        // soundFX = gameObject.GetComponent<AudioSource>();
        controls = new PlayerInput();
        musicControl = GameObject.Find("SoundManager").GetComponent<MusicControl>();
        Debug.Log(gameObject.transform.GetChild(2).name);
        joystic_instructions = gameObject.transform.GetChild(2).GetChild(0).gameObject;
        keyboard_instructions = gameObject.transform.GetChild(2).GetChild(1).gameObject;
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

        if (Gamepad.current != null)
        {
            keyboard_instructions.SetActive(false);
            joystic_instructions.SetActive(true);
        }
        else
        {
            keyboard_instructions.SetActive(true);
            joystic_instructions.SetActive(false);
        }
    }

    public void playGame() {
        musicControl.playSoundFX(MusicControl.SoundFX.MenuButton);
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1, 1));
    }

    public void continueGame() {
        GameData data = SaveSystem.LoadLevel();
        musicControl.playSoundFX(MusicControl.SoundFX.MenuButton);
        StartCoroutine(loadLevel(data.level, data.snapshot));
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
