using System.Collections.Generic;
using Truongtv.SoundManager;
using UnityEngine;

namespace GamePlay.Item
{
   
    public class Saw : DamageObject
    {
        [SerializeField] private SimpleAudio audio;
        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            DamageTarget(triggerObject);
        }
       
    }
}