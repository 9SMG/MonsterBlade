using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeSync : MonoBehaviour
{
    public Animator anime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        anime.SetLayerWeight(1, 0);

    }
}
