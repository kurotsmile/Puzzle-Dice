using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_handle : MonoBehaviour
{
    public Carrot.Carrot carrot;
    public Dice_Manager dice_manager;
    public Anim_obj anim_canvas;
    public GameObject panel_menu;
    public GameObject panel_play;
    public GameObject panel_gameover;
    public GameObject panel_customer;
    public GameObject[] effect_prefab;
    public AudioSource[] sound;

    [Header("Customer")]
    public InputField inp_customer_row;
    public InputField inp_customer_col;

    public Text txt_highest_score_menu;
    public Text txt_highest_score_gameover;
    private int highest_score_game = 0;

    void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_app);
        this.panel_menu.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_customer.SetActive(false);
        this.panel_gameover.SetActive(false);

        this.highest_score_game = PlayerPrefs.GetInt("highest_score_game",0);
        this.update_emp_hight_score_ui();
        if (this.carrot.get_status_sound()) this.carrot.game.load_bk_music(this.sound[4]);
    }

    private void check_exit_app()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.btn_back_menu();
            this.carrot.set_no_check_exit_app();
        }else if (this.panel_customer.activeInHierarchy)
        {
            this.btn_close_model_customer();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_reset()
    {
        this.panel_gameover.SetActive(false);
        this.dice_manager.reset_dice();
        this.play_sound(1);
    }

    public void create_effect(int index_effect,Vector3 pos,float timer=1f)
    {
        GameObject obj_effect = Instantiate(this.effect_prefab[index_effect]);
        obj_effect.transform.SetParent(this.GetComponent<Game_handle>().panel_play.transform);
        obj_effect.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_effect.transform.position = pos;
        Destroy(obj_effect, timer);
    }

    public void show_gameover()
    {
        this.carrot.play_vibrate();
        int scores_dice = this.dice_manager.get_scores();
        if (scores_dice > this.highest_score_game)
        {
            this.highest_score_game = scores_dice;
            PlayerPrefs.SetInt("highest_score_game", this.highest_score_game);
        }
        this.update_emp_hight_score_ui();
        this.carrot.game.update_scores_player(this.dice_manager.get_scores());
        this.play_sound(2);
        this.panel_gameover.SetActive(true);
        this.carrot.ads.show_ads_Interstitial();
    }

    public void play_sound(int index_sound)
    {
        if (this.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

    public void btn_show_setting()
    {
        this.carrot.ads.Destroy_Banner_Ad();
        Carrot.Carrot_Box box_setting=this.carrot.Create_Setting();
        box_setting.set_act_before_closing(act_after_close_setting);
    }

    private void act_after_close_setting(List<string> list_change)
    {
        foreach(string s in list_change)
        {
            if (s == "list_bk_music") this.carrot.game.load_bk_music(this.sound[4]);
        }

        if (this.carrot.get_status_sound())
            this.sound[4].Play();
        else
            this.sound[4].Stop();
        this.carrot.ads.create_banner_ads();
    }

    public void btn_play(int index_row_and_col)
    {
        this.panel_menu.SetActive(false);
        this.panel_play.SetActive(true);
        this.dice_manager.load(index_row_and_col);
        this.carrot.play_sound_click();
        this.carrot.ads.show_ads_Interstitial();
        this.anim_canvas.start_anim_play();
    }

    public void btn_show_user_login()
    {
        this.carrot.user.show_login();
    }

    public void btn_show_rate()
    {
        this.carrot.show_rate();
    }

    public void btn_back_menu()
    {
        this.anim_canvas.start_anim_menu();
        this.panel_play.SetActive(false);
        this.panel_menu.SetActive(true);
        this.panel_gameover.SetActive(false);
        this.carrot.play_sound_click();
        this.carrot.ads.show_ads_Interstitial();
    }

    public void btn_show_model_customer()
    {
        this.inp_customer_row.text = Random.Range(4, 20).ToString();
        this.inp_customer_col.text = Random.Range(4, 20).ToString();
        this.panel_customer.SetActive(true);
        this.carrot.play_sound_click();
    }

    public void btn_close_model_customer()
    {
        this.panel_customer.SetActive(false);
        this.carrot.play_sound_click();
    }

    public void btn_done_model_customer()
    {
        int n_row = int.Parse(this.inp_customer_row.text);
        int n_col = int.Parse(this.inp_customer_col.text);

        if ((n_row < 4) || (n_col < 4))
        {
            this.carrot.show_msg("Dice Puzzle", "The game cannot be started when the number of rows or columns of dice is less than 4 units!", Carrot.Msg_Icon.Error);
            return;
        }
        this.dice_manager.load(n_row, n_col);
        this.panel_customer.SetActive(false);
        this.panel_menu.SetActive(false);
        this.panel_play.SetActive(true);
        this.carrot.play_sound_click();
    }

    public void btn_show_share()
    {
        this.carrot.show_share();
    }

    public void btn_show_list_app_other()
    {
        this.carrot.show_list_carrot_app();
    }

    private void update_emp_hight_score_ui()
    {
        this.txt_highest_score_menu.text = "Highest score: " + this.highest_score_game.ToString();
        this.txt_highest_score_gameover.text = "Highest score: " + this.highest_score_game.ToString();
    }

    public void btn_show_top_player()
    {
        this.carrot.game.Show_List_Top_player();
    }
}
