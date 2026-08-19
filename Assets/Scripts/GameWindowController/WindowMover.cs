using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowMover : MonoBehaviour
{
#if UNITY_STANDALONE_WIN

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

    private IntPtr hwnd;

    private Vector2 windowCenter;

    private bool resizeIsRunning = true;

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

        lastWindowPos = new Vector2Int(
            rect.Left,
            rect.Top
        );

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

        SetWindowPos(
            hwnd,
            IntPtr.Zero,
            x,
            y,
            0,
            0,
            SWP_NOSIZE | SWP_NOZORDER
        );
    }

    [SerializeField] private float windowMovingSpeed = 7.5f;
    private Vector2Int lastWindowPos;
    public void Update()
    {
        if (hwnd == IntPtr.Zero) return;

        GetWindowRect(hwnd, out RECT rect);
        int currentX = rect.Left;
        int currentY = rect.Top;

        if (UpdateControler.Instance.ControlWindowMode)
        {
            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.A)) dir += Vector3.left;
            if (Input.GetKey(KeyCode.D)) dir += Vector3.right;
            if (Input.GetKey(KeyCode.W)) dir += Vector3.up;
            if (Input.GetKey(KeyCode.S)) dir += Vector3.down;

            dir = dir.normalized;

            if (dir != Vector3.zero)
            {
                int moveX = Mathf.RoundToInt(dir.x * windowMovingSpeed);
                int moveY = -Mathf.RoundToInt(dir.y * windowMovingSpeed); // OS screen Y goes down

                currentX += moveX;
                currentY += moveY;

                SetWindowPos(hwnd, IntPtr.Zero, currentX, currentY, 0, 0, SWP_NOSIZE | SWP_NOZORDER);

                // Refresh rect to match new position
                GetWindowRect(hwnd, out rect);
                currentX = rect.Left;
                currentY = rect.Top;
            }
        }

        // 2. Calculate actual delta from last frame
        int deltaX = currentX - lastWindowPos.x;
        int deltaY = currentY - lastWindowPos.y;

        if (deltaX != 0 || deltaY != 0)
        {
            if (Camera.main != null)
            {
                // Convert pixel delta to Unity world units
                float factor = (2f * Camera.main.orthographicSize) / Screen.height;
                factor *= 2.5f; // more speed
                Vector3 worldDelta = new Vector3(deltaX * factor, -deltaY * factor, 0f);

                // Compensate only through the camera. World objects must keep
                // their positions so they appear stationary on the desktop.
                if (!resizeIsRunning)
                    Camera.main.transform.position += worldDelta;
            }

            // Save new position
            lastWindowPos = new Vector2Int(currentX, currentY);
        }
    }

    public void OnRemoteUsed(bool value)
    {
        if (value)
        {
            ZoomSmall();
        }
        else
        {
            ZoomBig();
        }
            
    }    

    [SerializeField] private float resizeDuration = 0.35f;

    private Coroutine resizeCoroutine;


    public void ZoomBig()
    {
        StartResize(
            Screen.currentResolution.width,
            Screen.currentResolution.height
        );
    }

    public void ZoomSmall()
    {
        StartResize(
            Screen.currentResolution.width / 2,
            Screen.currentResolution.height / 2
        );
    }

    private void StartResize(int targetWidth, int targetHeight)
    {
        if (resizeCoroutine != null)
        {
            StopCoroutine(resizeCoroutine);
        }

        resizeCoroutine = StartCoroutine(
            ResizeWindow(targetWidth, targetHeight)
        );
    }
    private IEnumerator ResizeWindow(int targetWidth, int targetHeight)
    {
        if (hwnd == IntPtr.Zero)
            yield break;

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

            SetWindowPos(
                hwnd,
                IntPtr.Zero,
                x,
                y,
                width,
                height,
                SWP_NOZORDER
            );

            yield return null;
        }

        // Đảm bảo giá trị cuối chính xác
        SetWindowPos(
            hwnd,
            IntPtr.Zero,
            targetX,
            targetY,
            targetWidth,
            targetHeight,
            SWP_NOZORDER
        );

        resizeIsRunning = false;
    }

    private void OnDestroy()
    {
        PlayerController.IsRemoteUsed -= OnRemoteUsed;
    }
#endif
}
