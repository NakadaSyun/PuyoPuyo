using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puyo_Color : MonoBehaviour
{
    public enum puyo_color
    {
        BLUE = 1,
        GRAY = 2,
        GREEN = 3,
        PURPLE = 4,
        RED = 5,
        YERROW = 6
    }

    [SerializeField] puyo_color _Color;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int Get_Color()
    {
        return (int)_Color;
    }
}
