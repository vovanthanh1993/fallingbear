using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

namespace GamePolygon
{
    /// <summary>
    /// Performance Monitor cho Mobile Browser
    /// Hiển thị đầy đủ thông tin: FPS, Memory, Frame Time, Batches, Device Info
    /// Desktop: Nhấn F2 để bật/tắt
    /// Mobile: Tap 3 lần vào màn hình để bật/tắt
    /// </summary>
    public class MobilePerformanceMonitor : MonoBehaviour
    {
        [Header("UI Elements (Optional - Tự tạo nếu null)")]
        public Text statsText; // Dùng 1 Text để hiển thị tất cả
        
        [Header("Settings")]
        [Tooltip("Hiển thị FPS")]
        public bool showFPS = true;
        
        [Tooltip("Hiển thị Memory (MB)")]
        public bool showMemory = true;
        
        [Tooltip("Hiển thị Frame Time (ms)")]
        public bool showFrameTime = true;
        
        [Tooltip("Hiển thị Batches (Draw Calls)")]
        public bool showBatches = true;
        
        [Tooltip("Hiển thị Device Info")]
        public bool showDeviceInfo = true;
        
        [Tooltip("Hiển thị Touch Info")]
        public bool showTouchInfo = true;
        
        [Tooltip("Phím để bật/tắt (mặc định F2)")]
        public KeyCode toggleKey = KeyCode.F2;
        
        [Tooltip("Tap bao nhiêu lần để toggle (mobile)")]
        [Range(2, 5)]
        public int tapCountToToggle = 3;
        
        [Tooltip("Update interval (giây)")]
        [Range(0.1f, 2f)]
        public float updateInterval = 0.5f;
        
        [Tooltip("Font size")]
        [Range(12, 32)]
        public int fontSize = 18;
        
        [Tooltip("Màu tốt")]
        public Color goodColor = Color.green;
        
        [Tooltip("Màu trung bình")]
        public Color mediumColor = Color.yellow;
        
        [Tooltip("Màu kém")]
        public Color badColor = Color.red;
        
        [Tooltip("Background color (alpha)")]
        [Range(0f, 1f)]
        public float backgroundColorAlpha = 0.7f;
        
        private bool isVisible = true;
        private float deltaTime = 0.0f;
        private float lastUpdateTime = 0f;
        private Canvas canvas;
        private Image backgroundImage;
        private float lastTapTime = 0f;
        private int tapCount = 0;
        private float tapTimeWindow = 1f; // 1 giây để tap
        
        // Stats tracking
        private float minFPS = float.MaxValue;
        private float maxFPS = 0f;
        private long minMemory = long.MaxValue;
        private long maxMemory = 0;
        
        void Start()
        {
            // Tạo UI nếu chưa có
            if (statsText == null)
            {
                CreateUI();
            }
            
            // Ẩn/hiện ban đầu
            SetVisibility(isVisible);
        }
        
        void Update()
        {
            // Toggle với phím (desktop/editor)
            #if UNITY_EDITOR || !UNITY_WEBGL
            if (Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }
            #endif
            
            // Toggle với touch (mobile)
            HandleTouchToggle();
            
            // Update thông số
            if (isVisible)
            {
                deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
                
                // Update theo interval để không tốn performance
                if (Time.time - lastUpdateTime >= updateInterval)
                {
                    UpdateStats();
                    lastUpdateTime = Time.time;
                }
            }
        }
        
        void HandleTouchToggle()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    float currentTime = Time.time;
                    
                    // Reset tap count nếu quá lâu
                    if (currentTime - lastTapTime > tapTimeWindow)
                    {
                        tapCount = 0;
                    }
                    
                    tapCount++;
                    lastTapTime = currentTime;
                    
                    // Nếu tap đủ số lần
                    if (tapCount >= tapCountToToggle)
                    {
                        Toggle();
                        tapCount = 0;
                    }
                }
            }
            else
            {
                // Reset tap count nếu không có touch
                if (Time.time - lastTapTime > tapTimeWindow)
                {
                    tapCount = 0;
                }
            }
        }
        
        void UpdateStats()
        {
            if (statsText == null) return;
            
            StringBuilder sb = new StringBuilder();
            
            // FPS
            if (showFPS)
            {
                float fps = 1.0f / deltaTime;
                if (fps < minFPS) minFPS = fps;
                if (fps > maxFPS) maxFPS = fps;
                
                Color fpsColor = GetFPSColor(fps);
                sb.AppendLine($"<color=#{ColorUtility.ToHtmlStringRGB(fpsColor)}>FPS: {fps:F1} (Min: {minFPS:F1}, Max: {maxFPS:F1})</color>");
            }
            
            // Memory
            if (showMemory)
            {
                long memoryMB = System.GC.GetTotalMemory(false) / (1024 * 1024);
                if (memoryMB < minMemory) minMemory = memoryMB;
                if (memoryMB > maxMemory) maxMemory = memoryMB;
                
                Color memColor = GetMemoryColor(memoryMB);
                sb.AppendLine($"<color=#{ColorUtility.ToHtmlStringRGB(memColor)}>Memory: {memoryMB} MB (Min: {minMemory}, Max: {maxMemory})</color>");
            }
            
            // Frame Time
            if (showFrameTime)
            {
                float frameTimeMs = deltaTime * 1000f;
                Color frameColor = GetFrameTimeColor(frameTimeMs);
                sb.AppendLine($"<color=#{ColorUtility.ToHtmlStringRGB(frameColor)}>Frame: {frameTimeMs:F2} ms</color>");
            }
            
            // Batches (Draw Calls) - Không có API trực tiếp, cần check Unity Stats
            if (showBatches)
            {
                sb.AppendLine("Batches: Check Unity Stats (Tap 3x to toggle)");
            }
            
            // Device Info
            if (showDeviceInfo)
            {
                sb.AppendLine($"Platform: {Application.platform}");
                sb.AppendLine($"Screen: {Screen.width}x{Screen.height} @ {Screen.currentResolution.refreshRate}Hz");
                sb.AppendLine($"DPI: {Screen.dpi:F1}");
            }
            
            // Touch Info
            if (showTouchInfo)
            {
                int touchCount = Input.touchCount;
                sb.AppendLine($"Touches: {touchCount}");
            }
            
            statsText.text = sb.ToString();
        }
        
        Color GetFPSColor(float fps)
        {
            if (fps >= 30f) return goodColor;
            if (fps >= 20f) return mediumColor;
            return badColor;
        }
        
        Color GetMemoryColor(long memoryMB)
        {
            if (memoryMB < 200) return goodColor;
            if (memoryMB < 400) return mediumColor;
            return badColor;
        }
        
        Color GetFrameTimeColor(float frameTimeMs)
        {
            if (frameTimeMs < 33.3f) return goodColor; // < 30 FPS threshold
            if (frameTimeMs < 50f) return mediumColor;
            return badColor;
        }
        
        void SetVisibility(bool visible)
        {
            if (statsText != null) statsText.gameObject.SetActive(visible);
            if (backgroundImage != null) backgroundImage.gameObject.SetActive(visible);
        }
        
        void CreateUI()
        {
            // Tìm hoặc tạo Canvas
            canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("PerformanceCanvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 9999; // Luôn ở trên cùng
                
                CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.matchWidthOrHeight = 0.5f;
                
                canvasObj.AddComponent<GraphicRaycaster>();
            }
            
            // Tạo Background
            GameObject bgObj = new GameObject("StatsBackground");
            bgObj.transform.SetParent(canvas.transform, false);
            backgroundImage = bgObj.AddComponent<Image>();
            backgroundImage.color = new Color(0, 0, 0, backgroundColorAlpha);
            
            RectTransform bgRect = backgroundImage.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0, 1);
            bgRect.anchorMax = new Vector2(0, 1);
            bgRect.pivot = new Vector2(0, 1);
            bgRect.anchoredPosition = new Vector2(10, -10);
            bgRect.sizeDelta = new Vector2(350, 250); // Sẽ tự điều chỉnh theo content
            
            // Tạo Text cho tất cả stats
            GameObject textObj = new GameObject("StatsText");
            textObj.transform.SetParent(bgObj.transform, false);
            statsText = textObj.AddComponent<Text>();
            statsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            statsText.fontSize = fontSize;
            statsText.color = Color.white;
            statsText.alignment = TextAnchor.UpperLeft;
            statsText.supportRichText = true; // Cho phép HTML color tags
            
            RectTransform textRect = statsText.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0, 1);
            textRect.anchorMax = new Vector2(1, 0);
            textRect.pivot = new Vector2(0, 1);
            textRect.offsetMin = new Vector2(10, 10);
            textRect.offsetMax = new Vector2(-10, -10);
        }
        
        /// <summary>
        /// Bật/hiện performance monitor
        /// </summary>
        public void Show()
        {
            isVisible = true;
            SetVisibility(true);
        }
        
        /// <summary>
        /// Tắt/ẩn performance monitor
        /// </summary>
        public void Hide()
        {
            isVisible = false;
            SetVisibility(false);
        }
        
        /// <summary>
        /// Toggle hiển thị
        /// </summary>
        public void Toggle()
        {
            isVisible = !isVisible;
            SetVisibility(isVisible);
        }
        
        /// <summary>
        /// Reset min/max stats
        /// </summary>
        public void ResetStats()
        {
            minFPS = float.MaxValue;
            maxFPS = 0f;
            minMemory = long.MaxValue;
            maxMemory = 0;
        }
    }
}
