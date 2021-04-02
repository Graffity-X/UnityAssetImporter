using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Graffity.Editor.AssetImporter
{
    /// <summary>
    /// Config of Path string expression
    /// </summary>
    internal enum PathType
    {
        /// <summary>
        /// With WildCard
        /// *, matches any number of characters
        /// ?, matches a single character
        /// </summary>
        [Tooltip("Simple wildcard.\n\"*\" matches any number of characters.\n\"?\" matches a single character.")]
        Wildcard = 0,

        /// <summary>
        /// Regex pattern
        /// </summary>
        [Tooltip("A regular expression pattern.")]
        Regex
    }
    
    internal class ImporterSettings : ScriptableObject
    {
        [SerializeField, HideInInspector]
        protected List<TextureImportRule> textureRules = new List<TextureImportRule>();
        public List<TextureImportRule> TextureRules => textureRules;
        [SerializeField, HideInInspector]
        protected List<AudioClipImportRule> audioRules = new List<AudioClipImportRule>();
        public List<AudioClipImportRule> AudioRules => audioRules;


        [MenuItem("Assets/Create/Editor/Config/AssetImporter")]
        public static void CreateAsset()
        {
            if (!Directory.Exists(Util.ASSET_DIR_PATH))
            {
                Directory.CreateDirectory(Util.ASSET_DIR_PATH);
            }

            var settings = ScriptableObject.CreateInstance<ImporterSettings>();
            AssetDatabase.CreateAsset( settings, Path.Combine(Util.ASSET_DIR_PATH, Util.DEFAULT_CONFIG_FILE_NAME));
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// AssetImportRule
    /// </summary>
    [Serializable]
    internal class BaseImportRule
    {
        public string TargetPath;
        public PathType PathType = PathType.Wildcard;
        
    }

    [Serializable]
    internal class TextureImportRule: BaseImportRule
    {
        public TextureImporterType textureType = TextureImporterType.Default;
        public TextureImporterShape textureShape = TextureImporterShape.Texture2D;

        public Preset TexturePreset = null;
    }

    /// <summary>
    /// Import Rule for AudioClip
    /// </summary>
    [Serializable]
    internal class AudioClipImportRule : BaseImportRule
    {
        public Preset AudioPreset = null;

    }
}