using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Threading;

public class PlayerAttacks : MonoBehaviour
{
    Animator anim;
    CharacterController controller;

    private float timer;
    private bool ready;

    public AudioClip swing;
    private AudioSource source;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        timer = 0f;
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(timer <= 0f)
        {
            ready = true;
        }

        if (!ready)
        {
            timer -= Time.deltaTime;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started && controller.isGrounded && ready)
        {
            anim.SetTrigger(AnimStrings.atkTrig);
            ready = false;
            timer = .1f;
        }
    }

    public void Swing()
    {
        source.PlayOneShot(swing, .5f);
    }
}
