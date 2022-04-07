using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Projects.Scripts.Data
{
    [Serializable]
    public class StartData
    {
        public int level;
        public int life;
        [SerializeField,ValueDropdown(nameof(AllSkin))]public string currentSkin;

        private List<string> AllSkin()
        {
            return SkinData.Instance.GetAllSkinName();
        }
    }
}