using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Data))]
public class DataEditor : Editor
{
    int _choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        Data m = target as Data;
        if (m == null) return;

        // Draw the default inspector
        //DrawDefaultInspector(); //调试时开启，可显示所有public元素

        // 最终暴露在Inspector上的只有EditorGUILayout的内容
        //_choiceIndex = EditorGUILayout.Popup(_choiceIndex, m._choices);
        _choiceIndex = EditorGUILayout.Popup("Enum", m.index, m._choices);
        SetScale t = (SetScale)EditorGUILayout.EnumPopup("Type", m.setScale);//发现不能更换，需要继续执行下面if
        if (m.setScale != t) // Reload on change.
        {
            m.setScale = t;
        }

        // Update the selected choice in the underlying object
        m.mTexture = m.mTextureList[_choiceIndex];

        //material导致泄露，要用sharedMaterial
        //error : Instantiating material due to calling renderer.material during edit mode. This will leak materials into the scene. You most likely want to use renderer.sharedMaterial instead.
        m.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = m.mTexture;
        m.index = _choiceIndex;//保存当前选择，否则每次重新激活时会退回0

        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }
}
