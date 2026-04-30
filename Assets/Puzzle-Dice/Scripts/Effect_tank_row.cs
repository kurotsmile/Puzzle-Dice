using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_tank_row : MonoBehaviour
{
    private float speed = 200f;
    public GameObject effect_smoke_prefab;
    public Transform tr_pos_create;

    float timer_create_effect = 0f;
    void Update()
    {
        this.timer_create_effect += 1f * Time.deltaTime;
        if (this.timer_create_effect > 0.2f)
        {
            GameObject obj_effect_smoke = Instantiate(this.effect_smoke_prefab);
            obj_effect_smoke.transform.SetParent(this.transform.parent.transform);
            obj_effect_smoke.transform.position = tr_pos_create.position;
            obj_effect_smoke.transform.localScale = new Vector3(1f, 1f, 1f);
            Destroy(obj_effect_smoke, 2f);
            this.timer_create_effect = 0;
        }
        this.transform.position+=Vector3.right * this.speed * Time.deltaTime;
    }
}
