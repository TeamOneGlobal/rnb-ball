using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingFake : MonoBehaviour
{
    public Image imgLoading;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        imgLoading.fillAmount = 0f;
        imgLoading.DOFillAmount(1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
