using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject textPrefab;       // �e�L�X�g�̃v���n�u�i�L�����o�X2�p�j
    public Transform textParent;        // �L�����o�X2���Ńe�L�X�g��z�u����e

    public void GenerateText(string message)
    {
        if (textPrefab == null || textParent == null)
        {
            Debug.LogError("�e�L�X�g�v���n�u�܂��͐e�I�u�W�F�N�g���ݒ肳��Ă��܂���I");
            return;
        }

        // �e�L�X�g���C���X�^���X��
        GameObject newText = Instantiate(textPrefab, textParent);

        // �e�L�X�g�̓��e��ݒ�
        TMP_Text textComponent = newText.GetComponent<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = message;
        }
    }
}
