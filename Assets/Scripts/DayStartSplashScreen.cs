using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayStartSplashScreen : MonoBehaviour
{
    [SerializeField] GameObject dayTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimateIntro()
    {
        LeanTween.alpha(dayTxt, 1, 4);
    }
}
