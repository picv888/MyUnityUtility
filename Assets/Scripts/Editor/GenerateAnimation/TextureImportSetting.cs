/// <summary>
/// 批量图片资源导入设置
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEditor;
public class TextureImportSetting : EditorWindow {

    /// <summary>
    /// 临时存储int[]
    /// </summary>
    int[] IntArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
    //AnisoLevel
    int AnisoLevel = 1;
    //Filter Mode
    int FilterModeInt = 1;
    string[] FilterModeString = new string[] { "Point", "Bilinear", "Trilinear" };
    //Wrap Mode
    int WrapModeInt = 1;
    string[] WrapModeString = new string[] { "Repeat", "Clamp" };
    //Texture Type
    int TextureTypeInt = 3;
    string[] TextureTypeString = new string[] { "Texture", "Normal Map", "GUI", "Sprite", "Cursor", "Cookie", "Lightmap", "SingleChannel" };
    //Max Size
    int MaxSizeInt = 6;
    string[] MaxSizeString = new string[] { "32", "64", "128", "256", "512", "1024", "2048", "4096" };
    //Compress
    int compressInt = 0;
    string[] compressString = new string[] { "None", "Low Quality", "Normal Quality", "High Quality" };
    //pivot
    Vector2 pivot = new Vector2(0.5f, 0.5f);
    //是否显示帮助
    static bool IsHelp = false;
    /// <summary>
    /// 创建、显示窗体
    /// </summary>
    public static void Init() {
        TextureImportSetting window = (TextureImportSetting)EditorWindow.GetWindow(typeof(TextureImportSetting), true, "批量设置Texture");
        window.Show();
    }

    /// <summary>
    /// 显示窗体里面的内容
    /// </summary>
    void OnGUI() {
        //AnisoLevel
        GUILayout.BeginHorizontal();
        GUILayout.Label("Aniso Level  ");
        AnisoLevel = EditorGUILayout.IntSlider(AnisoLevel, 0, 9);
        GUILayout.EndHorizontal();
        //Filter Mode
        FilterModeInt = EditorGUILayout.IntPopup("Filter Mode", FilterModeInt, FilterModeString, IntArray);
        //Wrap Mode
        WrapModeInt = EditorGUILayout.IntPopup("Wrap Mode", WrapModeInt, WrapModeString, IntArray);
        //Texture Type
        TextureTypeInt = EditorGUILayout.IntPopup("Texture Type", TextureTypeInt, TextureTypeString, IntArray);
        //Max Size
        MaxSizeInt = EditorGUILayout.IntPopup("Max Size", MaxSizeInt, MaxSizeString, IntArray);
        //Compress
        compressInt = EditorGUILayout.IntPopup("Compress", compressInt, compressString, IntArray);
        //pivot
        pivot = EditorGUILayout.Vector2Field("Pivot", pivot);
        if (GUILayout.Button("Set Texture ImportSettings"))
            LoopSetTexture();
        //帮助按钮
        if (GUILayout.Button("Help")) {
            if (IsHelp) {
                IsHelp = false;
            }
            else {
                IsHelp = true;
            }
        }

        if (IsHelp) {
            GUILayout.TextArea("批量图片资源导入设置\n使用说明： \n1.在Project窗口选择需要批量设置的贴图，\n2.设置要修改的参数，\n3.点击Set Texture ImportSettings，\n4.稍等片刻，--批量设置成功。");
        }
    }

    /// <summary>
    /// 获取贴图设置
    /// </summary>
    public TextureImporter GetTextureSettings(string path) {
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        //AnisoLevel
        textureImporter.anisoLevel = AnisoLevel;
        //Filter Mode
        switch (FilterModeInt) {
            case 0:
                textureImporter.filterMode = FilterMode.Point;
                break;
            case 1:
                textureImporter.filterMode = FilterMode.Bilinear;
                break;
            case 2:
                textureImporter.filterMode = FilterMode.Trilinear;
                break;
        }
        //Wrap Mode
        switch (WrapModeInt) {
            case 0:
                textureImporter.wrapMode = TextureWrapMode.Repeat;
                break;
            case 1:
                textureImporter.wrapMode = TextureWrapMode.Clamp;
                break;
        }
        //Texture Type

        /*
         Default,
        NormalMap,
        GUI,
        Sprite = 8,
        Cursor = 7,
        Cookie,
        Lightmap = 6,
        SingleChannel = 10
         */
        switch (TextureTypeInt) {
            case 0:
                textureImporter.textureType = TextureImporterType.Default;
                break;
            case 1:
                textureImporter.textureType = TextureImporterType.NormalMap;
                break;
            case 2:
                textureImporter.textureType = TextureImporterType.GUI;
                break;
            case 3:
                textureImporter.textureType = TextureImporterType.Sprite;
                break;
            case 4:
                textureImporter.textureType = TextureImporterType.Cursor;
                break;
            case 5:
                textureImporter.textureType = TextureImporterType.Cookie;
                break;
            case 6:
                textureImporter.textureType = TextureImporterType.Lightmap;
                break;
            case 7:
                textureImporter.textureType = TextureImporterType.SingleChannel;
                break;
        }
        //Max Size 
        switch (MaxSizeInt) {
            case 0:
                textureImporter.maxTextureSize = 32;
                break;
            case 1:
                textureImporter.maxTextureSize = 64;
                break;
            case 2:
                textureImporter.maxTextureSize = 128;
                break;
            case 3:
                textureImporter.maxTextureSize = 256;
                break;
            case 4:
                textureImporter.maxTextureSize = 512;
                break;
            case 5:
                textureImporter.maxTextureSize = 1024;
                break;
            case 6:
                textureImporter.maxTextureSize = 2048;
                break;
            case 7:
                textureImporter.maxTextureSize = 4096;
                break;
        }
        //Compress
        switch (compressInt) {
            case 0:
                textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
                break;
            case 1:
                textureImporter.textureCompression = TextureImporterCompression.CompressedLQ;
                break;
            case 2:
                textureImporter.textureCompression = TextureImporterCompression.Compressed;
                break;
            case 3:
                textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
                break;
        }

        //pivot
        textureImporter.spritePivot = pivot;
        Debug.Log("set pivot: " + pivot.ToDescription());

        return textureImporter;
    }

    /// <summary>
    /// 循环设置选择的贴图
    /// </summary>
    void LoopSetTexture() {
        Object[] textures = GetSelectedTextures();
        Debug.Log("Loop Set " + textures.Length + " texture");
        Selection.objects = new Object[0];
        foreach (Texture2D texture in textures) {
            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter texImporter = GetTextureSettings(path);
            TextureImporterSettings tis = new TextureImporterSettings();
            texImporter.ReadTextureSettings(tis);
            tis.spriteAlignment = (int)SpriteAlignment.Custom;
            texImporter.SetTextureSettings(tis);
            AssetDatabase.ImportAsset(path);
        }
        Debug.Log("Loop Set Texture End");
    }

    /// <summary>
    /// 获取选择的贴图
    /// </summary>
    /// <returns></returns>
    Object[] GetSelectedTextures() {
        return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
    }
}