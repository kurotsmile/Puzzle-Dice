using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dice_obj_drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Dice_obj dice;
    private bool is_drag = false;
    public CanvasGroup canvas_group;

    public void OnDrag(PointerEventData eventData)
    {
        if (this.is_drag)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.is_drag = true;
        this.canvas_group.blocksRaycasts = false;
        if(this.dice.get_status_tool())
            GameObject.Find("Game").GetComponent<Game_handle>().dice_manager.select_all_dic_for_tool();
        else
            GameObject.Find("Game").GetComponent<Game_handle>().dice_manager.select_all_dice_none();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.is_drag = false;
        this.canvas_group.blocksRaycasts = true;
        this.transform.localPosition = new Vector3(0f, 0f, 0f);
        GameObject.Find("Game").GetComponent<Game_handle>().dice_manager.un_select_all_dice();
    }


}
