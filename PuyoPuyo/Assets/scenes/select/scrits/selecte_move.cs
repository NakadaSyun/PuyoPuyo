using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selecte_move : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] select_Manager SM;
    [SerializeField] GameObject bar;

    float[] height = { 0.9f, -0.9f, -2.3f };
    int old_col;


    private void Start()
    {
        old_col = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ver_move();
    }

    void ver_move()
    {
        int col = SM.Get_num();
        if (old_col == col) return;
        
        if(col <= 2)
        {
            bar.SetActive(true);
            bar.transform.position = mk_pos(col);
        }
        else
        {
            bar.SetActive(false);
        }
        old_col = col;
    }

    Vector3 mk_pos(int num)
    {
        Vector3 pos = bar.transform.position;

        pos.y = height[num];

        return pos;
    }

}
