using System.Collections.Generic;
using GamePlay.Item;
using Sirenix.OdinInspector;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Platform
{
    public class Water : DamageObject
    {
        [SerializeField, ValueDropdown(nameof(GetAllTriggerTag))]
        private List<string> dieTag;
        [SerializeField] private SimpleAudio simpleAudio;
        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            simpleAudio.Play();
            triggerObject.GetComponent<CharacterController>().SetOnWater(true);
            if(dieTag.Contains(triggerTag))
                DamageTarget(triggerObject);
        }

        protected override void TriggerExit(string triggerTag, Transform triggerObject)
        {
            base.TriggerExit(triggerTag, triggerObject);
            triggerObject.GetComponent<CharacterController>().SetOnWater(false);
        }
    }
}