using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] mode_vari MV;

    float TIME_MAX = 60f;
    float time_limit ;

    void Start()
    {
        set_Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Time_Lapse();
    }

    void set_Init()
    {
        if(MV.Get_game_Mode() == 1)
        {
            //時間制限アリの場合
            time_limit = TIME_MAX;
        }
        else
        {
            timer_text_enable();
            //時間制限無しの場合
            this.enabled = false;
        }
    }

    void Time_Lapse()
    {
        time_limit -= Time.deltaTime;

        if(time_limit <= 0)
        {
            this.gameObject.GetComponent<sceneManager>().GotoResult();
        }
    }

    public float Get_timelimit()
    {
        return time_limit;
    }

    void timer_text_enable()
    {
        this.gameObject.GetComponent<UI_Manager>().timer_Enable();
    }
}
