using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

namespace GamePlay.Platform
{
    public class HiddenMap : ObjectTrigger
    {
        [SerializeField] private SpriteShapeRenderer renderder;

        protected override void TriggerEnter(string triggerTag, Transform triggerObject)
        {
            base.TriggerEnter(triggerTag, triggerObject);
            var alpha = 0f;
            DOTween.To(()=> renderder.color.a, setter: x => alpha = x, 0,0.5f)
                .OnUpdate(() =>
                {
                    var color = renderder.color;
                    color.a = alpha;
                    renderder.color = color;
                });
        }

        protected override void TriggerExit(string triggerTag, Transform triggerObject)
        {
            base.TriggerExit(triggerTag, triggerObject);
            var alpha = 1f;
            DOTween.To(()=> renderder.color.a, x => alpha = x, 1,0.5f)
                .OnUpdate(() =>
                {
                    var color = renderder.color;
                    color.a = alpha;
                    renderder.color = color;
                });
        }
    }
}