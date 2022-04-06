using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using Truongtv.Utilities;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "SkinData", menuName = "Truongtv/data/skinData", order = 0)]
    public class SkinData : SingletonScriptableObject<SkinData>
    {
        [SerializeField] private SkeletonDataAsset skeleton;
        public List<SkinInfo> skins;
        public long baseCoinValue;
        public long increaseValue;
        public List<string> GetAllSkinName()
        {
            var result = new List<string>();
            var totalSkin = skeleton.GetSkeletonData(true).Skins.Items;
            foreach (var skin in totalSkin)
            {
                if(!result.Contains(skin.Name.Split('_')[0]))
                    result.Add(skin.Name.Split('_')[0]);
            }
            return result;
        }

        public bool IsSkinPremium(string skinName)
        {
            return skins.Find(a => a.skinName.Equals(skinName)).unlockType == UnlockSkinType.Iap;
        }
       
//        private void InitSkinData()
//        {
//            skins = new List<SkinInfo>();
//            var skinNames = GetAllSkinName();
//            foreach (var skinName in skinNames)
//            {
//                skins.Add(new SkinInfo
//                {
//                    skinName = skinName
//                });
//            }
//        }
    }

    [Serializable]
    public class SkinInfo
    {
        [ValueDropdown(nameof(GetAllSkinName))]
        public string skinName;
        public UnlockSkinType unlockType;
        [ShowIf("@this.unlockType == UnlockSkinType.Level")]public int unlockValue;
        public LocationType location;

        private List<string> GetAllSkinName()
        {
            return SkinData.Instance.GetAllSkinName();
        }
        
    }
    public enum UnlockSkinType{
        Coin,Level,Spin,DailyGift,Iap,None
    }

    public enum LocationType
    {
        Premium,Rescue,Purchase 
    }
}