using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Test), true)]
public class NewEditor : Editor
{
    private SerializedImageTarget mSerializedObject;

    private SerializedProperty mImageTargetType;
    private SerializedProperty mInitializedInEditor;
    private SerializedProperty mPreserveChildSize;
    private SerializedProperty mTrackableName;

    void OnEnable()
    {
        this.mSerializedObject = new SerializedImageTarget(base.serializedObject);

        this.mImageTargetType = base.serializedObject.FindProperty("mImageTargetType");

        this.mInitializedInEditor = this.serializedObject.FindProperty("mInitializedInEditor");
        this.mPreserveChildSize = this.serializedObject.FindProperty("mPreserveChildSize");
        this.mTrackableName = this.serializedObject.FindProperty("mTrackableName");
    }

    private void DrawCloudRecoTargetInspectorUI(bool typeChanged)
    {
        if (typeChanged)
        {

        }
        EditorGUILayout.LabelField("Time: ", System.DateTime.Now.ToString());
        //EditorGUILayout.PropertyField(mImageTargetType, new GUIContent("Preserve child size"), new GUILayoutOption[0]);
    }

    private void DrawPredefinedTargetInspectorUI(bool typeChanged)
    {
        if (typeChanged)
        {
        }
    }

    private void DrawUserDefinedTargetInspectorUI(bool typeChanged)
    {
        if (typeChanged)
        {
        }
        EditorGUILayout.PropertyField(mTrackableName, new GUIContent("Target Name"), new GUILayoutOption[0]);
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        ImageTargetType imageTargetType = this.ImageTargetType;

        bool typeChanged = this.ImageTargetType != imageTargetType;

        if (this.ImageTargetType == ImageTargetType.PREDEFINED)
        {
            this.DrawPredefinedTargetInspectorUI(typeChanged);
        }
        else if (this.ImageTargetType == ImageTargetType.USER_DEFINED)
        {
            this.DrawUserDefinedTargetInspectorUI(typeChanged);
        }
        else
        {
            this.DrawCloudRecoTargetInspectorUI(typeChanged);
        }

    }

    public ImageTargetType ImageTargetType
    {
        get
        {
            return (ImageTargetType)this.mImageTargetType.enumValueIndex;
        }
        set
        {
            this.mImageTargetType.enumValueIndex = (int)value;
        }
    }
}