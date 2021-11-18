using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Graffity.Editor.AssetImporter
{
    internal class CustomAssetPostProcessor : UnityEditor.AssetPostprocessor
    {
        static private ImporterSettings settings = null;
        

        private ImporterSettings FindConfigFile()
        {
            var candidates = AssetDatabase.FindAssets("t:ImporterSettings", null);
            if (candidates == null || candidates.Length < 1)
            {
                Debug.LogError("There is No ImporterSettings");
                return null;
            }

            var paths = candidates.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();

            if (paths.Length != 1)
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("There are more than 2 setting files.");
                sb.AppendLine($"First one (Path:{paths[0]}) will be applied");
                Debug.LogWarning(sb.ToString());
            }
            else
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"Setting File (Path:{paths[0]}) will be applied");
                Debug.Log(sb.ToString());
            }

            return AssetDatabase.LoadAssetAtPath<ImporterSettings>(paths[0]);
        }


        #region ===== AUDIO =====

        public void OnPostprocessAudio(AudioClip clip)
        {
            if (settings == null) settings = FindConfigFile();  
            if (settings == null)
            {
                Debug.LogError("There is No ImporterSettings");
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
            if (settings == null) settings = FindConfigFile();  
            if (settings == null)
            {
                Debug.LogError("There is No ImporterSettings");
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