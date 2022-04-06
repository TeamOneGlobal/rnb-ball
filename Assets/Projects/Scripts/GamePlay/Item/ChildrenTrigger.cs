using System;
using UnityEngine;

namespace GamePlay.Item
{
    public class ChildrenTrigger : ObjectTrigger
    {
        public Action<string,Transform> onTriggerEnter, ontriggerExit,ontriggerStay;
        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            onTriggerEnter?.Invoke(triggerTag, triggerObject);
        }

        protected override void TriggerExit(string triggerTag, Transform triggerObject)
        {
            base.TriggerExit(triggerTag, triggerObject);
            ontriggerExit?.Invoke(triggerTag, triggerObject);
        }

        protected override void TriggerStay(string triggerTag, Transform triggerObject)
        {
            base.TriggerStay(triggerTag, triggerObject);
            ontriggerStay?.Invoke(triggerTag, triggerObject);
        }
    }
}