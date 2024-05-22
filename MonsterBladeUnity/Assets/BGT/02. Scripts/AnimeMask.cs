using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeMask : MonoBehaviour
{
    public Animator anime;
    float temp = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (anime.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
        {
            if (temp >= 0)
            {
                temp -= Time.deltaTime;
            }
            anime.SetLayerWeight(1, temp);
        }
    }
}
