using System;
using System.Collections.Generic;
using UnityEngine;

namespace Projects.Scripts.Data
{
    [CreateAssetMenu(fileName = "SpinInGame", menuName = "truongtv/spinInGame", order = 1)]
    public class LuckyBonusIngame : ScriptableObject
    {
        public List<LuckyBonusData> spinData;
    }
    [Serializable] 
    public class LuckyBonusData
    {
        public int bonusValue;
    }
}
