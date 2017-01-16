/*
通过各种类型在Editor中设置，初步熟悉作用
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();//开始垂直组

        Test m = (Test)target; //target指向Test
        if (m == null) return;

        m.mTranform = EditorGUILayout.Vector3Field("位置", m.mTranform);
        m.Tag = EditorGUILayout.TextField("Marker tag", m.Tag);
        EditorGUILayout.LabelField("UID", (m.UID == 0 ? "Not Loaded" : m.UID.ToString()));//第二个参数需要是可选择的，判断，静态容器等
        //EditorGUILayout.LabelField("Description", Test.TypeNames[m.cardType]);

        m.Rate = EditorGUILayout.Slider("Rate:", m.Rate, 1.0f, 30.0f); //对应float值，重新赋予名字和GUI

        //EditorGUILayout.BeginHorizontal();//开始水平组，取消注释查看显示效果
        m.mSwitch = EditorGUILayout.Toggle("Switch:", m.mSwitch); //对应bool值，重新赋予名字和GUI

        CardType t = (CardType)EditorGUILayout.EnumPopup("Type", m.cardType);//发现不能更换，需要继续执行下面if
        
        if (m.cardType != t) // Reload on change.
        {
            m.Unload();
            m.cardType = t;
            m.Load();
        }

        //EditorGUILayout.EndHorizontal();//结束水平组

        EditorGUILayout.EndVertical();//结束垂直组
        EditorGUILayout.Separator();//回车，空一行
    }

}
