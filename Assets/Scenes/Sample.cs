using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour {
    private const int REFERENCE_WIDTH = 812;
    private const int REFERENCE_HEIGHT = 375;

    public CanvasScaler scaler;
    public Text resolutionText;

    private int lastWidth;
    private int lastHeight;

    private void Update() {
        if (lastWidth != Screen.width || lastHeight != Screen.height) {
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            double referenceRatio = REFERENCE_WIDTH * 1.0 / REFERENCE_HEIGHT;
            double ratio = lastWidth * 1.0 / lastHeight;
            if (ratio > referenceRatio) {
                scaler.matchWidthOrHeight = 1;
            } else {
                scaler.matchWidthOrHeight = 0;
            }

            resolutionText.text = $"{Screen.width} x {Screen.height} = {ratio.ToString("F2")}";
        }
    }
}
