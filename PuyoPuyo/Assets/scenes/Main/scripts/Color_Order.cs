using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Puyo_Colors")]
public class Color_Order : ScriptableObject
{
    //色の順序を格納
    List<int> color_order = new List<int>();
    int Color_Max;

    public void List_reset(int color)
    {
        color_order.Clear();
        Color_Max = color;
    }

    public void Mk_color_num(int qty)
    {
        for(int i=0; i <= qty; i++)
        {
            int num = UnityEngine.Random.Range(0, Color_Max);
            color_order.Add(num);
        }
    }

    public int Get_color_num(int index)
    {
        if(color_order.Count <= index)
        {
            Mk_color_num(100);
        }

        return color_order[index];
    }
}
