using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(New), true)]
public class NewEditor : Editor
{
    private SerializedImageTarget mSerializedObject;
    private SerializedProperty mImageTargetType;
    private SerializedProperty mTrackableName;

    void OnEnable()
    {
        this.mSerializedObject = new SerializedImageTarget(this.serializedObject);
        this.mImageTargetType = base.serializedObject.FindProperty("mImageTargetType");
        this.mTrackableName = this.serializedObject.FindProperty("mTrackableName");
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        ImageTargetType imageTargetType = this.mSerializedObject.ImageTargetType;

        bool typeChanged = this.mSerializedObject.ImageTargetType != imageTargetType;

        if (this.mSerializedObject.ImageTargetType == ImageTargetType.PREDEFINED)
        {
            this.DrawPredefinedTargetInspectorUI(typeChanged);
        }
        else if (this.mSerializedObject.ImageTargetType == ImageTargetType.USER_DEFINED)
        {
            this.DrawUserDefinedTargetInspectorUI(typeChanged);
        }
        else
        {
            this.DrawCloudRecoTargetInspectorUI(typeChanged);
        }

        base.serializedObject.ApplyModifiedProperties();
    }

    private void DrawCloudRecoTargetInspectorUI(bool typeChanged)
    {
        if (typeChanged)
        {

        }
        EditorGUILayout.LabelField("Time: ", System.DateTime.Now.ToString());
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