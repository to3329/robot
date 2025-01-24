using UnityEngine;
using System.IO;

public class ScreenshotCropper : MonoBehaviour
{
    // �^�悵�����͈͂��w�� (�X�N���[�����W)
    public Rect captureArea = new Rect(100, 100, 500, 500);

    public void CaptureAndCropScreenshot()
    {
        // �X�N���[���S�̂��L���v�`�����ăe�N�X�`���ɕۑ�
        Texture2D screenTexture = ScreenCapture.CaptureScreenshotAsTexture();

        // �؂���͈͂��v�Z (Y���̍��W�̓X�N���[���������)
        int x = (int)captureArea.x;
        int y = (int)(Screen.height - captureArea.y - captureArea.height); // Y���͋t�ɂȂ�̂Œ���
        int width = (int)captureArea.width;
        int height = (int)captureArea.height;

        // �؂���p�̃e�N�X�`�����쐬
        Texture2D croppedTexture = new Texture2D(width, height);
        Color[] pixels = screenTexture.GetPixels(x, y, width, height); // �w��͈͂̃s�N�Z�����擾
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        // PNG�`���ŕۑ�
        string filePath = Path.Combine(Application.dataPath, "CroppedScreenshot.png");
        File.WriteAllBytes(filePath, croppedTexture.EncodeToPNG());

        Debug.Log($"Screenshot captured and saved at: {filePath}");

        // ���������
        Destroy(screenTexture);
        Destroy(croppedTexture);
    }

    // Optional: Editor�Ŕ͈͂�\������
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 start = new Vector3(captureArea.x, Screen.height - captureArea.y, 0); // �㉺���t��
        Vector3 size = new Vector3(captureArea.width, -captureArea.height, 0); // �������t��
        Gizmos.DrawWireCube(start + size / 2, size);
    }
}
