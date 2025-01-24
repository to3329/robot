using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

using System.Text;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public class ScrollViewButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab; // ボタンプレハブ
    public Transform contentParentA; // スクロールビューAの Content
    public Transform contentParentB; // スクロールビューBの Content
    public GameObject dataPrefab;   // データ表示用プレハブ

    public Vector2 buttonSize = new Vector2(200, 50);

    // ファイル名ボタンをスクロールビューAに追加
    public void AddButtonToScrollView(string fileName)
    {
        if (buttonPrefab == null || contentParentA == null)
        {
            Debug.LogError("ボタンプレハブまたはContentが設定されていません。");
            return;
        }

        // ボタンを生成してContentに追加
        GameObject buttonInstance = Instantiate(buttonPrefab, contentParentA);

        // ボタンのサイズを設定
        RectTransform rectTransform = buttonInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = buttonSize;
        }

        // ボタンのテキストを設定
        TMP_Text buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.text = fileName;
        }

        // ボタンのクリックイベントを設定
        Button button = buttonInstance.GetComponent<Button>();
        if (button != null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");
            button.onClick.AddListener(() => LoadAndDisplayJson(filePath));
        }
    }

    // JSON ファイルを読み込んでスクロールビューBに表示
    public void LoadAndDisplayJson(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"ファイルが見つかりません: {filePath}");
            return;
        }

        string json = File.ReadAllText(filePath);
        List<ActionData> actions = JsonConvert.DeserializeObject<List<ActionData>>(json);

        // スクロールビューBの内容をクリア
        foreach (Transform child in contentParentB)
        {
            Destroy(child.gameObject);
        }

        // JSON データをスクロールビューBに表示
        foreach (var action in actions)
        {
            GameObject dataInstance = Instantiate(dataPrefab, contentParentB);
            TMP_Text dataText = dataInstance.GetComponentInChildren<TMP_Text>();

            if (dataText != null)
            {
                dataText.text = $"Action: {action.ActionType}, Speed: {action.Speed}, Time: {action.Time}, Angle: {action.Angle}, AngularSpeed: {action.AngularSpeed}";
            }
        }
    }
}


/*
public class ScrollViewButtonManager : MonoBehaviour
{
    public JsonLoader Load;
    public GameObject buttonPrefab; // ボタンプレハブ
    public Transform contentParent; // Scroll ViewのContent
    //public Transform contentParent1;//命令実行画面のcontent



    private JsonLoader jsonLoader;

    public Vector2 buttonSize = new Vector2(200, 50); // 幅200、高さ50
    //public float fontSize = 300f;                     // フォントサイズ24


    public void AddButtonToScrollView(string fileName)
    {
        //public void AddButtonToScrollView(string fileName)
        //{
            if (buttonPrefab == null || contentParent == null)
            {
                Debug.LogError("ボタンプレハブまたはContentが設定されていません。");
                return;
            }

            // プレハブをインスタンス化してContentに追加
            GameObject buttonInstance = Instantiate(buttonPrefab, contentParent);

            // ボタンのサイズを設定
            RectTransform rectTransform = buttonInstance.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = buttonSize;
            }

            // ボタンのテキストを設定
            TMP_Text buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = fileName;
            }

            // ボタンのクリックイベントを設定
            Button button = buttonInstance.GetComponent<Button>();
            if (button != null)
            {
                string filePath = Path.Combine("C:\\Users\\takum\\Downloads\\myproject6\\myproject6", fileName + ".json");
                button.onClick.AddListener(() => Load.LoadAndDisplayJson(filePath));
            }
        







        // ボタンのテキストを設定
        //Text buttonText = buttonInstance.GetComponentInChildren<TextMeshPro>();
        TMP_Text buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();

        
        if (buttonText != null)
        {
            Debug.Log("ボタン" + buttonText);
            buttonText.text = fileName; // ボタンにファイル名を表示
            //buttonText.fontSize = fontSize;
        }
        else
        {
            Debug.LogError("ボタンにTextコンポーネントが見つかりません。");
        }

        // ボタンのクリックイベントを設定
        //Button button = buttonInstance.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnButtonClick(fileName));
        }
        
    }

    // ボタンクリック時の処理
    private void OnButtonClick(string fileName)
    {
        Debug.Log($"ボタンがクリックされました: {fileName}");
        // 必要に応じてファイルを開いたり処理を実行
    }
}


*/