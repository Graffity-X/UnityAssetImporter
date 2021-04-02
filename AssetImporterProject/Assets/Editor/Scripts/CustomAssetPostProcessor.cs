using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Graffity.Editor.AssetImporter
{
    internal class CustomAssetPostProcessor : UnityEditor.AssetPostprocessor
    {
        protected ImporterSettings settings = null;

        public CustomAssetPostProcessor()
        {
            settings = AssetDatabase.LoadAssetAtPath<ImporterSettings>( Path.Combine(Util.ASSET_DIR_PATH, Util.DEFAULT_CONFIG_FILE_NAME));
            // settings?.AudioRules?.Reverse();
            // settings?.TextureRules?.Reverse();
        }



        #region ===== AUDIO =====

        public void OnPostprocessAudio(AudioClip clip)
        {
            if (settings == null)
            {
                return;
            }

            foreach (var rule in settings.AudioRules)
            {
                if (!IsMatchPath(rule, assetImporter.assetPath))
                {
                    continue;
                }

                bool isApplySucceeded = ApplyAudioRule(rule, (AudioImporter)assetImporter);
                if (isApplySucceeded)
                {
                    EditorUtility.SetDirty(clip);
                }
                break;
            }
        }

        protected bool ApplyAudioRule(AudioClipImportRule rule, UnityEditor.AudioImporter importer)
        {
            if (rule == null || importer == null)
            {
                return false;
            }
            var preset = rule.AudioPreset;

            return preset.ApplyTo(importer);
        }
        
        #endregion //) ===== AUDIO =====

        
        
        #region ===== TEXTURE =====

        public void OnPostprocessTexture(Texture2D texture)
        {
            if (settings == null)
            {
                return;
            }
            foreach (var rule in settings.TextureRules)
            {
                if (!IsMatchPath(rule, assetImporter.assetPath))
                {
                    continue;
                }

                bool isApplySucceeded = ApplyTextureRule(rule, (TextureImporter)assetImporter);
                if (isApplySucceeded)
                {        
                    Debug.Log($"{assetImporter.assetPath} is applied rule :{rule.TargetPath}");

                    EditorUtility.SetDirty(texture);
                }
                break;
            }
        }

        protected bool ApplyTextureRule(TextureImportRule rule, UnityEditor.TextureImporter importer)
        {
            if (rule == null || importer == null)
            {
                return false;
            }
            var preset = rule.TexturePreset;

            return preset.ApplyTo(importer);
        }
        #endregion //) ===== TEXTURE =====


        /// <summary>
        /// Check whether target path is matched to rule
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="comparePath"></param>
        /// <returns></returns>
        private static bool IsMatchPath(BaseImportRule rule, string comparePath)
        {
            if (rule == null || string.IsNullOrEmpty(comparePath))
            {
                return false;
            }

            string rulePath = rule.TargetPath.Trim();
            if (string.IsNullOrEmpty(rulePath))
            {
                return false;
            }

            switch (rule.PathType)
            {
                case PathType.Regex:
                {
                    if (rulePath.Contains("*") || rulePath.Contains("?"))
                    {
                        var regex = "^" + Regex.Escape(rulePath).Replace(@"\*", ".*").Replace(@"\?", ".");
                        return Regex.IsMatch(comparePath, regex);
                    }
                    else
                    {
                        return comparePath.StartsWith(rulePath);
                    }
                }
                case PathType.Wildcard: return Regex.IsMatch( comparePath, rulePath);
                
                default: return false;
            }
        }
    }
}