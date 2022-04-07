using System;
using System.Collections.Generic;
using Projects.Scripts.Models;
using UnityEngine;

namespace Projects.Scripts.Data
{
    [CreateAssetMenu(fileName = "DailyGiftData", menuName = "truongtv/DailyGiftData", order = 0)]
    public class DailyGiftData : ScriptableObject
    {
        public List<DailyGiftItem> data;
    }
    
    [Serializable]
    public class DailyGiftItem
    {
        public List<ItemData> itemData;
    }
}