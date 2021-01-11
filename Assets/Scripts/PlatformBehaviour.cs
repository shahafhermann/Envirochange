using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    public void AlertObservers(string message)
    {
        if (message.Equals("AppearAnimationEnded")) {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            if (gameObject.CompareTag("Magnet")) {
                gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
            }
        } 
        if (message.Equals("DisappearAnimationStarted")) {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (gameObject.CompareTag("Magnet")) {
                StartCoroutine(disableGravityField());
            }
        }
    }

    IEnumerator disableGravityField() {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
    }
}
