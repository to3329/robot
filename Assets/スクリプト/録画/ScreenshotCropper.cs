using UnityEngine;
using System.IO;

public class ScreenshotCropper : MonoBehaviour
{
    // 録画したい範囲を指定 (スクリーン座標)
    public Rect captureArea = new Rect(100, 100, 500, 500);

    public void CaptureAndCropScreenshot()
    {
        // スクリーン全体をキャプチャしてテクスチャに保存
        Texture2D screenTexture = ScreenCapture.CaptureScreenshotAsTexture();

        // 切り取り範囲を計算 (Y軸の座標はスクリーン左下が基準)
        int x = (int)captureArea.x;
        int y = (int)(Screen.height - captureArea.y - captureArea.height); // Y軸は逆になるので調整
        int width = (int)captureArea.width;
        int height = (int)captureArea.height;

        // 切り取り用のテクスチャを作成
        Texture2D croppedTexture = new Texture2D(width, height);
        Color[] pixels = screenTexture.GetPixels(x, y, width, height); // 指定範囲のピクセルを取得
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        // PNG形式で保存
        string filePath = Path.Combine(Application.dataPath, "CroppedScreenshot.png");
        File.WriteAllBytes(filePath, croppedTexture.EncodeToPNG());

        Debug.Log($"Screenshot captured and saved at: {filePath}");

        // メモリ解放
        Destroy(screenTexture);
        Destroy(croppedTexture);
    }

    // Optional: Editorで範囲を表示する
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 start = new Vector3(captureArea.x, Screen.height - captureArea.y, 0); // 上下を逆に
        Vector3 size = new Vector3(captureArea.width, -captureArea.height, 0); // 高さを逆に
        Gizmos.DrawWireCube(start + size / 2, size);
    }
}
