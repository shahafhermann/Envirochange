using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private Collider2D[] platformColliders;
    private Animator transitionAnimator;
    
    public void Start()
    {
        platformColliders = gameObject.GetComponentsInChildren<Collider2D>();
        transitionAnimator = GameManager.transitionAnimator;
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("AppearAnimationEnded")) {
            for (var index = 0; index < platformColliders.Length; index++)
            {
                platformColliders[index].enabled = true;
            } 
        } 
        if (message.Equals("DisappearAnimationStarted")) {
            for (var index = 0; index < platformColliders.Length; index++)
            {
                platformColliders[index].enabled = false;
            } 
        }
        
        if (message.Equals("VortexExplosion")) {
            // Level transition
            transitionAnimator.SetTrigger("Start");
            // StartCoroutine(levelTransition());
        }
    }
    
    
    IEnumerator levelTransition() {
        // Level transition
        transitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(0f);
    }

}
