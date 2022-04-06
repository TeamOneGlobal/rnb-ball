using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Truongtv.Utilities
{
    public class AutoGarbageCollect : MonoBehaviour
    {
        private const float DURATION = 3f;

        private void Start()
        {
            GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
            InvokeRepeating(nameof(AutoCollect),0f,DURATION);
        }
        private static void AutoCollect()
        {
            GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
            GC.Collect();
            GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
        }
    }
}