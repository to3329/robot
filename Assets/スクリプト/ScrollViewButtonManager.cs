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
    public GameObject buttonPrefab; // �{�^���v���n�u
    public Transform contentParentA; // �X�N���[���r���[A�� Content
    public Transform contentParentB; // �X�N���[���r���[B�� Content
    public GameObject dataPrefab;   // �f�[�^�\���p�v���n�u

    public Vector2 buttonSize = new Vector2(200, 50);

    // �t�@�C�����{�^�����X�N���[���r���[A�ɒǉ�
    public void AddButtonToScrollView(string fileName)
    {
        if (buttonPrefab == null || contentParentA == null)
        {
            Debug.LogError("�{�^���v���n�u�܂���Content���ݒ肳��Ă��܂���B");
            return;
        }

        // �{�^���𐶐�����Content�ɒǉ�
        GameObject buttonInstance = Instantiate(buttonPrefab, contentParentA);

        // �{�^���̃T�C�Y��ݒ�
        RectTransform rectTransform = buttonInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = buttonSize;
        }

        // �{�^���̃e�L�X�g��ݒ�
        TMP_Text buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.text = fileName;
        }

        // �{�^���̃N���b�N�C�x���g��ݒ�
        Button button = buttonInstance.GetComponent<Button>();
        if (button != null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");
            button.onClick.AddListener(() => LoadAndDisplayJson(filePath));
        }
    }

    // JSON �t�@�C����ǂݍ���ŃX�N���[���r���[B�ɕ\��
    public void LoadAndDisplayJson(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"�t�@�C����������܂���: {filePath}");
            return;
        }

        string json = File.ReadAllText(filePath);
        List<ActionData> actions = JsonConvert.DeserializeObject<List<ActionData>>(json);

        // �X�N���[���r���[B�̓��e���N���A
        foreach (Transform child in contentParentB)
        {
            Destroy(child.gameObject);
        }

        // JSON �f�[�^���X�N���[���r���[B�ɕ\��
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
    public GameObject buttonPrefab; // �{�^���v���n�u
    public Transform contentParent; // Scroll View��Content
    //public Transform contentParent1;//���ߎ��s��ʂ�content



    private JsonLoader jsonLoader;

    public Vector2 buttonSize = new Vector2(200, 50); // ��200�A����50
    //public float fontSize = 300f;                     // �t�H���g�T�C�Y24


    public void AddButtonToScrollView(string fileName)
    {
        //public void AddButtonToScrollView(string fileName)
        //{
            if (buttonPrefab == null || contentParent == null)
            {
                Debug.LogError("�{�^���v���n�u�܂���Content���ݒ肳��Ă��܂���B");
                return;
            }

            // �v���n�u���C���X�^���X������Content�ɒǉ�
            GameObject buttonInstance = Instantiate(buttonPrefab, contentParent);

            // �{�^���̃T�C�Y��ݒ�
            RectTransform rectTransform = buttonInstance.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = buttonSize;
            }

            // �{�^���̃e�L�X�g��ݒ�
            TMP_Text buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = fileName;
            }

            // �{�^���̃N���b�N�C�x���g��ݒ�
            Button button = buttonInstance.GetComponent<Button>();
            if (button != null)
            {
                string filePath = Path.Combine("C:\\Users\\takum\\Downloads\\myproject6\\myproject6", fileName + ".json");
                button.onClick.AddListener(() => Load.LoadAndDisplayJson(filePath));
            }
        







        // �{�^���̃e�L�X�g��ݒ�
        //Text buttonText = buttonInstance.GetComponentInChildren<TextMeshPro>();
        TMP_Text buttonText = buttonInstance.GetComponentInChildren<TMP_Text>();

        
        if (buttonText != null)
        {
            Debug.Log("�{�^��" + buttonText);
            buttonText.text = fileName; // �{�^���Ƀt�@�C������\��
            //buttonText.fontSize = fontSize;
        }
        else
        {
            Debug.LogError("�{�^����Text�R���|�[�l���g��������܂���B");
        }

        // �{�^���̃N���b�N�C�x���g��ݒ�
        //Button button = buttonInstance.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnButtonClick(fileName));
        }
        
    }

    // �{�^���N���b�N���̏���
    private void OnButtonClick(string fileName)
    {
        Debug.Log($"�{�^�����N���b�N����܂���: {fileName}");
        // �K�v�ɉ����ăt�@�C�����J�����菈�������s
    }
}


*/