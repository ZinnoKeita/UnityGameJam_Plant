
using UnityEngine;

public class GrazeSCR : MonoBehaviour
{
    private Transform targetPlayer;
    private Vector3 headOffset;
    private Canvas parentCanvas;
    private Camera mainCamera;

    // 初期化用のメソッド（UISCRから呼ばれる想定）
    public void Initialize(Transform player, Vector3 offset, Canvas canvas, Camera cam)
    {
        targetPlayer = player;
        headOffset = offset;
        parentCanvas = canvas;
        mainCamera = cam;
    }

    void Update()
    {
        // プレイヤーや必要な参照がない場合は何もしない
        if (targetPlayer == null || parentCanvas == null || mainCamera == null) return;

        // 1. プレイヤーの頭上のワールド座標を計算
        Vector3 worldPosition = targetPlayer.position + headOffset;

        // 2. スクリーン座標に変換
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPosition);

        // 3. Canvas内のローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.GetComponent<RectTransform>(),
            screenPoint,
            parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out Vector2 localPoint
        );

        // 4. 位置を反映
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }
}