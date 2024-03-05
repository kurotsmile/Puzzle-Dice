using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row_Dice : MonoBehaviour
{
    public Transform area_row;
    private int num_dice;
    private List<Dice_obj> list_dice;

    public void load(GameObject dice_prefab,int col_dice,int row_dice)
    {
        this.list_dice = new List<Dice_obj>();
        this.num_dice = col_dice;

        for(int i = 0; i < this.num_dice; i++)
        {
            GameObject obj_dice_none = Instantiate(dice_prefab);
            obj_dice_none.name = "dice_obj_play";
            obj_dice_none.transform.SetParent(this.area_row);
            obj_dice_none.transform.localScale = new Vector3(1f, 1f, 1f);
            obj_dice_none.GetComponent<Dice_obj>().col = i;
            obj_dice_none.GetComponent<Dice_obj>().row = row_dice;
            obj_dice_none.GetComponent<Dice_obj>().load(false);
            Destroy(obj_dice_none.GetComponent<Dice_obj_drag>());
            this.list_dice.Add(obj_dice_none.GetComponent<Dice_obj>());
        }

    }

    public bool check_row_true(Dice_obj dice_check)
    {
        int coun_true = 0;
        for(int i = 0; i < this.list_dice.Count; i++) if (this.list_dice[i].type_dice == dice_check.type_dice) coun_true++;

        if (coun_true == this.list_dice.Count)
            return true;
        else
            return false;
    }

    public bool check_row_full()
    {
        int coun_full = 0;
        for (int i = 0; i < this.list_dice.Count; i++) if (this.list_dice[i].type_dice !=-1) coun_full++;

        if (coun_full == this.list_dice.Count)
            return true;
        else
            return false;
    }

    public List<Dice_obj> get_list()
    {
        return this.list_dice;
    }

    public Dice_obj get_col(int index)
    {
        return this.list_dice[index];
    }

    public void un_select()
    {
        for(int i = 0; i < this.list_dice.Count; i++)
        {
            this.list_dice[i].un_select();
        }
    }

    public List<Dice_obj> get_list_dice_none()
    {
        List<Dice_obj> list_none = new List<Dice_obj>();

        for(int i = 0; i < this.list_dice.Count; i++)
        {
            if (!this.list_dice[i].get_status_open()) list_none.Add(this.list_dice[i]);
        }
        return list_none;
    }

    public void clear()
    {
        for(int i = 0; i < this.list_dice.Count; i++)
        {
            this.list_dice[i].reset();
        }
    }
}
