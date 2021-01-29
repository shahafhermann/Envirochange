using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCursor : MonoBehaviour
{
    void Update() {
        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(Mouse.current.position.ReadValue().y - transform.position.y, 
                                     Mouse.current.position.ReadValue().x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        transform.rotation = Quaternion.Euler(0, 0, AngleDeg + 90f);
    }
}
