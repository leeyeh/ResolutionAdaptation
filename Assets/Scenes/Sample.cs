using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{
    public CanvasScaler refCanvasScaler;

    public CanvasScaler autoScaler;
    public CanvasScaler syncedScaler;

    public Text resolutionText;
    public Text autoScaleFactor;
    public Text syncedScaleFactor;

    public Button scaleModeSwitcher;

    private int lastWidth;
    private int lastHeight;

    const float BASE_SCALE_FACTOR = 16f / 11;

    private float caculateScaleFactor(CanvasScaler scaler)
    {
        Vector2 referenceResolution = scaler.referenceResolution;

        float logWidth = Mathf.Log(Screen.width / referenceResolution.x, 2);
        float logHeight = Mathf.Log(Screen.height / referenceResolution.y, 2);
        float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, scaler.matchWidthOrHeight);
        return Mathf.Pow(2, logWeightedAverage);

    }

    private void Update()
    {
        if (lastWidth != Screen.width || lastHeight != Screen.height)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            Vector2 referenceResolution = refCanvasScaler.referenceResolution;

            double referenceRatio = referenceResolution.x * 1.0 / referenceResolution.y;
            double ratio = lastWidth * 1.0 / lastHeight;
            resolutionText.text = $"{Screen.width} x {Screen.height} / {ratio.ToString("F2")} : 1; Game Scale Mode: {refCanvasScaler.uiScaleMode}";

            if (ratio > referenceRatio)
            {
                autoScaler.matchWidthOrHeight = 1;
            }
            else
            {
                autoScaler.matchWidthOrHeight = 0;
            }
            autoScaleFactor.text = $"Auto Scale; Factor = { this.caculateScaleFactor(autoScaler).ToString("F2")}";

            float scaleFactor;

            if (refCanvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) {
                CanvasScaler.ScreenMatchMode matchMode = refCanvasScaler.screenMatchMode;

                if (matchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
                {
                    scaleFactor = caculateScaleFactor(refCanvasScaler);
                }
                else if (matchMode == CanvasScaler.ScreenMatchMode.Expand)
                {
                    scaleFactor = Mathf.Min(Screen.width / referenceResolution.x, Screen.height / referenceResolution.y);
                }
                else // matchMode == CanvasScaler.ScreenMatchMode.Shrink
                {
                    scaleFactor = Mathf.Max(Screen.width / referenceResolution.x, Screen.height / referenceResolution.y);
                }
            } else
            {
                scaleFactor = refCanvasScaler.scaleFactor;
            }

            syncedScaler.scaleFactor = scaleFactor * BASE_SCALE_FACTOR;
            syncedScaleFactor.text = $"Synced Scale; Factor = { syncedScaler.scaleFactor.ToString("F2")}";
        }
    }

    private void ForceUpdate()
    {
        this.lastHeight = 0;
        this.Update();
    }

    public void SetConstantMode()
    {
        refCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        refCanvasScaler.scaleFactor = 1f;
        this.ForceUpdate();
    }
    public void SetLargeConstantMode()
    {
        refCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        refCanvasScaler.scaleFactor = 1.3f;
        this.ForceUpdate();
    }
    public void SetMatchMode()
    {
        refCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        refCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        refCanvasScaler.matchWidthOrHeight = 0.5f;
        this.ForceUpdate();
    }
    public void SetMatchExpandMode()
    {
        refCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        refCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        this.ForceUpdate();
    }
}
