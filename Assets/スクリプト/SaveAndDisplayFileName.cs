using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class SaveAndDisplayFileName : MonoBehaviour
{
    public ScrollViewButtonManager scrollViewManager; // �{�^���Ǘ��X�N���v�g
    public string saveFileName = "commands.json"; // �ۑ��t�@�C����

    public void SaveToJson()
    {
        // JSON��ۑ�
        string filePath = System.IO.Path.Combine("C:\\Users\\takum\\Downloads\\myproject6\\myproject6", saveFileName);
        string json = File.ReadAllText(filePath);
        //string json = "{\"key\":\"value\"}"; // ����JSON�f�[�^
        System.IO.File.WriteAllText(filePath, json);

        Debug.Log($"JSON�t�@�C�����ۑ�����܂���: {filePath}");

        // �X�N���[���r���[�Ƀt�@�C������ǉ�
        if (scrollViewManager != null)
        {
            scrollViewManager.AddButtonToScrollView(saveFileName);
        }
    }
}
