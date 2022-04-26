using System.Collections;
using System.Collections.Generic;
using GamePlay;
using TMPro;
using UnityEngine;

public class LevelBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    void Start()
    {
        levelText.text = "Level " + GamePlayController.Instance.level;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
