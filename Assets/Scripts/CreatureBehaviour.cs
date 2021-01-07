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
    public float groundSpeed = 5f;
    public float midAir = 5f;
    public int MAX_JUMPS_ROW = 2;
    private int num_of_jumps;

    
    // Start is called before the first frame update
    void Start()
    {
        creature_rigid = GetComponent<Rigidbody2D>();
        num_of_jumps = MAX_JUMPS_ROW;
    }

    private void Update()
    {
        if (num_of_jumps == MAX_JUMPS_ROW)
        {
            if (Input.GetKey("left"))
            {
                creature_rigid.transform.position += Vector3.left * groundSpeed * Time.deltaTime;
            }
            if (Input.GetKey("right"))
            {
                creature_rigid.transform.position += Vector3.right * groundSpeed * Time.deltaTime;
            }
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("jumps = " + num_of_jumps);
        if (num_of_jumps  < MAX_JUMPS_ROW)
        {
            if (Input.GetKey("left"))
            {
                creature_rigid.AddForce(new Vector2(-midAir, 0));
            }
            if (Input.GetKey("right"))
            {
                creature_rigid.AddForce(new Vector2(midAir, 0));
            }
        }
        if (Input.GetKeyDown("space") && num_of_jumps > 0)
        {
            creature_rigid.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            num_of_jumps--;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            Debug.Log("made contact!");
            num_of_jumps = MAX_JUMPS_ROW;
        }
    }
}
