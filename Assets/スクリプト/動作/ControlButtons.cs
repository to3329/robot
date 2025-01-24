using UnityEngine;
using UnityEngine.UI;

public class ControlButtons : MonoBehaviour
{
    public Button executeButton, saveButton, loadButton;
    public ActionManager actionManager;
    private string saveFilePath;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/actions.json";

        executeButton.onClick.AddListener(() => actionManager.ExecuteActions());
        //saveButton.onClick.AddListener(() => actionManager.SaveActions(saveFilePath));
        saveButton.onClick.AddListener(() => actionManager.SaveActions());
        loadButton.onClick.AddListener(() => actionManager.LoadActions(saveFilePath));
    }
}
