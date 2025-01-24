using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public Transform scrollContent; // スクロールビューのContent
    public GameObject actionPrefab; // プレハブ
    public ActionManager actionManager; // ActionManagerの参照

    // ボタンがクリックされたときに呼び出される
    public void AddAction(string actionType)
    {
        float defaultSpeed = 0f;
        float defaultTime = 0f;
        float defaultAngle = 0f;
        float defaultAngleSpeed = 0f;
        // プレハブを生成してContentに追加
        GameObject actionInstance = Instantiate(actionPrefab, scrollContent);

        ////////////////////////////////////////////////////////////////////////////////////////入力フォームの参照
        TextMeshProUGUI actionText = actionInstance.transform.Find("動作名").GetComponent<TextMeshProUGUI>();
        TMP_InputField speedInput = actionInstance.transform.Find("SpeedInput").GetComponent<TMP_InputField>();
        TMP_InputField timeInput = actionInstance.transform.Find("TimeInput").GetComponent<TMP_InputField>();
        TMP_InputField angleInput = actionInstance.transform.Find("AngleInput").GetComponent<TMP_InputField>();
        TMP_InputField anglespeedInput = actionInstance.transform.Find("AngleSpeedInput").GetComponent<TMP_InputField>();

        /////////////////////////////////////////////////////////////////////////////////テキストの参照
        /*
        TextMeshProUGUI speedInputText = actionInstance.transform.Find("速度").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI timeInputText = actionInstance.transform.Find("時間").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI angleInputText = actionInstance.transform.Find("角度").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI anglespeedInputText = actionInstance.transform.Find("角速度").GetComponent<TextMeshProUGUI>();///
        */
        
        GameObject speedInputText = actionInstance.transform.Find("速度").gameObject;
        GameObject timeInputText = actionInstance.transform.Find("時間").gameObject;
        GameObject angleInputText = actionInstance.transform.Find("角度").gameObject;
        GameObject anglespeedInputText = actionInstance.transform.Find("角速度").gameObject;
        

        actionText.text = actionType;///////////////名前の変更
        speedInput.text = defaultSpeed.ToString();
        timeInput.text = defaultTime.ToString();
        angleInput.text = defaultAngle.ToString();
        anglespeedInput.text = defaultAngleSpeed.ToString();



        GameObject angleInputObject = actionInstance.transform.Find("AngleInput").gameObject; // GameObject を取得
        GameObject timeInputObject = actionInstance.transform.Find("TimeInput").gameObject; // GameObject を取得
        GameObject speedInputObject = actionInstance.transform.Find("SpeedInput").gameObject; // GameObject を取得
        GameObject anglespeedInputObject = actionInstance.transform.Find("AngleSpeedInput").gameObject; // GameObject を取得

        angleInputObject.SetActive(true); // AngleInput を非表示にする
        anglespeedInputObject.SetActive(true); // AngleInput を非表示にする
              
        speedInputObject.SetActive(true); // AngleInput を非表示にする
        timeInputObject.SetActive(true); // AngleInput を非表示にする


        angleInputText.SetActive(true);
        anglespeedInputText.SetActive(true);
        timeInputText.SetActive(true);
        speedInputText.SetActive(true);


        if (actionType == "前進" || actionType == "後進")
        {
            angleInputText.SetActive(false);
            anglespeedInputText.SetActive(false);
            angleInputObject.SetActive(false); // AngleInput を非表示にする
            anglespeedInputObject.SetActive(false); // AngleInput を非表示にする
        }
        else
        {
            timeInputText.SetActive(false);
            speedInputText.SetActive(false);
            speedInputObject.SetActive(false); // AngleInput を非表示にする
            timeInputObject.SetActive(false); // AngleInput を非表示にする

        }

        /////////////////////////////////////////////////////////////////////////////////////////

        // プレハブのActionUIスクリプトを取得して初期化
        ActionUI actionUI = actionInstance.GetComponent<ActionUI>();
        if (actionUI != null)
        {
            actionUI.Setup(actionType, actionManager);
        }
    }
}
