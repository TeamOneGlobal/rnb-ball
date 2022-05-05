using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class BrokenPlatform2 : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeleton;
    [SerializeField, SpineAnimation] private string idleAnim, impactAnim, stayInAnim, appearAnim, disappearAnim;
    [SerializeField] private float stayDuration, hideDuration;
    [SerializeField] private Collider2D collider2D;
        
    private bool stay,isBreak,iStart;

    private void Start()
    {
        collider2D.isTrigger = false;
        skeleton.state.SetAnimation(0, idleAnim, true);
    }

    public void OnImpact()
    {
        if(isBreak) return;
        stay = true;
        if(iStart) return;
        var trackEntry = skeleton.state.SetAnimation(0, impactAnim, false);
        trackEntry.Complete += entry =>
        {
            skeleton.state.SetAnimation(0, stayInAnim, true);
        }; 
        StartCoroutine(Break());
    }

    public void OnStay()
    {
        stay = true;
        if (!iStart && !collider2D.isTrigger)
        {
            StartCoroutine(Break());
        }
    }

    public void OnExit()
    {
        stay = false;
    }

    private IEnumerator Break()
    {
        iStart = true;
        yield return new WaitForSeconds(stayDuration);
        skeleton.state.SetAnimation(0, disappearAnim, false);
        isBreak = true;
        collider2D.isTrigger = true;
        yield return new WaitForSeconds(hideDuration);
        var trackEntry = skeleton.state.SetAnimation(0, appearAnim, false);
        trackEntry.Complete += entry =>
        {
            isBreak = false;
            collider2D.isTrigger = false;
            if(!stay)
                skeleton.state.SetAnimation(0, idleAnim, true);
        };
        iStart = false;
    }
}
