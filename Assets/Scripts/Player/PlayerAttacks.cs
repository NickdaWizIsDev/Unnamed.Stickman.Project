using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    Animator anim;
    CharacterController controller;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started && controller.isGrounded)
        {
            anim.SetTrigger(AnimStrings.atkTrig);
        }
    }
}
