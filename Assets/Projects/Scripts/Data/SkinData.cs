using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Spine.Unity;
using Truongtv.Utilities;
using UnityEngine;

namespace Projects.Scripts.Data
{
    [CreateAssetMenu(fileName = "SkinData", menuName = "truongtv/skinData", order = 0)]
    public class SkinData : SingletonScriptableObject<SkinData>
    {
        [SerializeField] private SkeletonDataAsset skeleton;
        public List<SkinInfo> skinList;
        [ValueDropdown(nameof(GetAllSkinName))]public List<string> licensingSkin;
        public int baseCoinValue;
        public int increaseCoinValue;
        
        public List<SkinInfo> Skins
        {
            get
            {
#if UNITY_IOS||UNITY_IPHONE
                if (GameDataManager.Instance.versionReview.Equals(Application.version))
                {
                    var result  = new List<SkinInfo>(skinList);
                    result.RemoveAll(a => licensingSkin.Contains(a.skinName));
                    return result;
                }
#endif
                return skinList;
            }
        }

        public bool IsSkinPremium(string skinName)
        {
            return Skins.Find(a => a.skinName.Equals(skinName)).unlockType == UnlockType.Iap;
        }
        public List<string> GetAllSkinName()
        {
            var result = new List<string>();
            var totalSkin = skeleton.GetSkeletonData(true).Skins.Items;
            foreach (var skin in totalSkin)
            {
                var skinName = skin.Name.Replace("_1", "");
                skinName = skinName.Replace("_2", "");
                if (!result.Contains(skinName) && !skin.Name.Equals("default"))
                {
                    result.Add(skinName);
                }
                    
            }
            #if UNITY_IOS|| UNITY_IPHONE
            result = result.Except(licensingSkin).ToList();
            #endif
            return result;
        }

        public List<SkinInfo> GetAllPremiumSkin()
        {
            return Skins.FindAll(a=>a.location == LocationType.Premium);
        }

        public List<SkinInfo> GetNormalSkin()
        {
            return Skins.FindAll(a=>a.location == LocationType.Coin);
        }

        public List<SkinInfo> GetSkinByLocation(LocationType location)
        {
            return Skins.FindAll(a=>a.location == location);
        }
        public List<string> GetSkinNameCanBuyDirectly()
        {
            return Skins.FindAll(a=>a.location == LocationType.Rescue).Select(a=>a.skinName).ToList();
        }

        public int GetSkinIndex(string skinName)
        {
            var totalsSkin = GetAllSkinName();
            return totalsSkin.IndexOf(skinName);
        }

        [Button]
        private void Init()
        {
            var item = GetAllSkinName();
            foreach (var i in item)
            {
                skinList.Add(new SkinInfo{skinName = i});

            }
        }
    }
    [Serializable]
    public class SkinInfo
    {
        [ValueDropdown(nameof(GetAllSkinName))]
        public string skinName;
        public UnlockType unlockType;
        [ShowIf("@this.unlockType == UnlockType.Level")]public int unlockValue;
        public LocationType location;
        public List<string> GetAllSkinName()
        {
            return SkinData.Instance.GetAllSkinName();
        }
        
    }
    public enum LocationType
    {
        Premium,Rescue,Coin,SpecialOffer
    }

    public enum UnlockType
    {
        None,Level,Iap,Coin,Star
    }
}