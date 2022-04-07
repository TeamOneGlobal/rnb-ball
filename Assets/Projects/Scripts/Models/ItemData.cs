using System;
using System.Collections.Generic;
using Projects.Scripts.Data;
using Sirenix.OdinInspector;

namespace Projects.Scripts.Models
{
    [Serializable]
    public class ItemData
    {
        public int coinValue;
        public int lifeValue;
        [ValueDropdown(nameof(GetAllSkinName))]public string skinName;

        private List<string> GetAllSkinName()
        {
            return SkinData.Instance.GetAllSkinName();
        }
    }
}