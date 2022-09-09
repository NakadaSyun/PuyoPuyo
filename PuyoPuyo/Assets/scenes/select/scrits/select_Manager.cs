using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class select_Manager : MonoBehaviour
{
    // Start is called before the first frame update

    int Select_Col;     //選択している行 0:ソロorCPU 1:ソロのみ　endless or timeattack 2:color

    int Max_Color;

    int[] mode_sel = { 0, 0 };
    [SerializeField]GameObject[] Selecter;
    Vector3[,] poss = { { new Vector3(-2.25f, 0.8f, 0.0f), new Vector3(2.3f, 0.8f, 0.0f) },
                        { new Vector3(-2.0f, -0.9f, 0.0f), new Vector3(2.5f, -0.9f, 0.0f) }};

    //色数のテキスト
    [SerializeField]Text color_text;

    //データ保存用
    [SerializeField] mode_vari MV;

    //CPUを選んでいる間,色を変える
    [SerializeField] GameObject[] modes;

    [SerializeField] GameObject[] sounds;

    [SerializeField]private GameObject _feed;

    void Start()
    {
        Select_Col = 0;
        Max_Color = 4;
         

        mode_sel[0] = 0;
        mode_sel[1] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Key_input();
        UI_view();
    }

    void Key_input()
    { 
        //右矢印キーを押したとき
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            right_key_func();
        }
        //左矢印キーを押したとき
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            left_key_func();
        }
        //上矢印キーを押したとき
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            col_Up();
            sounds[1].GetComponent<SE_Manager>().SE_Play(1);
        }

        //下矢印キーを押したとき
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            col_down();
            sounds[1].GetComponent<SE_Manager>().SE_Play(1);
        }

        //Spaceキーを押したとき
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            sounds[1].GetComponent<SE_Manager>().SE_Play(0);
            _feed.GetComponent<feed_Con>().Is_feedout();
            Invoke("GotoMain",1.2f);
        }
    }


    void UI_view()
    {
        color_text.text = Max_Color.ToString();
        Move_Selecter();
        mode_Color();
    }


    void mode_Color()
    {
        if(mode_sel[0] == 1)
        {
            foreach(GameObject obj in modes)
            {
                Color _color = obj.GetComponent<SpriteRenderer>().color;
                _color.a = 0.5f;
                obj.GetComponent<SpriteRenderer>().color = _color;
            }
        }
        else
        {
            foreach (GameObject obj in modes)
            {
                Color _color = obj.GetComponent<SpriteRenderer>().color;
                _color.a = 1.0f;
                obj.GetComponent<SpriteRenderer>().color = _color;
            }
        }
    }

    void change_flg(int num)
    {
        mode_sel[num] = 1 - mode_sel[num];
    }

    void col_down()
    {
        if(++Select_Col >= 3)
        {
            Select_Col = 0;
        }
        if(Select_Col == 1 && mode_sel[0] == 1)
        {
            Select_Col++;
        }
    }

    void col_Up()
    {
        if (--Select_Col < 0)
        {
            Select_Col = 2;
        }
        if (Select_Col == 1 && mode_sel[0] == 1)
        {
            Select_Col--;
        }
    }

    void Color_up()
    {
        if(++Max_Color > 6)
        {
            Max_Color = 6;
            sounds[1].GetComponent<SE_Manager>().SE_Play(2);
        }
        else
        {
            sounds[1].GetComponent<SE_Manager>().SE_Play(1);
        }
    }
    void Color_down()
    {
        if (--Max_Color < 2)
        {
            Max_Color = 2;
            sounds[1].GetComponent<SE_Manager>().SE_Play(2);
        }
        else
        {
            sounds[1].GetComponent<SE_Manager>().SE_Play(1);
        }
    }


    void Move_Selecter()
    {
        for(int i=0; i<2; i++)
        {
            Selecter[i].transform.position = poss[i,mode_sel[i]];
        }
    }

    void right_key_func()
    {
        if(Select_Col <= 1)
        {
            change_flg(Select_Col);
            sounds[1].GetComponent<SE_Manager>().SE_Play(1);
        }
        else if(Select_Col == 2)
        {
            Color_up();
        }
    }

    void left_key_func()
    {
        if (Select_Col <= 1)
        {
            change_flg(Select_Col);
            sounds[1].GetComponent<SE_Manager>().SE_Play(1);
        }
        else if (Select_Col == 2)
        {
            Color_down();
        }
    }

    public int Get_num()
    {
        return Select_Col;
    }


    void GotoMain()
    {
        set_game_Data();
        if(mode_sel[0] == 0)    //ソロモード
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
        else                    //CPU対戦
        {
            MV.set_Data(Max_Color, 0);
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }

    void set_game_Data()
    {
        MV.set_Data(Max_Color, mode_sel[1]);
    }
}
