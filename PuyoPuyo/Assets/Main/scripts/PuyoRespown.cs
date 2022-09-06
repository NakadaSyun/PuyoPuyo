﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoRespown : MonoBehaviour
{
    /// <summary>
    /// 0->Blue , 1->Gray , 2->Green , 3->Purple , 4->Red , 5->Yerrow
    /// </summary>
    [SerializeField] GameObject[] puyo_temp = new GameObject[6];
    private int Color_Max = 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ぷよの生成処理
    public GameObject[] Respown()
    {
        GameObject[] newPuyo = new GameObject[2];

        GameObject obj1 = Instantiate(puyo_temp[UnityEngine.Random.Range(0, Color_Max)]);
        GameObject obj2 = Instantiate(puyo_temp[UnityEngine.Random.Range(0, Color_Max)]);
        

        newPuyo[0] = obj1;
        newPuyo[1] = obj2;

        return newPuyo;
    }

    public void Set_MaxColor(int num)
    {
        if (num < 0) num = 0;
        if (num > 6) num = 6;

        Color_Max = num;
    }

}
