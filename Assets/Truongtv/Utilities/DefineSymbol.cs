#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Truongtv.Utilities
{
    public static class DefineSymbol
    {
        public const string IN_APP_PURCHASE_SYMBOL = "USING_IN_APP_PURCHASE";
        public const string IAP_SYMBOL = "USING_IAP";
        public const string UDP_SYMBOL = "USING_UDP";
        public const string UNITY_REMOTE_CONFIG_SYMBOL = "USING_UNITY_REMOTE_CONFIG";
        public const string FACEBOOK_SYMBOL = "USING_FACEBOOK";
        public const string AD_SYMBOL = "USING_ADS";
        public const string IRON_SOURCE_SYMBOL = "USING_IRON_SOURCE";
        public const string AD_MOB_SYMBOL = "USING_ADMOB";
        public const string FIREBASE_SYMBOL = "USING_FIREBASE";
        public const string FIREBASE_REMOTE_CONFIG_SYMBOL = "USING_FIREBASE_REMOTE_CONFIG";
        public const string FIREBASE_DATABASE_SYMBOL = "USING_FIREBASE_DATABASE";
        public const string FIREBASE_MESSAGING_SYMBOL = "USING_FIREBASE_MESSAGING";
        public const string FIREBASE_AUTH_SYMBOL = "USING_FIREBASE_AUTH";
        public const string FIREBASE_ANALYTICS_SYMBOL = "USING_FIREBASE_ANALYTICS";
        public const string IN_APP_REVIEW_SYMBOL = "USING_IN_APP_REVIEW";
        public const string ADDRESSABLE_SYMBOL = "USING_ADDRESSABLE";
        private static string GetAllDefineSymbol()
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            return defines;
        }
        public static void UpdateDefineSymbols(List<string> list)
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            defines = list.Aggregate("", (current, variable) => current + variable + ";");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup,defines);
        }
        public static List<string> GetAllDefineSymbols()
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            var list = defines.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            return list;
        }
        public static bool Contain(string value)
        {
            var total = GetAllDefineSymbol();
            return total.Contains(value);
        }
    }
}
#endif