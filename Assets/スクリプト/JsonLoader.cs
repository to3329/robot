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


    //public string jsonFileName = "commands.json"; // JSON�t�@�C����
    public Transform contentParent; // �f�[�^��\������X�N���[���r���[�� Content
    public GameObject dataPrefab;            // �f�[�^�\���p�̃v���n�u


    public void LoadAndDisplayJson(string filePath)
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        // JSON�t�@�C���̃p�X���擾
        //string filePath = Path.Combine("C:\\Users\\takum\\Downloads\\myproject6\\myproject6", jsonFileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"JSON�t�@�C����������܂���: {filePath}");
            return;
        }

        // JSON�t�@�C����ǂݍ���
        string json = File.ReadAllText(filePath);

        // JSON�f�[�^���f�V���A���C�Y
        //List<ActionData> actions = JsonConvert.DeserializeObject<List<ActionData>>(json);
        List<ActionData> actions = JsonConvert.DeserializeObject<List<ActionData>>(json);
        // Scroll View ���Ƀf�[�^��\��
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
