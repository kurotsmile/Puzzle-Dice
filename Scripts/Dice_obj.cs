using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice_obj : MonoBehaviour
{
    public Image img_dice;
    public GameObject obj_img;
    public GameObject obj_effect;
    public Animator ani;
    public bool is_tool = false;

    public int col = -1;
    public int row = -1;
    public int type_dice = -1;

    private bool is_in_tray = false;

    private bool is_open = false;
    private bool is_select = false;


    public void load(bool in_tray)
    {
        this.obj_img.SetActive(false);
        if (in_tray)
        {
            this.ani.Play("dice_effect");
            this.obj_effect.SetActive(true);
        }
        else
        {
            this.ani.Play("dice_nomal");
            this.obj_effect.SetActive(false);
        }
        this.is_open = false;
        this.is_select = false;
        this.is_in_tray = in_tray;
        
    }

    public void click()
    {
        if (this.is_in_tray)
        {
            GameObject.Find("Game").GetComponent<Game_handle>().play_sound(0);
        }
        else
        {
            if (this.is_select)
            {
                 GameObject.Find("Game").GetComponent<Game_handle>().dice_manager.act_move_to(this);
            }
            else
            {
                if (this.is_open == false)
                    GameObject.Find("Game").GetComponent<Game_handle>().dice_manager.change_dice(this);
                else
                    GameObject.Find("Game").GetComponent<Game_handle>().dice_manager.sel_move_dice(this);
            }
        }

    }

    public void open()
    {
        this.is_open = true;
        this.obj_img.SetActive(true);
        this.obj_effect.SetActive(false);
    }

    public void reset()
    {
        this.obj_img.SetActive(false);
        this.obj_effect.SetActive(false);
        this.type_dice = -1;
        this.is_open = false;
        this.is_select = false;
        this.ani.Play("dice_nomal");
    }

    public void select()
    {
        if (this.is_open == false)
        {
            this.ani.Play("dice_select");
            this.is_select = true;
        }
    }

    public void select_for_tool()
    {
        this.ani.Play("dice_select");
        this.is_select = true;
    }

    public void un_select()
    {
        if (this.is_select)
        {
            this.ani.Play("dice_nomal");
            this.GetComponent<Image>().color = Color.white;
            this.img_dice.color = Color.white;
            this.is_select = false;
        }
    }

    public bool get_status_open()
    {
        return this.is_open;
    }

    public bool get_status_tool()
    {
        return this.is_tool;
    }

    public void re_create(Dice_obj dice_tray)
    {
        this.is_open = false;
        this.is_select = false;
        this.is_in_tray = false;
        this.type_dice = dice_tray.type_dice;
        this.img_dice.sprite = dice_tray.img_dice.sprite;
        this.obj_img.SetActive(false);
        this.ani.Play("dice_effect");
        this.obj_effect.SetActive(true);
    }
}
