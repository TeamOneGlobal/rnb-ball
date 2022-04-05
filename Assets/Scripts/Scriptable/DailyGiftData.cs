using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "DailyGiftData", menuName = "Truongtv/data/DailyGiftData", order = 0)]
    public class DailyGiftData : ScriptableObject
    {
        public List<DailyGift> data;
    }

    [Serializable]
    public class Gift
    {
        public ItemType type;
        public int value;
    }
    [Serializable]
    public class DailyGift
    {
        public List<Gift> gifts;
    }
    
}