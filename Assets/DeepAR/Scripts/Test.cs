using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum CardType
{
    RedHeart,
    Spade,
    Diamond,
    Club
}

[ExecuteInEditMode]//在非Run时，使UID能在stop时保留值
public class Test : MonoBehaviour
{
    public int UID;
    public string Tag = "";
    public float Rate;
    public bool mSwitch;
    public Vector3 mTranform;
    public CardType cardType = CardType.RedHeart;

    void Awake ()
    {
        Load();
    }
	
	public void Load ()//public才能给editor调
    {
        //SDK中是底层读.dat文件的，unity的TextAsset不支持.dat，要用StreamReader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/markers.txt");
        string strAll = reader.ReadToEnd();

        //解析.dat文件
        string FileContent = strAll;
        string[] Lines = FileContent.Split('\n');//总共多少行，Line[0]为总识别图数量
        UID = int.Parse(Lines[0]);

        int Count = 0;

        for (int i = 1; i < Lines.Length; i++)//过滤第一行识别图数量
        {
            //Debug.Log(Lines[i]);//读取每行内容，为什么非空？
            //if (Lines[i] != null || Lines[i] != " ") //(!string.IsNullOrEmpty(Lines[i]))
            if (Lines[i].ToCharArray().Length > 1)
            {
                Count++;
                Debug.Log(Count);
            }
        }
    }

    [ContextMenu("Unload")]//复位UID
    public void Unload()
    {
        UID = 0;
    }

    public static int getUID(string str)
    {
        int id = int.Parse(str);
        return id;
    }
}
