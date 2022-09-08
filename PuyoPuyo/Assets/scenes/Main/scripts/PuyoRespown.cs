using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoRespown : MonoBehaviour
{
    /// <summary>
    /// 0->Blue , 1->Gray , 2->Green , 3->Purple , 4->Red , 5->Yerrow
    /// </summary>
    [SerializeField] GameObject[] puyo_temp = new GameObject[6];

    //次のぷよを置いておく場所2つ分
    [SerializeField] GameObject[] stanby_pos = new GameObject[2];

    //おきぷよ(0,1) その次のおきぷよ(2,3)
    private GameObject[] stanby_puyo = new GameObject[4];

    //ぷよの作成数
    int puyo_count;

    //色取得用
    [SerializeField] Color_Order CO;

    // Start is called before the first frame update
    void Start()
    {
        puyo_count = 0;
        Init_reserve();
        //this.GetComponent<PuyoManager>().create_new_puyo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void puyo_next()
    {
        //セットしたぷよの押上
        stanby_puyo[0] = stanby_puyo[2];
        stanby_puyo[1] = stanby_puyo[3];

        reserve_puyo();
    }
    void reserve_puyo()
    {
        //次のぷよの生成

        stanby_puyo[2] = Puyo_Create();
        stanby_puyo[3] = Puyo_Create();

        set_reseve_pos();
    }

    void set_reseve_pos()
    {
        for(int i=0; i<stanby_pos.Length; i++)
        {
            Vector3 pos = stanby_pos[i].transform.position;

            stanby_puyo[(i*2)].transform.position =pos;

            stanby_puyo[(i*2) + 1].transform.position =pos + new Vector3(0.0f,1.0f,0.0f);
        }
    }


    void Init_reserve()
    {

        stanby_puyo[0] = Puyo_Create();
        stanby_puyo[1] = Puyo_Create();
        stanby_puyo[2] = Puyo_Create();
        stanby_puyo[3] = Puyo_Create();

        set_reseve_pos();
    }

    //ぷよの生成処理
    public GameObject[] Respown()
    {
        //メイン用にでてくるぷよ
        GameObject[] newPuyo = new GameObject[2];


        newPuyo[0] = stanby_puyo[0];
        newPuyo[1] = stanby_puyo[1];

        //次のぷよを準備
        puyo_next();

        return newPuyo;
    }

    GameObject Puyo_Create()
    {
        return Instantiate(puyo_temp[CO.Get_color_num(puyo_count++)]);
    }

}
