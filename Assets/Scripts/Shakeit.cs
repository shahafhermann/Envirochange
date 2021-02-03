using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shakeit : MonoBehaviour
{
    [Range(0f, 100f)]
    public float min_speed = 50.0f;
    
    [Range(0f, 200f)]
    public float max_speed = 85.0f;

    [Range(0f, 0.1f)] public float max_offset = 0.062f;
    
    private float speed; //how fast it shakes
    private Vector3 original_position;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(min_speed, max_speed);
        original_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float noise = Mathf.Sin(Time.time * speed);
        float x_pos = original_position.x + (noise * Random.Range(-max_offset, max_offset));
        float y_pos = original_position.y + (noise * Random.Range(-max_offset, max_offset));
        float z_pos = original_position.z;
        transform.position = new Vector3(x_pos, y_pos, z_pos);
    }
    
    public void setPosition(Vector3 newPosition)
    {
        original_position = newPosition;
    }
}
