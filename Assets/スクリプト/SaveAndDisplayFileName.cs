using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class SaveAndDisplayFileName : MonoBehaviour
{
    public ScrollViewButtonManager scrollViewManager; // ボタン管理スクリプト
    public string saveFileName = "commands.json"; // 保存ファイル名

    public void SaveToJson()
    {
        // JSONを保存
        string filePath = System.IO.Path.Combine("C:\\Users\\takum\\Downloads\\myproject6\\myproject6", saveFileName);
        string json = File.ReadAllText(filePath);
        //string json = "{\"key\":\"value\"}"; // 仮のJSONデータ
        System.IO.File.WriteAllText(filePath, json);

        Debug.Log($"JSONファイルが保存されました: {filePath}");

        // スクロールビューにファイル名を追加
        if (scrollViewManager != null)
        {
            scrollViewManager.AddButtonToScrollView(saveFileName);
        }
    }
}
