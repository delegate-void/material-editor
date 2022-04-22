using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MaterialUnitDrawer : MaterialPropertyDrawer
{
    private enum Mode
    {
        Unknown,
        Deg2Rad,
        Celsius2Fahrenheit
    }

    private Mode _mode = Mode.Unknown;
    
    public MaterialUnitDrawer(string mode)
    {
        foreach (var candidate in Enum.GetValues(typeof(Mode)))
        {
            if (candidate.ToString() == mode)
            {
                _mode = (Mode) candidate;
            }
        }
    }
    
    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
        if (prop.type != MaterialProperty.PropType.Float || _mode == Mode.Unknown)
        {
            return;
        }

        var storedVal = prop.floatValue;
        var displayValue = GetDisplayValue(storedVal);
        var valueToStore = EditorGUI.FloatField(position, label, displayValue);
        if (Mathf.Abs(displayValue - valueToStore) > float.Epsilon)
        {
            prop.floatValue = GetStorageValue(valueToStore);
        }
    }

    private float GetDisplayValue(float val)
    {
        switch (_mode)
        {
            case Mode.Deg2Rad:
                return Mathf.Rad2Deg * val;
            case Mode.Celsius2Fahrenheit:
                return (val - 32f) * 5f / 9f;
        }

        return val;
    }
    
    private float GetStorageValue(float val)
    {
        switch (_mode)
        {
            case Mode.Deg2Rad:
                return Mathf.Deg2Rad * val;
            case Mode.Celsius2Fahrenheit:
                return (val * 9f / 5f) + 32f;
        }

        return val;
    }
}
