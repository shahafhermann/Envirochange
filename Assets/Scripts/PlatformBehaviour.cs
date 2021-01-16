using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private Collider2D[] platformColliders;
    public void Start()
    {
        platformColliders = gameObject.GetComponentsInChildren<Collider2D>();


    }

    public void AlertObservers(string message)
    {
        if (message.Equals("AppearAnimationEnded")) {
            // gameObject.GetComponent<BoxCollider2D>().enabled = true;
            // gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
            // if (gameObject.CompareTag("Magnet")) {
            //     gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
            // }
            for (var index = 0; index < platformColliders.Length; index++)
            {
                platformColliders[index].enabled = true;
            } 
        } 
        if (message.Equals("DisappearAnimationStarted")) {
            // gameObject.GetComponent<BoxCollider2D>().enabled = false;
            // gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            // if (gameObject.CompareTag("Magnet")) {
            //     StartCoroutine(disableGravityField());
            // }
            for (var index = 0; index < platformColliders.Length; index++)
            {
                platformColliders[index].enabled = false;
            } 
        }
    }

    IEnumerator disableGravityField() {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
    }
}
