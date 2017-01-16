using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public enum SetScale
{
    FromWidth,
    FromHeight
}

[ExecuteInEditMode]
public class Data : MonoBehaviour
{
    public int index;
    public Material mMaterial;
    public Texture2D mTexture;
    public List<Texture2D> mTextureList = new List<Texture2D>();
    public List<string> mTextureName = new List<string>();
    public string[] _choices;

    public SetScale setScale = SetScale.FromWidth;
    public int _width, _height;
    public float m_Width = 1.0f, m_Height = 1.0f;
    MeshFilter filter;
    MeshRenderer meshRender;

    void Start()
    {
        ReadDat();
        MeshBuilder();
        GetImageSize(mTexture, out _height, out _width);
        //string assetPath = AssetDatabase.GetAssetPath(mTexture);
        //Debug.Log("assetPath : " + assetPath);
    }

    void Update()
    {
        //刷新长宽，待改进，不应该放在Update()
        switch (setScale)
        {
            case SetScale.FromWidth:
                SetScaleFromWidth();
                break;
            case SetScale.FromHeight:
                SetScaleFromHeight();
                break;
        }
        UpdateMesh();
        GetImageSize(mTexture, out _height, out _width);
    }

    private List<Vector3> m_Vertices = new List<Vector3>();
    public List<Vector3> Vertices { get { return m_Vertices; } }

    private List<Vector3> m_Normals = new List<Vector3>();
    public List<Vector3> Normals { get { return m_Normals; } }

    private List<Vector2> m_UVs = new List<Vector2>();
    public List<Vector2> UVs { get { return m_UVs; } }

    private List<int> m_indexs = new List<int>();

    Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = m_Vertices.ToArray();
        mesh.triangles = m_indexs.ToArray();

        if (m_Normals.Count == m_Vertices.Count)
            mesh.normals = m_Normals.ToArray();

        if (m_UVs.Count == m_Vertices.Count)
            mesh.uv = m_UVs.ToArray();

        mesh.RecalculateBounds();

        return mesh;
    }

    void AddTriangle(int index0, int index1, int index2)
    {
        m_indexs.Add(index0);
        m_indexs.Add(index1);
        m_indexs.Add(index2);
    }

    //创建面，材质球，贴图
    void MeshBuilder()
    {
        //Set up the vertices and triangles:
        Vertices.Add(new Vector3(-m_Width / 2, 0.0f, -m_Height / 2));
        UVs.Add(new Vector2(0.0f, 0.0f));
        Normals.Add(Vector3.up);

        Vertices.Add(new Vector3(-m_Width / 2, 0.0f, m_Height / 2));
        UVs.Add(new Vector2(0.0f, 1.0f));
        Normals.Add(Vector3.up);

        Vertices.Add(new Vector3(m_Width / 2, 0.0f, m_Height / 2));
        UVs.Add(new Vector2(1.0f, 1.0f));
        Normals.Add(Vector3.up);

        Vertices.Add(new Vector3(m_Width / 2, 0.0f, -m_Height / 2));
        UVs.Add(new Vector2(1.0f, 0.0f));
        Normals.Add(Vector3.up);

        AddTriangle(0, 1, 2);
        AddTriangle(0, 2, 3);

        if (!GetComponent<MeshFilter>() || !GetComponent<MeshRenderer>())
        {
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
        }
        filter = GetComponent<MeshFilter>();
        meshRender = GetComponent<MeshRenderer>();

        if (filter != null)
        {
            filter.sharedMesh = CreateMesh();//Mesh与sharedMesh区别，是否全局改变
        }

        //Shader shader = Shader.Find("Unlit/Texture");//对应shader索引
        //mMaterial = new Material(shader);
        mMaterial = AssetDatabase.LoadAssetAtPath("Assets/Editor/Materials/MyMaterial.mat", typeof(Material)) as Material;
        meshRender.sharedMaterial = Instantiate(mMaterial) as Material;
        //mTexture = AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/Miku.png", typeof(Texture2D)) as Texture2D;
        mMaterial.mainTexture = mTexture;
    }

    [ContextMenu("UpdateMesh")]
    void UpdateMesh()
    {
        Mesh _mesh = GetComponent<MeshFilter>().sharedMesh;
        Vertices.Clear();
        Vertices.Add(new Vector3(-m_Width / 2, 0.0f, -m_Height / 2));
        Vertices.Add(new Vector3(-m_Width / 2, 0.0f, m_Height / 2));
        Vertices.Add(new Vector3(m_Width / 2, 0.0f, m_Height / 2));
        Vertices.Add(new Vector3(m_Width / 2, 0.0f, -m_Height / 2));
        _mesh.vertices = m_Vertices.ToArray();
        _mesh.RecalculateBounds();
    }

    public static bool GetImageSize(Texture2D asset, out int width, out int height)
    {
        if (asset != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer != null)
            {
                object[] args = new object[2] { 0, 0 };
                MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(importer, args);

                width = (int)args[0];
                height = (int)args[1];

                return true;
            }
        }
        height = width = 0;
        return false;
    }

    //以宽为准缩放
    void SetScaleFromWidth()
    {
        m_Width = 1.0f;
        m_Height = AspectRaito(_width, _height);
    }

    //以高为准缩放
    void SetScaleFromHeight()
    {
        m_Height = 1.0f;
        m_Width = 1 / AspectRaito(_width, _height);
    }

    //原始图片宽高比例
    float AspectRaito(float width, float height)
    {
        float aspect = width / height;
        return aspect;
    }

    public void ReadDat()
    {
        mTextureList.Clear();
        mTextureName.Clear();
        //SDK中是底层读.dat文件的，unity的TextAsset不支持.dat，要用StreamReader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/markers.dat");
        string strAll = reader.ReadToEnd();

        //解析.dat文件
        string FileContent = strAll;
        string[] Lines = FileContent.Split('\n');//总共多少行，Line[0]为总识别图数量
        string[] NewLines = new string[Lines.Length]; ;

        //int Count = 0;
        
        for (int i = 1; i < Lines.Length; i++)//过滤第一行识别图数量
        {
            NewLines[i] = Lines[i].Replace("\r", "").Replace("\n", "");
            //if (NewLines[i] != null && NewLines[i] != "")
            if(!string.IsNullOrEmpty(NewLines[i]))
            {
                if (NewLines[i] == CardType.RedHeart.ToString())
                {
                    Debug.Log("No : " + i + " Type : " + NewLines[i]);
                }
                else
                {
                    Debug.Log("No : " + i + " TargetTexture : " + NewLines[i]);
                    mTexture = AssetDatabase.LoadAssetAtPath("Assets/Editor/Textures/" + NewLines[i] + ".jpg", typeof(Texture2D)) as Texture2D;
                    //Debug.Log("Assets/Editor/Textures/" + NewLines[i] + ".png");
                    mTextureList.Add(mTexture);
                    mTextureName.Add(NewLines[i]);
                }
            }
        }
        //Debug.Log(mTextureName.ToArray().Length);
        _choices = mTextureName.ToArray();
    }
}
