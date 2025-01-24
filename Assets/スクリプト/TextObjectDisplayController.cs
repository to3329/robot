using UnityEngine;
using TMPro;

public class TextObjectDisplayController : MonoBehaviour
{
    public Transform contentParent; // Scroll ViewのContentを指定する
    public GameObject CommandManager;
    /////////////////////////////////////////////////
    public GameObject prefab; // プレハブ
    public Transform parentTransform; // 親オブジェクト（Contentなど）

    private TMP_InputField inputField;

/// //////////////////////////////////////////////

    public float inputValue = 0; // 他のスクリプトが参照するための公開変数

    // ボタンがクリックされたときに呼ばれるメソッド
    public void ShowTextObject(GameObject textObjectPrefab)
    {
        // プレハブをインスタンス化してContentに追加
        GameObject textObjectInstance = Instantiate(textObjectPrefab, contentParent);

        // RectTransformの設定
        RectTransform rectTransform = textObjectInstance.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one; // スケールをリセット


        

       

        // 子オブジェクトのInputFieldを取得




    }

    private TMP_InputField FindInputFieldByName(GameObject parent, string fieldName)
    {
        Debug.Log("インプットフィールド");
        foreach (TMP_InputField inputField in parent.GetComponentsInChildren<TMP_InputField>())
        {
            if (inputField.name == fieldName)
            {
                return inputField;
            }
        }
        return null;
    }


    public void OnValueChanged(TMP_InputField input)
    {
        if (float.TryParse(input.text, out float value))
        {
        inputValue = value; // 入力値を更新
        
        Debug.Log("入力値が更新されました:abcde " + inputValue);
        }
        else
        {
            Debug.LogWarning("無効な数値が入力されました");
        }
    }


    public float GetInputValue()
    {
        return inputValue;
    }


}





/*using UnityEngine;

public class TextObjectDisplayController : MonoBehaviour
{
    public Transform contentParent; // Scroll ViewのContentを指定する

    // ボタンがクリックされたときに呼ばれるメソッド
    public void ShowTextObject(GameObject textObjectPrefab)
    {
        



        // プレハブをインスタンス化してContentに追加
        GameObject textObjectInstance = Instantiate(textObjectPrefab, contentParent);


        RectTransform rectTransform = textObjectInstance.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
       
        rectTransform.anchoredPosition = new Vector2(70, 130);

        //RectTransform rectTransform = textObjectInstance.GetComponent<RectTransform>();
        //rectTransform.localScale = Vector3.one; // スケールをリセット
        //rectTransform.sizeDelta = new Vector2(500, 500); // 幅300、高さ100に設定
        textObjectInstance.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1); // 1.5倍に拡大

        


        // 表示位置の調整（必要に応じて追加）
        //RectTransform rectTransform = textObjectInstance.GetComponent<RectTransform>();
        //rectTransform.localScale = Vector3.one;
    }
}
*/