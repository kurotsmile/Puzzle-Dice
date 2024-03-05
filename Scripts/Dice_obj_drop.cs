using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dice_obj_drop : MonoBehaviour, IDropHandler
{
    public Dice_obj dice;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (dice.get_status_open()==false) GameObject.Find("Game").GetComponent<Game_handle>().dice_manager.change_dice(this.dice);
            if (eventData.pointerDrag.GetComponent<Dice_obj>().get_status_tool() == true)
            {
                GameObject.Find("Game").GetComponent<Game_handle>().dice_manager.change_dice(this.dice);
            }
        }
    } 
}
