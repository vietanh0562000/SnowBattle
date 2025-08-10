using UnityEngine;
using UnityEditor;
 
 
[CustomPropertyDrawer (typeof (PolarCoords))]
public class PolarCoordsDrawer : PropertyDrawer 
{ 
    // TODO: this is sort of like, but not exactly like the X positional handling of Vector3
    public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {

        float nameWidth = pos.width;// * .41f;
 
        float labelWidth = 12;//200f;
        float fieldWidth = (pos.width - 60) / 3f;//((pos.width - nameWidth) / 3f) - labelWidth;
 
        SerializedProperty radius = prop.FindPropertyRelative ("radius");
        SerializedProperty angle = prop.FindPropertyRelative ("angle");
        SerializedProperty elevation = prop.FindPropertyRelative ("elevation");
 
        float posx = pos.x;
 
        int indent = EditorGUI.indentLevel;
 
       // EditorGUI.LabelField (new Rect (pos.x, pos.y, nameWidth, pos.height), prop.displayName); posx += nameWidth;
 
        // Draw Angle
        EditorGUI.LabelField (new Rect (posx, pos.y, labelWidth, pos.height), "R"); posx += labelWidth;
        radius.floatValue = EditorGUI.FloatField (new Rect (posx, pos.y, fieldWidth, pos.height), radius.floatValue);  posx += fieldWidth;

        EditorGUI.LabelField(new Rect(posx, pos.y, labelWidth, pos.height), "A"); posx += labelWidth;
        angle.floatValue = EditorGUI.FloatField(new Rect(posx, pos.y, fieldWidth, pos.height), angle.floatValue); posx += fieldWidth;

        EditorGUI.LabelField(new Rect(posx, pos.y, labelWidth, pos.height), "E"); posx += labelWidth;
        elevation.floatValue = EditorGUI.FloatField(new Rect(posx, pos.y, fieldWidth, pos.height), elevation.floatValue); posx += fieldWidth;

        EditorGUI.indentLevel = indent;
    }
}