using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class puyo_ai : MonoBehaviour
{
    // Start is called before the first frame update

    public PuyoManager PM;
    //ステージマスの大きさ
    Vector2Int STAGE_MASS = new Vector2Int(6, 15);
    void Awake()
    {
        PM = gameObject.GetComponent<PuyoManager>();
        stage_text = GameObject.Find("Debug_STAGE_UI (1)").GetComponent<Text>();
    }



    [SerializeField] Text stage_text;
    void text()
    {
        string hoge = "";

        for (int i = 13; i >= 0; i--)
        {
            for (int j = 0; j < 6; j++)
            {
                if (PM.get_Stage_info()[j, i] > 0)
                {
                    hoge += PM.get_Stage_info()[j, i] + " ";
                }
                else
                {
                    hoge += "+ ";
                }
            }
            hoge += "\n";
        }
        Debug.Log("STAGEUI : " + PM.gameObject.name);
        stage_text.text = hoge;
    }


    int[,] Stage_info = new int[6,15];
    private void FixedUpdate()
    {
        text();
        st(Stage_info, PM.get_Stage_info());
    }







    public IEnumerator AI_Move()
    {
        yield return new WaitForSeconds(3.0f);
        Search_pos();
        StartCoroutine(AI_Move());
    }

    void Search_pos()
    {
        
        GameObject[] hold_puyos = PM.get_active_Puyos();

        int[]score = pat_evaluation( Mk_All_Pattern(Stage_info, hold_puyos));
        int best_move = Score_High(score);
        Debug.Log("SCORE : " + best_move);

        Debug.Log("STAGEUI : " + PM.gameObject.name);
        //for (int i = STAGE_MASS.y - 1; i >= 0; i--)
        //{
        //    Debug.Log(i + "行目\t\t" + PM.get_Stage_info()[0, i] + "\t" + PM.get_Stage_info()[1, i] + "\t" + PM.get_Stage_info()[2, i] + "\t" + PM.get_Stage_info()[3, i] + "\t" + PM.get_Stage_info()[4, i] + "\t" + PM.get_Stage_info()[5, i]);
        //}


        PM.set_activePuyo_pos(Mk_puyo_poss(best_move, Stage_info));
    }

    int[,,] Mk_All_Pattern(int[,]Stage , GameObject[] puyos)
    {
        int[,,] all_pattern = new int [22,STAGE_MASS.x,STAGE_MASS.y];

        for(int i = 0; i < 22; i++)
        {
            int[,]pos = new int[STAGE_MASS.x, STAGE_MASS.y];

            Array.Copy(pos, Mk_Stage_Pattern(Stage, puyos, i), STAGE_MASS.x * STAGE_MASS.y);

            for (int j=0; j<STAGE_MASS.x; j++)
            {
                for(int k=0; k<STAGE_MASS.y; k++)
                {
                    all_pattern[i,j,k] = pos[j,k];
                }
            }
        }

        return all_pattern;
    }


    int[,] Mk_Stage_Pattern(int[,] Stage, GameObject[] puyos,int num)
    {
        int[,]pattern = new int[STAGE_MASS.x , STAGE_MASS.y];
        Array.Copy(pattern, Stage, STAGE_MASS.x * STAGE_MASS.y);
        
        Vector2Int puyo_pat = place(num);
        if (num < 12)
        {
            //縦置きの場合　座標の一番下から埋める
            for (int i=0; i<STAGE_MASS.y - 1; i++)
            {
                if(pattern[puyo_pat.x,i] == 0)
                {
                    pattern[puyo_pat.x,i] = puyos[1 - puyo_pat.y].GetComponent<Puyo_Color>().Get_Color();
                    pattern[puyo_pat.x,i + 1] = puyos[puyo_pat.y].GetComponent<Puyo_Color>().Get_Color();
                    return pattern;
                }
            }
        }
        else
        {
            bool[] IS_Storage = { false, false };
            //横置きの場合　どちらも高い位置に置く
            for (int i = 0; i < STAGE_MASS.y; i++)
            {
                if (pattern[puyo_pat.x,i] == 0)
                {
                    pattern[puyo_pat.x,i] = puyos[1 - puyo_pat.y].GetComponent<Puyo_Color>().Get_Color();
                    IS_Storage[0] = true;
                }
                if (pattern[puyo_pat.x + 1,i] == 0)
                {
                    pattern[puyo_pat.x + 1,i] = puyos[puyo_pat.y].GetComponent<Puyo_Color>().Get_Color();
                    IS_Storage[1] = true;
                }

                if(IS_Storage[0] && IS_Storage[1])
                {
                    return pattern;
                }
            }
        }

        return pattern;
    }

    Vector2Int place(int num)
    {
        Vector2Int pos = new Vector2Int (0,0);
        if(num < 12)
        {
            //縦置きの場合
            //x座標はそのまま yは反転するかどうか
            pos.x = num / 2;
            pos.y = num % 2;
        }
        else
        {
            //横置きの場合
            //x座標はそのまま yは横向きの時の位置
            pos.x = (num / 2) - 6;
            pos.y = (num % 2);
        }
        return pos;

    }


    int[] pat_evaluation(int[,,]stage)
    {
        int[] score = new int[22];



        for(int i=0; i<22; i++)
        {
            int[,] stage_info =new int[STAGE_MASS.x, STAGE_MASS.y];
            
            for(int j=0; j< STAGE_MASS.x; j++)
            {
                for(int k=0; k<STAGE_MASS.y; k++)
                {
                    stage_info[j,k] = stage[i,j,k];
                }
            }


            score[i] = check_Link(stage_info);
        }
        

        return score;
    }


    Vector2Int[] temp = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
    List<Vector2Int> Link_puyo = new List<Vector2Int>();        //つながっているぷよを一時保存先

    List<List<Vector2Int>> del_puyos = new List<List<Vector2Int>>();        //消えるぷよを格納する　2次元配列
    int check_Link(int[,] Stage_puyo)
    {
        //つながっているぷよを初期化
        del_puyos.Clear();
        Link_puyo.Clear();

        for (int i = 0; i < STAGE_MASS.y; i++)
        {
            for (int j = 0; j < STAGE_MASS.x; j++)
            {
                Vector2Int pos = new Vector2Int(j, i);
                puyo_Link_check(pos,Stage_puyo);
            }
        }
        return del_puyos.Count;
    }

    void puyo_Link_check(Vector2Int pos,int[,]Stage_puyo)
    {
        if (Stage_puyo[pos.x, pos.y] == 0) return;      //空白    
        if (!Is_same_pos(pos)) return;                  //探索済みの座標

        Link_puyo.Clear();
        Link_puyo.Add(pos);
        for (int i = 0; i < temp.Length; i++)
        {
            if (Is_sameColor(pos, i,Stage_puyo))
            {
                Link_storage(pos,Stage_puyo);
            }
        }

        if (Link_puyo.Count >= 4)
        {
            del_puyos.Add(new List<Vector2Int>(Link_puyo));
        }
        return;
    }

    bool Is_same_pos(Vector2Int pos)
    {
        //同じ座標が存在したらfalse
        for (int i = 0; i < del_puyos.Count; i++)
        {
            for (int j = 0; j < del_puyos[i].Count; j++)
            {
                if (del_puyos[i][j] == pos) return false;
            }
        }
        return true;
    }

    void Link_storage(Vector2Int pos, int[,] Stage_puyo)
    {
        int link_puyo_num = Link_puyo.Count;
        bool[] Is_someCol = Is_SameColor(pos,Stage_puyo);
        for (int i = 0; i < temp.Length; i++)
        {
            if (Is_someCol[i])
            {
                bool Is_ident = true;
                foreach (Vector2Int linkpos in Link_puyo)
                {
                    if (linkpos == (pos + temp[i]))      //同じものがあったら終了
                    {
                        Is_ident = false;
                        break;
                    }
                }

                if (Is_ident)       //同じものがなければリストに追加して再帰処理
                {
                    Link_puyo.Add(pos + temp[i]);
                    Link_storage(pos + temp[i],Stage_puyo);
                }
            }
        }
    }

    //４方向の同じ色を探索
    bool[] Is_SameColor(Vector2Int pos, int[,] Stage_puyo)
    {
        bool[] FDir = { false, false, false, false };

        for (int i = 0; i < temp.Length; i++)
        {
            if(Is_sameColor(pos, i,Stage_puyo))
            {
                FDir[i] = true;
            }
        }

        return FDir;
    }
    bool Is_sameColor(Vector2Int pos, int dir, int[,] Stage_puyo)
    {
        Vector2Int next_pos = pos + temp[dir];
        if (next_pos.x >= 0 && next_pos.x < STAGE_MASS.x
                && next_pos.y >= 0 && next_pos.y < STAGE_MASS.y)         //ステージの範囲内
        {
            if (Stage_puyo[pos.x, pos.y] == Stage_puyo[next_pos.x, next_pos.y])
            {
                return true;
            }
        }
        return false;
    }

    int Score_High(int[] score)
    {
        int Max_order = 0;
        for(int i=1; i<score.Length; i++)
        {
            if (score[Max_order] < score[i])
            {
                Max_order = i;
            }
        }
        return Max_order;
    }


    Vector2[] Mk_puyo_poss(int best,int[,]stage)
    {
        Vector2[] poss = { new Vector2(0f,0f) ,new Vector2(0f,0f)};

        Vector2Int puyo_pat = place(best);

        if (best < 12)
        {
            //縦置きの場合　座標の一番下から埋める
            for (int i = 0; i < STAGE_MASS.y - 1; i++)
            {
                if (stage[puyo_pat.x, i] == 0)
                {
                    poss[1 - puyo_pat.y] = new Vector2(puyo_pat.x, i);
                    poss[puyo_pat.y] = new Vector2(puyo_pat.x, i + 1);
                    return poss;
                }
            }
        }
        else
        {
            bool[] IS_Storage = { false, false };
            //横置きの場合　どちらも高い位置に置く
            for (int i = 0; i < STAGE_MASS.y; i++)
            {
                if (stage[puyo_pat.x, i] == 0)
                {
                    poss[1 - puyo_pat.y] = new Vector2(puyo_pat.x, i);
                    IS_Storage[0] = true;
                }
                if (stage[puyo_pat.x + 1, i] == 0)
                {
                    poss[puyo_pat.y] = new Vector2(puyo_pat.x + 1, i);
                    IS_Storage[1] = true;
                }
                if (IS_Storage[0] && IS_Storage[1]) return poss;
            }
        }
        return poss;
    }
    private void st(int[,] copy, int[,] ori)
    {
        for (int i = 0; i < STAGE_MASS.x; i++)
        {
            for (int j = 0; j < STAGE_MASS.y; j++)
            {
                copy[i, j] = ori[i, j];
            }
        }
    }

}
