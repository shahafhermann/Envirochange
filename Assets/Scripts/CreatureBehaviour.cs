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

    private Quaternion originalRotation;

    // private Animator animator;
    // private Vector2 _movement;
    
    // public Transform spawnPosition;
    private TrailRenderer trail;

    // Start is called before the first frame update
    void Start() {
        originalRotation = gameObject.transform.rotation;
        
        creature_rigid = GetComponent<Rigidbody2D>();
        num_of_jumps = 0;
        // animator = GetComponent<Animator>();
        
        trail = gameObject.GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        // _movement.x = Input.GetAxisRaw("Horizontal");
        
        float turnSpeed = groundSpeed;
        if (num_of_jumps > 0)
        {
            turnSpeed = midAir;
        }
        if (Input.GetKey("left"))
        {
            creature_rigid.transform.position += -transform.right * (turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey("right"))
        {
            creature_rigid.transform.position += transform.right * (turnSpeed * Time.deltaTime);
        }
        
        if (Input.GetKeyDown("space") && num_of_jumps < MAX_JUMPS_ROW)
        {
            
            num_of_jumps++;
            //creature_rigid.AddForce(new Vector2(0, jumpHeight * Time.fixedDeltaTime), ForceMode2D.Impulse);
            creature_rigid.velocity = new Vector2(creature_rigid.velocity.x, 0f);
            Vector2 jumpDir = new Vector2((transform.up.x + Vector2.up.x) / 2, (transform.up.y + Vector2.up.y) / 2);
            creature_rigid.AddForce(jumpDir * jumpHeight, ForceMode2D.Impulse);
        }
        
        // Animation control
        // animator.SetFloat("Horizontal", _movement.x);
        // animator.SetFloat("Speed", _movement.sqrMagnitude);
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            num_of_jumps = 0;
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Goal")) {
            gameManager.completeLevel();
        }
        
        if (other.gameObject.CompareTag("Bottom"))
        {
            StartCoroutine(respawn());
        }

        if (other.gameObject.CompareTag("Magnet")) {
            magnetizeTo(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Magnet")) {
            magnetizeTo(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Magnet")) {
            Physics2D.gravity = new Vector2(0, -9.8f);
            gameObject.transform.rotation = originalRotation;
        }
    }

    private void magnetizeTo(GameObject other) {
        Vector2 dir = -(transform.position - other.gameObject.transform.position).normalized * 9.8f;
        Physics2D.gravity = dir;
        gameObject.transform.rotation = other.transform.rotation;
    }

    IEnumerator respawn()
    {
        trail.enabled = false;
        num_of_jumps = 0;
        creature_rigid.transform.position = gameManager.getCurrentLevel().getRespawnPosition() 
                                            + new Vector3(0, 0.5f, 0);
        yield return new WaitForSeconds(0.2f);
        trail.enabled = true;
    }
}
