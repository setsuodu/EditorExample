using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SerializedTrackables
{
    private readonly SerializedProperty mInitializedInEditor;
    private readonly SerializedProperty mPreserveChildSize;
    protected readonly SerializedObject mSerializedObject;
    private readonly SerializedProperty mTrackableName;

    public SerializedTrackables(SerializedObject target)
    {
        this.mSerializedObject = target;
        this.mTrackableName = this.mSerializedObject.FindProperty("mTrackableName");
        this.mPreserveChildSize = this.mSerializedObject.FindProperty("mPreserveChildSize");
        this.mInitializedInEditor = this.mSerializedObject.FindProperty("mInitializedInEditor");
    }

    public List<GameObject> GetGameObjects()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (UnityEngine.Object obj2 in this.mSerializedObject.targetObjects)
        {
            list.Add(((MonoBehaviour)obj2).gameObject);
        }
        return list;
    }

    public void SetMaterial(Material material)
    {
        foreach (UnityEngine.Object obj2 in this.mSerializedObject.targetObjects)
        {
            ((MonoBehaviour)obj2).GetComponent<Renderer>().sharedMaterial = material;
        }
    }

    public void SetMaterial(Material[] materials)
    {
        foreach (UnityEngine.Object obj2 in this.mSerializedObject.targetObjects)
        {
            ((MonoBehaviour)obj2).GetComponent<Renderer>().sharedMaterials = materials;
        }
    }

    public bool InitializedInEditor
    {
        get
        {
            return this.mInitializedInEditor.boolValue;
        }
        set
        {
            this.mInitializedInEditor.boolValue = value;
        }
    }

    public SerializedProperty InitializedInEditorProperty()
    {
        return this.mInitializedInEditor;
    }

    public bool PreserveChildSize
    {
        get
        {
            return this.mPreserveChildSize.boolValue;
        }
        set
        {
            this.mPreserveChildSize.boolValue = value;
        }
    }

    public SerializedProperty PreserveChildSizeProperty()
    {
        return this.mPreserveChildSize;
    }

    public UnityEditor.SerializedObject SerializedObject()
    {
        return this.mSerializedObject;
    }

    public string TrackableName
    {
        get
        {
            return this.mTrackableName.stringValue;
        }
        set
        {
            this.mTrackableName.stringValue = value;
        }
    }

    //public SerializedProperty TrackableNameProperty => this.mTrackableName;
}

