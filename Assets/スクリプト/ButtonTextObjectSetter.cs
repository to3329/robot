using UnityEngine;

public class ButtonTextObjectSetter : MonoBehaviour
{
    public TextObjectDisplayController displayController; // TextObjectDisplayController�ւ̎Q��
    public GameObject textObjectPrefab; // �\���������e�L�X�g�I�u�W�F�N�g�̃v���n�u

    // �{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void OnButtonClick()
    {
        displayController.ShowTextObject(textObjectPrefab);
    }
}
