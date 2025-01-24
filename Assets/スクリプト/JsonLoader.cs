using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public class JsonLoader : MonoBehaviour
{
    //NewBehaviourScript newBe;
    /*
    [System.Serializable]
    public class RobotData
    {
        public float speed;
        public float rotationSpeed;
        public string action;
        public float duration;
    }

    [System.Serializable]
    public class RobotDataWrapper
    {
        public List<RobotData> rob;
    }
    */


    //public string jsonFileName = "commands.json"; // JSONファイル名
    public Transform contentParent; // データを表示するスクロールビューの Content
    public GameObject dataPrefab;            // データ表示用のプレハブ


    public void LoadAndDisplayJson(string filePath)
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        // JSONファイルのパスを取得
        //string filePath = Path.Combine("C:\\Users\\takum\\Downloads\\myproject6\\myproject6", jsonFileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"JSONファイルが見つかりません: {filePath}");
            return;
        }

        // JSONファイルを読み込む
        string json = File.ReadAllText(filePath);

        // JSONデータをデシリアライズ
        //List<ActionData> actions = JsonConvert.DeserializeObject<List<ActionData>>(json);
        List<ActionData> actions = JsonConvert.DeserializeObject<List<ActionData>>(json);
        // Scroll View 内にデータを表示
        foreach (var action in actions)
        {

            GameObject dataInstance = Instantiate(dataPrefab, contentParent);
            TMP_Text dataText = dataInstance.GetComponentInChildren<TMP_Text>();
            if (dataText != null)
            {
                dataText.text = $"Type: {action.ActionType}, Speed: {action.Speed}, Time: {action.Time}, Angle: {action.Angle}, AngularSpeed: {action.AngularSpeed}";
            }
        }
    }

    
}
