using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject instructionsMenu;
    public Animator background;
    public float animationTime = 3f;
    public Animator transition;
    [Range(0.1f, 2f)]
    public float transitionTime = 0.5f;
    
    public void playGame() {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    
    IEnumerator loadLevel(int levelIndex) {
        // Could be used to play some animation after pressing play
        // background.SetTrigger("PlayTrigger");
        // yield return new WaitForSeconds(animationTime);
        
        // Used for scene transition
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        
        SceneManager.LoadScene(levelIndex);
        yield return  new WaitForSeconds(0f);  // Unnecessary if having some other returns
    }
}
