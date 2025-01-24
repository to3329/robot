using UnityEngine;
using TMPro;

public class TextObjectDisplayController : MonoBehaviour
{
    public Transform contentParent; // Scroll View��Content���w�肷��
    public GameObject CommandManager;
    /////////////////////////////////////////////////
    public GameObject prefab; // �v���n�u
    public Transform parentTransform; // �e�I�u�W�F�N�g�iContent�Ȃǁj

    private TMP_InputField inputField;

/// //////////////////////////////////////////////

    public float inputValue = 0; // ���̃X�N���v�g���Q�Ƃ��邽�߂̌��J�ϐ�

    // �{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void ShowTextObject(GameObject textObjectPrefab)
    {
        // �v���n�u���C���X�^���X������Content�ɒǉ�
        GameObject textObjectInstance = Instantiate(textObjectPrefab, contentParent);

        // RectTransform�̐ݒ�
        RectTransform rectTransform = textObjectInstance.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one; // �X�P�[�������Z�b�g


        

       

        // �q�I�u�W�F�N�g��InputField���擾




    }

    private TMP_InputField FindInputFieldByName(GameObject parent, string fieldName)
    {
        Debug.Log("�C���v�b�g�t�B�[���h");
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
        inputValue = value; // ���͒l���X�V
        
        Debug.Log("���͒l���X�V����܂���:abcde " + inputValue);
        }
        else
        {
            Debug.LogWarning("�����Ȑ��l�����͂���܂���");
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
    public Transform contentParent; // Scroll View��Content���w�肷��

    // �{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void ShowTextObject(GameObject textObjectPrefab)
    {
        



        // �v���n�u���C���X�^���X������Content�ɒǉ�
        GameObject textObjectInstance = Instantiate(textObjectPrefab, contentParent);


        RectTransform rectTransform = textObjectInstance.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
       
        rectTransform.anchoredPosition = new Vector2(70, 130);

        //RectTransform rectTransform = textObjectInstance.GetComponent<RectTransform>();
        //rectTransform.localScale = Vector3.one; // �X�P�[�������Z�b�g
        //rectTransform.sizeDelta = new Vector2(500, 500); // ��300�A����100�ɐݒ�
        textObjectInstance.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1); // 1.5�{�Ɋg��

        


        // �\���ʒu�̒����i�K�v�ɉ����Ēǉ��j
        //RectTransform rectTransform = textObjectInstance.GetComponent<RectTransform>();
        //rectTransform.localScale = Vector3.one;
    }
}
*/