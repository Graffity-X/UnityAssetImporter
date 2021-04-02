using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Graffity.Editor.AssetImporter
{
    internal class ReordableListInfo : IDisposable
    {
        private ReorderableList m_list = null;
        internal ReorderableList Reorderables => m_list;
        private SerializedProperty myProperty = null;
        internal SerializedProperty MyProperty => myProperty;

        internal Vector2 scrollPos;
        public ReordableListInfo(SerializedObject so, string key, string listLabel, float heightRatio)
        {
            myProperty = so.FindProperty(key);
            Assert.IsNotNull(myProperty, key+" property is NULL");
            m_list = new ReorderableList(so, myProperty);
            m_list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, listLabel); };
            m_list.elementHeightCallback = (int index) => EditorGUIUtility.singleLineHeight * heightRatio;
            
            m_list.onAddCallback = list =>
            {
                myProperty.arraySize++;
            };
            m_list.onRemoveCallback = delegate(ReorderableList list)
            {
                if (list.index < 0 || list.count <= list.index)
                {
                    return;
                }
                myProperty.DeleteArrayElementAtIndex( list.index);
            };
        }

        public void Dispose()
        {
            m_list = null;
            myProperty = null;
        }
    }
    
    [CustomEditor(typeof(ImporterSettings))]
    [CanEditMultipleObjects]
    public class ImporterSettingsEditor : UnityEditor.Editor
    {
        private const float LIST_ITEM_HEIGHT_RATIO = 4.5f;
        private static readonly string LABEL_LIST_HEADER_AUDIO_LIST = "The top item is the highest Priority";
        private static readonly string LABEL_LIST_HEADER_TEXTURE_LIST = "The top item is the highest Priority";

        private bool isFoldAudioRules = false;
        private bool isFoldTextureRules = false;

        private ReordableListInfo audioRuleListInfo = null;
        private ReordableListInfo textureRuleListInfo = null;

        protected  void OnEnable()
        {
            audioRuleListInfo = new ReordableListInfo(serializedObject, "audioRules", LABEL_LIST_HEADER_AUDIO_LIST, LIST_ITEM_HEIGHT_RATIO);
            audioRuleListInfo.Reorderables.drawElementCallback = DrawAudioRuleItem;

            textureRuleListInfo = new ReordableListInfo(serializedObject, "textureRules", LABEL_LIST_HEADER_TEXTURE_LIST, LIST_ITEM_HEIGHT_RATIO);
            textureRuleListInfo.Reorderables.drawElementCallback = DrawTextureRuleItem;

        }
        protected  void OnDisable()
        {
            audioRuleListInfo?.Dispose();
            audioRuleListInfo = null;
            textureRuleListInfo?.Dispose();
            textureRuleListInfo = null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            {
                DrawAudioRule();
                DrawTextureRule();
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        
        private void DrawAudioRule()
        {
            EditorGUILayout.LabelField("Category:Audio");
            isFoldAudioRules = EditorGUILayout.Foldout(isFoldAudioRules, "Rules");
            if (isFoldAudioRules)
            {
                DrawList(audioRuleListInfo);
            }
        }
        private void DrawTextureRule()
        {
            EditorGUILayout.LabelField("Category:Texture");
            isFoldTextureRules = EditorGUILayout.Foldout(isFoldTextureRules, "Rules");
            if (isFoldTextureRules)
            {
                DrawList(textureRuleListInfo);
            }
        }
        
        
        void DrawList(ReordableListInfo info)
        {
            if( info?.Reorderables == null || info.MyProperty == null)
            {
                EditorGUILayout.HelpBox($"表示するリストがありません(List is null ?:{info?.Reorderables == null}, EnumList is null ? :{info.MyProperty == null}", MessageType.Warning);
                return;
            }
            using (var scrollView = new EditorGUILayout.ScrollViewScope(info.scrollPos))
            {
                info.scrollPos = scrollView.scrollPosition;

                info?.Reorderables?.DoLayoutList();
            }
        }
        
        private void DrawAudioRuleItem(Rect rect, int index, bool isActive, bool isFocused)
        {
            Assert.IsNotNull(audioRuleListInfo?.MyProperty, "audioRuleListInfo?.MyProperty is null");
            // Tag 表記
            var p = audioRuleListInfo?.MyProperty?.GetArrayElementAtIndex(index);
            if (p == null)
            {
                return;
            }
            var itemRect = new Rect(rect);
            itemRect.height = EditorGUIUtility.singleLineHeight * 1.5f;
            EditorGUI.LabelField(itemRect, $"Item:{(index+1)}");
            itemRect.y += itemRect.height;
            itemRect.height = EditorGUIUtility.singleLineHeight;
            
            DrawBaseRule(p, ref itemRect);

            string presetKey = string.Intern("AudioPreset");
            EditorGUI.PropertyField(itemRect, p.FindPropertyRelative(presetKey) );
        }
        private void DrawTextureRuleItem(Rect rect, int index, bool isActive, bool isFocused)
        {
            Assert.IsNotNull(textureRuleListInfo?.MyProperty, "textureRuleListInfo?.MyProperty is null");
            // Tag 表記
            var p = textureRuleListInfo?.MyProperty?.GetArrayElementAtIndex(index);
            if (p == null)
            {
                return;
            }
            var itemRect = new Rect(rect);
            itemRect.height = EditorGUIUtility.singleLineHeight * 1.5f;
            EditorGUI.LabelField(itemRect, $"Item:{(index+1)}");
            itemRect.y += itemRect.height;
            itemRect.height = EditorGUIUtility.singleLineHeight;

            DrawBaseRule(p, ref itemRect);
            string presetKey = string.Intern("TexturePreset");
            EditorGUI.PropertyField(itemRect, p.FindPropertyRelative(presetKey) );
        }

        private void DrawBaseRule(SerializedProperty property, ref Rect rect)
        {
            if (property == null)
            {
                return;
            }
            string pathKey = string.Intern("TargetPath");
            string pathTypeKey = string.Intern("PathType");
            string path = property.FindPropertyRelative(pathKey).stringValue;
            int pathTypeId = property.FindPropertyRelative(pathTypeKey).intValue;

            PathType pathType = Util.ConvertToEnum<PathType>(pathTypeId);
            pathType = (PathType)EditorGUI.EnumPopup(rect,"Path Type" ,pathType);
            pathTypeId = Util.ConvertToInt(pathType);
            rect.y += EditorGUIUtility.singleLineHeight;

            path = EditorGUI.TextField(rect, "Path", path);
            rect.y += EditorGUIUtility.singleLineHeight;
            
            property.FindPropertyRelative(pathKey).stringValue = path;
            property.FindPropertyRelative(pathTypeKey).intValue = pathTypeId;

        }
        
        
        
        protected void DrawSaveButton()
        {
            if (GUILayout.Button("Save"))
            {
                if (IsValid() )
                {
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(target);
                    EditorUtility.DisplayDialog("Success", "保存に成功しました", "ok");
                }
            }
        }

        /// <summary>
        /// SaveButton実行用のバリデーション
        /// </summary>
        /// <returns></returns>
        protected bool IsValid()
        {
            return true;
        }
    }
}