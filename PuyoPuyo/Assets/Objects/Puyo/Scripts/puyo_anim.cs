using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puyo_anim : MonoBehaviour
{
    [SerializeField] Sprite[] anim_image;
    private int Image_num;

    // Start is called before the first frame update
    void Start()
    {
        Image_num = 0;
        StartCoroutine(anim());
    }
    

    IEnumerator anim(float delay = 0.07f)
    {
        yield return new WaitForSeconds(delay);

        if(++Image_num >= anim_image.Length)
        {
            Image_num = 0;
            StartCoroutine(anim(0.8f));
        }
        else
        {

            StartCoroutine(anim());
        }

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = anim_image[Image_num];


    }
}
