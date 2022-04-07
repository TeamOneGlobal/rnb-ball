using System;
using System.Collections.Generic;
using Projects.Scripts.Models;
using UnityEngine;

namespace Projects.Scripts.Data
{
    [CreateAssetMenu(fileName = "SpinData", menuName = "truongtv/spinData", order = 0)]
    public class LuckySpinData : ScriptableObject
    {
        public List<SpinItemData> spinData;
    }
    [Serializable]
    public class SpinItemData
    {
        public ItemData itemData;
        public bool isFirstReward;
        public bool isSkin;
        
    }
    
    
    
}