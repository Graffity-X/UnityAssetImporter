
using System;

namespace Graffity.Editor.AssetImporter
{
    /// <summary>
    /// Utility method & Const value definition
    /// </summary>
    public static class Util
    {
        public const string ASSET_DIR_PATH = "Assets/Editor/CustomAssetImporter/";
        public const string DEFAULT_CONFIG_FILE_NAME = "CustomImporter.asset";
        
        
        
        
        public static TEnum ConvertToEnum<TEnum>(int value) where TEnum : Enum
        {
            return (TEnum) (object) value;
        }

        public static int ConvertToInt<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            return Convert.ToInt32(enumValue);
        }
    }
}