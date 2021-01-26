using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ButtonBehaviour : MonoBehaviour {
    // public GameObject mainMenu;
    // public GameObject instructionsMenu;
    // public Animator background;
    // public float animationTime = 3f;
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
            playGame();
        }
        else if (controls.Creature.dash.triggered)
        {
            exitGame();
        }
    }

    public void playGame() {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void exitGame()
    {
        Application.Quit();
    }
    
    IEnumerator loadLevel(int levelIndex) {
        // Could be used to play some animation after pressing play
        // background.SetTrigger("PlayTrigger");
        // yield return new WaitForSeconds(animationTime);

        if (soundFX.isPlaying) {
            soundFX.Stop();
        }
        soundFX.Play();
        
        yield return new WaitForSeconds(0.2f);
        
        // Used for scene transition
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime - 0.2f);
        
        SceneManager.LoadScene(levelIndex);
        yield return  new WaitForSeconds(0f);  // Unnecessary if having some other returns
    }
}
