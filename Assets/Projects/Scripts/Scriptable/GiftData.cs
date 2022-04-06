using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "GiftData", menuName = "Truongtv/data/GiftData", order = 0)]
    public class GiftData : ScriptableObject
    {
        public List<GiftInfo> giftList;
    }

    [Serializable]
    public class GiftInfo
    {
        public string skinName;
        public bool isSpecial;
        public int unlockValue;
    }
}