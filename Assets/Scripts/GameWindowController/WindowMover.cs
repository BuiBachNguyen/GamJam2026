using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowMover : MonoBehaviour
{

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        uint uFlags);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(
        IntPtr hWnd,
        out RECT rect);

    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_NOZORDER = 0x0004;

    public static WindowMover Instance { get; private set; }

    [Header("Window Settings")]
    [SerializeField] private float windowMovingSpeed = 7.5f;
    [SerializeField] private float resizeDuration = 0.35f;

    [Header("Camera Constraints")]
    [Tooltip("Kéo một PolygonCollider2D hoặc BoxCollider2D (isTrigger = true) vào đây để làm vùng di chuyển")]
    [SerializeField] private Collider2D cameraMoveArea;

    private IntPtr hwnd;
    private Vector2 windowCenter;
    private bool resizeIsRunning = true;
    private Vector2Int lastWindowPos;
    private Coroutine resizeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerController.IsRemoteUsed -= OnRemoteUsed;
        PlayerController.IsRemoteUsed += OnRemoteUsed;
    }

    IEnumerator Start()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;

        // đợi window init
        yield return null;
        yield return new WaitForSeconds(0.2f);

        hwnd = GetActiveWindow();
        CenterWindow();

        // save start position
        GetWindowRect(hwnd, out RECT rect);
        lastWindowPos = new Vector2Int(rect.Left, rect.Top);
        windowCenter = GetWindowCenter(rect);
    }

    private Vector2 GetWindowCenter(RECT rect)
    {
        return new Vector2(
            (rect.Left + rect.Right) * 0.5f,
            (rect.Top + rect.Bottom) * 0.5f
        );
    }

    void CenterWindow()
    {
        GetWindowRect(hwnd, out RECT rect);
        int width = rect.Right - rect.Left;
        int height = rect.Bottom - rect.Top;
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        int x = (screenWidth - width) / 2;
        int y = (screenHeight - height) / 2;

        SetWindowPos(hwnd, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
    }

    public void Update()
    {
        if (hwnd == IntPtr.Zero) return;

        GetWindowRect(hwnd, out RECT rect);
        int currentX = rect.Left;
        int currentY = rect.Top;

        float factor = (2f * Camera.main.orthographicSize) / Screen.height;
        factor *= 2.5f; // more speed

        // 1. CHUẨN BỊ DI CHUYỂN BẰNG PHÍM (WASD)
        if (UpdateControler.Instance.ControlWindowMode)
        {
            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.LeftArrow)) dir += Vector3.left;
            if (Input.GetKey(KeyCode.RightArrow)) dir += Vector3.right;
            if (Input.GetKey(KeyCode.UpArrow)) dir += Vector3.up;
            if (Input.GetKey(KeyCode.DownArrow)) dir += Vector3.down;

            dir = dir.normalized;

            if (dir != Vector3.zero)
            {
                int moveX = Mathf.RoundToInt(dir.x * windowMovingSpeed);
                int moveY = -Mathf.RoundToInt(dir.y * windowMovingSpeed);

                if (cameraMoveArea != null)
                {
                    Vector2 currentCamPos = Camera.main.transform.position;

                    // Thử di chuyển trục X, nếu khung hình lọt ra ngoài -> Hủy trục X
                    Vector2 predictX = new Vector2(currentCamPos.x + (moveX * factor), currentCamPos.y);
                    if (!IsCameraViewInside(predictX)) moveX = 0;

                    // Thử di chuyển trục Y, nếu khung hình lọt ra ngoài -> Hủy trục Y
                    Vector2 predictY = new Vector2(currentCamPos.x, currentCamPos.y + (-moveY * factor));
                    if (!IsCameraViewInside(predictY)) moveY = 0;
                }

                // Nếu sau khi kiểm tra tường mà vẫn còn có thể đi được
                if (moveX != 0 || moveY != 0)
                {
                    currentX += moveX;
                    currentY += moveY;

                    SetWindowPos(hwnd, IntPtr.Zero, currentX, currentY, 0, 0, SWP_NOSIZE | SWP_NOZORDER);

                    // Refresh rect to match new position
                    GetWindowRect(hwnd, out rect);
                    currentX = rect.Left;
                    currentY = rect.Top;
                }
            }
        }

        // 2. TÍNH TOÁN VÀ DI CHUYỂN CAMERA THEO CỬA SỔ
        int deltaX = currentX - lastWindowPos.x;
        int deltaY = currentY - lastWindowPos.y;

        if (deltaX != 0 || deltaY != 0)
        {
            if (Camera.main != null && !resizeIsRunning)
            {
                Vector3 worldDelta = new Vector3(deltaX * factor, -deltaY * factor, 0f);
                Vector3 newCamPos = Camera.main.transform.position + worldDelta;

                // Nếu người chơi lôi cửa sổ bằng chuột ra khỏi phòng -> Ép Camera nằm lại mép phòng
                if (cameraMoveArea != null)
                {
                    Vector2 clampedPos = cameraMoveArea.ClosestPoint(newCamPos);
                    newCamPos.x = clampedPos.x;
                    newCamPos.y = clampedPos.y;
                }

                Camera.main.transform.position = newCamPos;
            }

            // Save new position
            lastWindowPos = new Vector2Int(currentX, currentY);
        }
    }

    // ==========================================
    // HỆ THỐNG ĐỔI PHÒNG (TELEPORT & ĐỔI BOUNDS)
    // ==========================================

    /// <summary>
    /// Thay đổi vùng giới hạn Camera hiện tại sang một Collider mới (Phòng mới)
    /// </summary>
    public void SetNewCameraBounds(Collider2D newArea)
    {
        cameraMoveArea = newArea;
    }

    /// <summary>
    /// Teleport Camera sang vị trí mới và set luôn Collider của phòng mới
    /// Tránh được lỗi Camera bị giật ngược lại vị trí phòng cũ
    /// </summary>
    public void TeleportToNewRoom(Vector3 newCameraPosition, Collider2D newRoomBounds)
    {
        // 1. Cập nhật giới hạn mới ngay lập tức
        cameraMoveArea = newRoomBounds;

        // 2. Di chuyển camera
        if (Camera.main != null)
        {
            // Tạm thời giữ nguyên trục Z của camera (thường là -10)
            newCameraPosition.z = Camera.main.transform.position.z;
            Camera.main.transform.position = newCameraPosition;
        }
    }

    // ==========================================
    // ZOOM HỆ THỐNG
    // ==========================================

    public void OnRemoteUsed(bool value)
    {
        if (value) ZoomSmall();
        else ZoomBig();
    }

    public void ZoomBig()
    {
        StartResize(Screen.currentResolution.width, Screen.currentResolution.height);
    }

    public void ZoomSmall()
    {
        StartResize(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);
    }

    private void StartResize(int targetWidth, int targetHeight)
    {
        if (resizeCoroutine != null) StopCoroutine(resizeCoroutine);
        resizeCoroutine = StartCoroutine(ResizeWindow(targetWidth, targetHeight));
    }

    // check 4 cornet of the camera to be inside collider
    private bool IsCameraViewInside(Vector2 camCenter)
    {
        if (cameraMoveArea == null) return true;

        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * Camera.main.aspect;

        Vector2 topLeft = new Vector2(camCenter.x - halfWidth, camCenter.y + halfHeight);
        Vector2 topRight = new Vector2(camCenter.x + halfWidth, camCenter.y + halfHeight);
        Vector2 bottomLeft = new Vector2(camCenter.x - halfWidth, camCenter.y - halfHeight);
        Vector2 bottomRight = new Vector2(camCenter.x + halfWidth, camCenter.y - halfHeight);
        return cameraMoveArea.OverlapPoint(topLeft) &&
               cameraMoveArea.OverlapPoint(topRight) &&
               cameraMoveArea.OverlapPoint(bottomLeft) &&
               cameraMoveArea.OverlapPoint(bottomRight);
    }

    private IEnumerator ResizeWindow(int targetWidth, int targetHeight)
    {
        if (hwnd == IntPtr.Zero) yield break;

        resizeIsRunning = true;
        GetWindowRect(hwnd, out RECT rect);

        int startX = rect.Left;
        int startY = rect.Top;
        int startWidth = rect.Right - rect.Left;
        int startHeight = rect.Bottom - rect.Top;

        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        int targetX = (screenWidth - targetWidth) / 2;
        int targetY = (screenHeight - targetHeight) / 2;

        float elapsed = 0f;

        while (elapsed < resizeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / resizeDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            int x = Mathf.RoundToInt(Mathf.Lerp(startX, targetX, t));
            int y = Mathf.RoundToInt(Mathf.Lerp(startY, targetY, t));
            int width = Mathf.RoundToInt(Mathf.Lerp(startWidth, targetWidth, t));
            int height = Mathf.RoundToInt(Mathf.Lerp(startHeight, targetHeight, t));

            SetWindowPos(hwnd, IntPtr.Zero, x, y, width, height, SWP_NOZORDER);
            yield return null;
        }

        SetWindowPos(hwnd, IntPtr.Zero, targetX, targetY, targetWidth, targetHeight, SWP_NOZORDER);

        Screen.SetResolution(targetWidth, targetHeight, FullScreenMode.Windowed);
        yield return null; // Đợi 1 frame để Unity cập nhật đồng bộ với Windows

        resizeIsRunning = false;
    }

    private void OnDestroy()
    {
        PlayerController.IsRemoteUsed -= OnRemoteUsed;
    }
}