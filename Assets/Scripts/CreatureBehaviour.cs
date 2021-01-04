using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore;

public class CreatureBehaviour : MonoBehaviour
{
    private Rigidbody2D creature_rigid;
    public float jumpHeight = 0f;
    private bool isJumping = false;
    [FormerlySerializedAs("midAirTurnForce")] public float sidewaySpeed = 5f;

    
    // Start is called before the first frame update
    void Start()
    {
        creature_rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        if (Input.GetKey("space") && !isJumping)
        {
            creature_rigid.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            isJumping = true;
        }
        
        if (Input.GetKey("left"))
        {
            creature_rigid.AddForce(new Vector2(-sidewaySpeed, 0));
        }
        if (Input.GetKey("right"))
        {
            creature_rigid.AddForce(new Vector2(sidewaySpeed, 0));
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            Debug.Log("made contact!");
            isJumping = false;
        }
    }
}
