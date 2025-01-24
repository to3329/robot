using UnityEngine;

public class ButtonTextObjectSetter : MonoBehaviour
{
    public TextObjectDisplayController displayController; // TextObjectDisplayControllerへの参照
    public GameObject textObjectPrefab; // 表示したいテキストオブジェクトのプレハブ

    // ボタンがクリックされたときに呼ばれるメソッド
    public void OnButtonClick()
    {
        displayController.ShowTextObject(textObjectPrefab);
    }
}
