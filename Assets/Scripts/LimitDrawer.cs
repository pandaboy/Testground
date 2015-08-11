using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(LimitAttribute))]
public class LimitDrawer : PropertyDrawer {

	// Use this for initialization
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {

        LimitAttribute limit = attribute as LimitAttribute;

        if (property.propertyType == SerializedPropertyType.Float)
            EditorGUI.Slider(position, property, limit.min, limit.max, label);
        else if (property.propertyType == SerializedPropertyType.Integer)
            EditorGUI.IntSlider(position, property, (int)limit.min, (int)limit.max, label);
        else
            EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
	}
}
