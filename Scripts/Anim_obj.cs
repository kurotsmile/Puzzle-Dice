using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_obj : MonoBehaviour
{
    public Animator anim;

    public void stop_anim()
    {
        this.anim.enabled = false;
    }

    public void start_anim_play()
    {
        this.anim.enabled = true;
        this.anim.Play("Canvas_play");
    }

    public void start_anim_menu()
    {
        this.anim.enabled = true;
        this.anim.Play("Canvas_game");
    }
}
