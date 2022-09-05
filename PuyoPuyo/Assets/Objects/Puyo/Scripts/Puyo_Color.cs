using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puyo_Color : MonoBehaviour
{
    public enum puyo_color {BLUE,GRAY,GREEN,PURPLE,RED,YERROW }

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
