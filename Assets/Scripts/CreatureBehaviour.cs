using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.TextCore;

public class CreatureBehaviour : MonoBehaviour {
    public GameManager gameManager;
    
    private Rigidbody2D creature_rigid;
    public float jumpHeight = 0f;
    public float groundSpeed = 5f;
    public float midAir = 5f;
    public int MAX_JUMPS_ROW = 2;
    private int num_of_jumps;

    // private Animator animator;
    private Vector2 _movement;

    // Start is called before the first frame update
    void Start()
    {
        creature_rigid = GetComponent<Rigidbody2D>();
        num_of_jumps = 0;
        // animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        
        float turnSpeed = groundSpeed;
        if (num_of_jumps > 0)
        {
            turnSpeed = midAir;
        }
        if (Input.GetKey("left"))
        {
            creature_rigid.transform.position += Vector3.left * (turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey("right"))
        {
            creature_rigid.transform.position += Vector3.right * (turnSpeed * Time.deltaTime);
        }
        
        if (Input.GetKeyDown("space") && num_of_jumps < MAX_JUMPS_ROW)
        {
            
            num_of_jumps++;
            //creature_rigid.AddForce(new Vector2(0, jumpHeight * Time.fixedDeltaTime), ForceMode2D.Impulse);
            creature_rigid.velocity = new Vector2(creature_rigid.velocity.x, 0f);
            creature_rigid.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
        
        // Animation control
        // animator.SetFloat("Horizontal", _movement.x);
        // animator.SetFloat("Speed", _movement.sqrMagnitude);
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            Debug.Log("made contact!");
            num_of_jumps = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Goal")) {
            Debug.Log("Reached Goal!");
            gameManager.completeLevel();
        }
    }
}
