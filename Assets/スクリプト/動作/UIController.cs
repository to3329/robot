using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public Transform scrollContent; // �X�N���[���r���[��Content
    public GameObject actionPrefab; // �v���n�u
    public ActionManager actionManager; // ActionManager�̎Q��

    // �{�^�����N���b�N���ꂽ�Ƃ��ɌĂяo�����
    public void AddAction(string actionType)
    {
        float defaultSpeed = 0f;
        float defaultTime = 0f;
        float defaultAngle = 0f;
        float defaultAngleSpeed = 0f;
        // �v���n�u�𐶐�����Content�ɒǉ�
        GameObject actionInstance = Instantiate(actionPrefab, scrollContent);

        ////////////////////////////////////////////////////////////////////////////////////////���̓t�H�[���̎Q��
        TextMeshProUGUI actionText = actionInstance.transform.Find("���얼").GetComponent<TextMeshProUGUI>();
        TMP_InputField speedInput = actionInstance.transform.Find("SpeedInput").GetComponent<TMP_InputField>();
        TMP_InputField timeInput = actionInstance.transform.Find("TimeInput").GetComponent<TMP_InputField>();
        TMP_InputField angleInput = actionInstance.transform.Find("AngleInput").GetComponent<TMP_InputField>();
        TMP_InputField anglespeedInput = actionInstance.transform.Find("AngleSpeedInput").GetComponent<TMP_InputField>();

        /////////////////////////////////////////////////////////////////////////////////�e�L�X�g�̎Q��
        /*
        TextMeshProUGUI speedInputText = actionInstance.transform.Find("���x").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI timeInputText = actionInstance.transform.Find("����").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI angleInputText = actionInstance.transform.Find("�p�x").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI anglespeedInputText = actionInstance.transform.Find("�p���x").GetComponent<TextMeshProUGUI>();///
        */
        
        GameObject speedInputText = actionInstance.transform.Find("���x").gameObject;
        GameObject timeInputText = actionInstance.transform.Find("����").gameObject;
        GameObject angleInputText = actionInstance.transform.Find("�p�x").gameObject;
        GameObject anglespeedInputText = actionInstance.transform.Find("�p���x").gameObject;
        

        actionText.text = actionType;///////////////���O�̕ύX
        speedInput.text = defaultSpeed.ToString();
        timeInput.text = defaultTime.ToString();
        angleInput.text = defaultAngle.ToString();
        anglespeedInput.text = defaultAngleSpeed.ToString();



        GameObject angleInputObject = actionInstance.transform.Find("AngleInput").gameObject; // GameObject ���擾
        GameObject timeInputObject = actionInstance.transform.Find("TimeInput").gameObject; // GameObject ���擾
        GameObject speedInputObject = actionInstance.transform.Find("SpeedInput").gameObject; // GameObject ���擾
        GameObject anglespeedInputObject = actionInstance.transform.Find("AngleSpeedInput").gameObject; // GameObject ���擾

        angleInputObject.SetActive(true); // AngleInput ���\���ɂ���
        anglespeedInputObject.SetActive(true); // AngleInput ���\���ɂ���
              
        speedInputObject.SetActive(true); // AngleInput ���\���ɂ���
        timeInputObject.SetActive(true); // AngleInput ���\���ɂ���


        angleInputText.SetActive(true);
        anglespeedInputText.SetActive(true);
        timeInputText.SetActive(true);
        speedInputText.SetActive(true);


        if (actionType == "�O�i" || actionType == "��i")
        {
            angleInputText.SetActive(false);
            anglespeedInputText.SetActive(false);
            angleInputObject.SetActive(false); // AngleInput ���\���ɂ���
            anglespeedInputObject.SetActive(false); // AngleInput ���\���ɂ���
        }
        else
        {
            timeInputText.SetActive(false);
            speedInputText.SetActive(false);
            speedInputObject.SetActive(false); // AngleInput ���\���ɂ���
            timeInputObject.SetActive(false); // AngleInput ���\���ɂ���

        }

        /////////////////////////////////////////////////////////////////////////////////////////

        // �v���n�u��ActionUI�X�N���v�g���擾���ď�����
        ActionUI actionUI = actionInstance.GetComponent<ActionUI>();
        if (actionUI != null)
        {
            actionUI.Setup(actionType, actionManager);
        }
    }
}
