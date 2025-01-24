using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject textPrefab;       // テキストのプレハブ（キャンバス2用）
    public Transform textParent;        // キャンバス2内でテキストを配置する親

    public void GenerateText(string message)
    {
        if (textPrefab == null || textParent == null)
        {
            Debug.LogError("テキストプレハブまたは親オブジェクトが設定されていません！");
            return;
        }

        // テキストをインスタンス化
        GameObject newText = Instantiate(textPrefab, textParent);

        // テキストの内容を設定
        TMP_Text textComponent = newText.GetComponent<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = message;
        }
    }
}
