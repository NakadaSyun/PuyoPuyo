using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "mode_data")]
public class mode_vari : ScriptableObject
{
    // Start is called before the first frame update
    //色を指定
    //bool[] col_flg = { false,false,false,false,false,false};
    [SerializeField]int color_max;      //色の数
    [SerializeField]int game_mode;     //0:エンドレス 1:時間制限あり

    //
    void Start()
    {
    }

     public void set_Data(int color_num,int mode)
    {
        color_max = color_num;
        game_mode = mode;
    }

    public int Get_color_max()
    {
        return color_max;
    }

    /// <summary>
    ///0:エンドレス 1:時間制限あり
    /// </summary>
    /// <returns></returns>
    public int Get_game_Mode()
    {
        return game_mode;
    }


    //void set_Col_flg(bool[] use_col)
    //{
    //    for(int i =0; i<6; i++)
    //    {
    //        //使う色を指定
    //        col_flg[i] = use_col[i];
    //    }
    //}

    //public bool[] Get_use_Col()
    //{
    //    return col_flg;
    //}
}
