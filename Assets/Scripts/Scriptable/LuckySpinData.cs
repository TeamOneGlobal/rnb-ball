using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "LuckySpinData", menuName = "Truongtv/data/LuckySpinData", order = 0)]
    public class LuckySpinData : ScriptableObject
    {
        public List<SpinItemData> data;
        
    }

    [Serializable]
    public class SpinItemData
    {
        public ItemType type;
        public int value;
        public bool isFirstReward;
        public bool neverReward;
    }

    public enum ItemType
    {
        Coin,Heart,Skin
    }
}