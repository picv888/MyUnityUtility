using UnityEngine;
using System.Collections;
using UnityEditor;
public class AnimationTools : Editor {
    /// <summary>
    /// 批量图片资源导入设置
    /// 使用说明： 
    /// 1.选择需要批量设置的贴图，
    /// 2.单击IGamesTools/TextureImportSetting，
    /// 3.打开窗口后选择对应参数，
    /// 4.点击Set Texture ImportSettings，
    /// 5.稍等片刻，--批量设置成功。
    /// </summary>
    [MenuItem("MyUtility/AnimationTools/TextureImportSetting")]
    static void TextureImportSettingInit() {
        TextureImportSetting.Init();
    }

    [MenuItem("MyUtility/AnimationTools/GenerateAnimation")]
    static void BuildAnimaitonInit() {
        GenerateAnimation.Init();
    }
}
