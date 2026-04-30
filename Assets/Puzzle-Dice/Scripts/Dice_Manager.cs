using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice_Manager : MonoBehaviour
{
    public int col_dice = 4;
    public int row_dice = 4;
    public Sprite[] sp_dice;
    public GameObject dice_obj_prefab;
    public GameObject row_dice_obj_prefab;
    public GameObject effect_boom_prefab;
    public Text txt_scores;

    public Transform area_dice;
    public Transform area_dice_tray;
    private Dice_obj dice_obj_tray=null;
    private Dice_obj dice_select = null;
    private List<Row_Dice> list_row_dice = null;
    private List<Dice_obj> list_true_dice = null;
    private int scores;

    public void load(int num_emp)
    {
        this.col_dice = num_emp;
        this.row_dice = num_emp;
        this.reset_dice();
    }

    public void load(int num_row,int num_col)
    {
        this.col_dice = num_col;
        this.row_dice = num_row;
        this.reset_dice();
    }

    private void create_table_dice()
    {
        this.GetComponent<Game_handle>().carrot.clear_contain(this.area_dice);
        this.list_row_dice = new List<Row_Dice>();

        for (int i = 0; i < this.row_dice; i++)
        {
            GameObject obj_dice_row = Instantiate(this.row_dice_obj_prefab);
            obj_dice_row.transform.SetParent(this.area_dice);
            obj_dice_row.transform.localScale = new Vector3(1f, 1f, 1f);
            obj_dice_row.GetComponent<Row_Dice>().load(this.dice_obj_prefab, this.col_dice,i);
            this.list_row_dice.Add(obj_dice_row.GetComponent<Row_Dice>());
        }
    }

    private void create_dice_tray(bool is_tool)
    {
        this.un_select_all_dice();
        this.GetComponent<Game_handle>().carrot.clear_contain(this.area_dice_tray);
        GameObject obj_dice_none = Instantiate(this.dice_obj_prefab);
        obj_dice_none.transform.SetParent(this.area_dice_tray);
        obj_dice_none.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_dice_none.transform.localPosition = new Vector3(0f, 0f, 0f);
        int index_dice = this.get_random_type_dice(is_tool);
        obj_dice_none.GetComponent<Dice_obj>().img_dice.sprite = this.sp_dice[index_dice];
        obj_dice_none.GetComponent<Dice_obj>().load(true);
        Destroy(obj_dice_none.GetComponent<Dice_obj_drop>());

        if (index_dice == 7 || index_dice == 8)
            obj_dice_none.GetComponent<Dice_obj>().is_tool = true;
   
        obj_dice_none.GetComponent<Dice_obj>().type_dice = index_dice;
        this.dice_obj_tray = obj_dice_none.GetComponent<Dice_obj>();
    }

    private int get_random_type_dice(bool is_tool)
    {
        if (is_tool)
        {
            int random_type = Random.Range(0, 3);
            if(random_type==1)
                return Random.Range(1, this.sp_dice.Length);
            else
                return Random.Range(1, this.sp_dice.Length - 2);
        }
        else
        {
            return Random.Range(1, this.sp_dice.Length-2);
        }
    }

    public void change_dice(Dice_obj dice_sel)
    {
        if (this.dice_obj_tray.is_tool)
        {
            this.un_select_all_dice();
            if(this.dice_obj_tray.type_dice==7)
                this.clear_row_dice(dice_sel.row);
            else
                this.clear_col_dice(dice_sel.col);
        }
        else
        {
            dice_sel.img_dice.sprite = this.dice_obj_tray.img_dice.sprite;
            dice_sel.type_dice = this.dice_obj_tray.type_dice;
            dice_sel.open();
            this.check_table_dice(dice_sel);
        }

        this.create_dice_tray(true);
        this.GetComponent<Game_handle>().carrot.play_sound_click();
    }

    public void reset_dice()
    {
        this.create_table_dice();
        this.create_dice_tray(false);
        this.scores = 0;
        this.txt_scores.text = this.scores.ToString();
    }

    private void check_table_dice(Dice_obj dice_check)
    {
        this.list_true_dice = new List<Dice_obj>();

        int count_full = 0;
        for (int i = 0; i < this.list_row_dice.Count; i++)
        {
            if (this.list_row_dice[i].check_row_true(dice_check))
            {
                List<Dice_obj> list_dice_true = this.list_row_dice[i].get_list();
                for (int y=0;y< list_dice_true.Count;y++)
                {
                    this.list_true_dice.Add(list_dice_true[y]);
                }
            }
            else
            {
                if (this.list_row_dice[i].check_row_full()) count_full++;
            }
        }

        bool is_check_true=this.check_col_true(dice_check);

        if (count_full == this.list_row_dice.Count&& is_check_true==false)
        {
            this.GetComponent<Game_handle>().show_gameover();
        }

        this.scores += this.list_true_dice.Count;
        this.txt_scores.text = this.scores.ToString();
        this.GetComponent<Game_handle>().carrot.delay_function(1f, act_effect_destroy);
    }

    private void act_effect_destroy()
    {
        if(this.list_true_dice.Count>0) this.GetComponent<Game_handle>().play_sound(3);

        for (int i = 0; i < this.list_true_dice.Count; i++)
        {
            this.list_true_dice[i].reset();
            this.GetComponent<Game_handle>().create_effect(0,this.list_true_dice[i].gameObject.transform.position);
        }
    }

    private bool check_col_true(Dice_obj dice_check)
    {
        int count_true = 0;
        bool is_check_true = false;
        List<Dice_obj> list_true = new List<Dice_obj>();

        for (int i = 0; i < this.list_row_dice.Count; i++)
        {
            
            if (this.list_row_dice[i].get_col(dice_check.col).type_dice == dice_check.type_dice)
            {
                count_true++;
                list_true.Add(this.list_row_dice[i].get_col(dice_check.col));
            }

            if (count_true == this.col_dice)
            {
                is_check_true = true;
                for(int a = 0; a < list_true.Count; a++)
                {
                    this.list_true_dice.Add(list_true[a]);
                }
            }
        }
        return is_check_true;
    }

    public int get_scores()
    {
        return this.scores;
    }

    public void sel_move_dice(Dice_obj dice_sel)
    {
        if (this.dice_obj_tray.get_status_tool())
        {
            this.change_dice(dice_sel);
            return;
        }
        this.un_select_all_dice();

        int index_top = dice_sel.row - 1;
        int index_bottom = dice_sel.row + 1;
        int index_left = dice_sel.col - 1;
        int index_right = dice_sel.col + 1;

        if (index_top >= 0) this.get_dice(dice_sel.col, index_top).select();
        if (index_bottom < this.row_dice) this.get_dice(dice_sel.col, index_bottom).select();
        if (index_left >= 0) this.get_dice(index_left, dice_sel.row).select();
        if (index_right < this.col_dice) this.get_dice(index_right, dice_sel.row).select();

        this.dice_select = dice_sel;
    }

    private Dice_obj get_dice(int col,int row)
    {
        return this.list_row_dice[row].get_col(col);
    }

    public void un_select_all_dice()
    {
        for(int i = 0; i < this.list_row_dice.Count; i++)
        {
            this.list_row_dice[i].un_select();
        }
    }

    public void act_move_to(Dice_obj dice_to)
    {
        dice_to.img_dice.sprite = this.dice_select.img_dice.sprite;
        dice_to.type_dice = this.dice_select.type_dice;
        dice_to.open();
        this.dice_select.reset();
        this.dice_select = null;
        this.check_table_dice(dice_to);
        this.GetComponent<Game_handle>().play_sound(1);
        this.create_dice_after_move();
    }

    private void create_dice_after_move()
    {
        List<Dice_obj> list_none = new List<Dice_obj>();
        for(int i = 0; i < this.list_row_dice.Count; i++)
        {
            list_none.AddRange(this.list_row_dice[i].get_list_dice_none());
        }

        int index_random_dice = Random.Range(0, list_none.Count);
        list_none[index_random_dice].re_create(this.dice_obj_tray);
        this.check_table_dice(list_none[index_random_dice]);
        this.create_dice_tray(false);
    }


    private void clear_row_dice(int row_sel)
    {
        this.list_row_dice[row_sel].clear();
        Vector3 pos_effect = this.list_row_dice[row_sel].get_col(0).transform.position;
        this.GetComponent<Game_handle>().create_effect(1, pos_effect, 5f);
        this.GetComponent<Game_handle>().play_sound(5);
    }

    private void clear_col_dice(int col_dice)
    {
        for(int i = 0; i < this.list_row_dice.Count; i++)
        {
            this.list_row_dice[i].get_col(col_dice).reset();
        }
        Vector3 pos_effect = this.list_row_dice[this.list_row_dice.Count-1].get_col(col_dice).transform.position;
        this.GetComponent<Game_handle>().create_effect(2, pos_effect, 5f);
        this.GetComponent<Game_handle>().play_sound(6);
    }

    public void select_all_dice_none()
    {
        for (int i = 0; i < this.list_row_dice.Count; i++)
        {
            this.list_row_dice[i].un_select();
        }

        for (int i = 0; i < this.list_row_dice.Count; i++)
        {
            List<Dice_obj> list_none=this.list_row_dice[i].get_list_dice_none();
            for (int y = 0; y < list_none.Count; y++) list_none[y].select();
        }
    }

    public void select_all_dic_for_tool()
    {
        for (int i = 0; i < this.list_row_dice.Count; i++)
        {
            this.list_row_dice[i].un_select();
        }

        for (int i = 0; i < this.list_row_dice.Count; i++)
        {
            List<Dice_obj> list_dice = this.list_row_dice[i].get_list();
            for (int y = 0; y < list_dice.Count; y++) list_dice[y].select_for_tool();
        }
    }

}
