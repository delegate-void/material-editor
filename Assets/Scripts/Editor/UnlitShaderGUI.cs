using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace MaterialEditors
{
    public class UnlitShaderGUI : ShaderGUI
    {
        #region Properties
    
        private MaterialProperty _mainTex = default;
        private MaterialProperty _ditherTex = default;
    
        #endregion

        #region Property Styles

        private static readonly GUIContent _mainTexStyle = EditorGUIUtility.TrTextContent("Main Texture",
            "Specifies the albedo map.");
        
        private static readonly GUIContent _ditherTexStyle = EditorGUIUtility.TrTextContent("Dither Texture",
            "Specifies the dither texture.");

        #endregion
        
        #region Categories
    
        [System.Flags]
        private enum Category
        {
            General = 1 << 0,
            Advanced = 1 << 1
        }
    
        public static readonly GUIContent CatGeneral = EditorGUIUtility.TrTextContent("General", "General material inputs.");
        public static readonly GUIContent CatAdvanced = EditorGUIUtility.TrTextContent("Advanced", "Advanced material inputs.");
        
        private MaterialHeaderScopeList _categories = new MaterialHeaderScopeList(uint.MaxValue);
        
        #endregion
    
        #region Helper Variables
    
        private MaterialEditor _materialEditor = default;
        private bool _initialised = false;
    
        #endregion
        
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            if (materialEditor == null)
            {
                return;
            }
    
            _materialEditor = materialEditor;
            
            SetProperties(properties);
    
            if (!_initialised)
            {
                Initialise();
                _initialised = true;
            }
            
            _categories.DrawHeaders(materialEditor, materialEditor.target as Material);
        }
    
        private void Initialise()
        {
            _categories.RegisterHeaderScope(CatGeneral, Category.General, DrawGeneralProps);
            _categories.RegisterHeaderScope(CatAdvanced, Category.Advanced, DrawAdvancedProps);
        }
    
        private void SetProperties(MaterialProperty[] properties)
        {
            _mainTex = FindProperty("_MainTex", properties);
            _ditherTex = FindProperty("_DitherTex", properties);
        }
        
        private void DrawGeneralProps(Material material)
        {
            if (_mainTex != null)
            {
                _materialEditor.ShaderProperty(_mainTex, _mainTexStyle);
            }
        }
    
        private void DrawAdvancedProps(Material material)
        {
            if (_ditherTex != null)
            {
                _materialEditor.ShaderProperty(_ditherTex, _ditherTexStyle);
                if (_ditherTex.textureValue != null)
                {
                    material.EnableKeyword("_HAS_DITHER_TEXTURE");
                }
                else
                {
                    material.DisableKeyword("_HAS_DITHER_TEXTURE");
                }
            }
        }
    }
}


