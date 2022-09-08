using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puyo_anim : MonoBehaviour
{
    [SerializeField] Sprite[] anim_image;
    [SerializeField] Sprite[] destroy_anim_image;
    private int Image_num;
    public bool Is_destroy = false;

    // Start is called before the first frame update
    void Start()
    {
        Image_num = 0;
        StartCoroutine(anim());
    }
    

    IEnumerator anim(float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);

        if (!Is_destroy)
        {
            if (++Image_num >= anim_image.Length)
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

    public IEnumerator destroy_anim(float delay = 0.1f)
    {
        if (!Is_destroy)
        {
            Is_destroy = true;
            Image_num = 0;
        }
        yield return new WaitForSeconds(delay);

        if (++Image_num >= destroy_anim_image.Length)
        {
            //画像一周したら終了
            Destroy(this.gameObject);
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = destroy_anim_image[Image_num];
            StartCoroutine(destroy_anim());
        }
    }
}
