using UnityEngine;
using CharacterController = GamePlay.Characters.CharacterController;

namespace GamePlay.Item
{
    public class Spike : DamageObject
    {
        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            DamageTarget(triggerObject);
        }
    }
}