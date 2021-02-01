using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class controllerInstructions : MonoBehaviour
{
    private GameObject joystic_instructions;
    private GameObject keyboard_instructions;
    // Start is called before the first frame update
    void Start()
    {
        
        keyboard_instructions = gameObject.transform.GetChild(0).gameObject;
        joystic_instructions = gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            keyboard_instructions.SetActive(false);
            joystic_instructions.SetActive(true);
        }
        else
        {
            keyboard_instructions.SetActive(true);
            joystic_instructions.SetActive(false);
        }
    }
}
