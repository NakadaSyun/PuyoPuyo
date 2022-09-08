using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update


    int score;

    private static readonly int[] chain_bonus = { 1, 8, 16, 32, 64, 128, 256, 512, 999 };
    [SerializeField]PuyoManager PM;

    void Start()
    {
        //スコアの初期化
        score = 0;
    }

    public void Set_Score(int del_puyo_sum, int chain)
    {
        score += Score_calcu(del_puyo_sum, chain);
    }

    public int Get_Score()
    {
        return score;
    }

    //１チェインごとのスコア計算用関数
    int Score_calcu(int del_puyo_sum,int chain)
    {
        int bonus_num = chain -1;
        if (bonus_num < 0) return 0;
        if (bonus_num > chain_bonus.Length) bonus_num = chain_bonus.Length - 1;
        int one_chain_score = (del_puyo_sum * 10) * chain_bonus[bonus_num];
        return one_chain_score;
    }
}
