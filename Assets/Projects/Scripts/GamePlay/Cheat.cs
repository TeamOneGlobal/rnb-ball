using System;
using System.Collections.Generic;
using Projects.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    public Toggle toggle;
    public List<CanvasGroup> graphs;

    private void Start()
    {
        if(GameDataManager.Instance.cheated)
        toggle.onValueChanged.AddListener(OnValueChange);
    }

    private void OnValueChange(bool value)
    {
        foreach (var graphic in graphs)
        {
            graphic.alpha = value ? 1 : 0;
        }
    }
}
