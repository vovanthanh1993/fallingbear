# Unity WebGL Tự Động Scale Trên Mobile Browser

## Tổng Quan

Unity WebGL **tự động scale** game trên mobile browser để phù hợp với màn hình. Đây là tính năng tự động, nhưng bạn có thể **kiểm soát** thông qua các settings.

---

## Cách Unity WebGL Scale Tự Động

### 1. **Canvas Scaler Settings**

#### Trong Project:
- **Canvas Scaler** component quyết định cách UI scale
- **Reference Resolution:** Resolution mục tiêu (ví dụ: 1920x1080)
- **Match Width Or Height:** Tỷ lệ scale theo width hay height

#### Ví Dụ:
```
Reference Resolution: 1920x1080
Match Width Or Height: 0.5 (cân bằng)

Mobile Screen: 366x731
→ Unity tự động scale để fit màn hình
→ Game sẽ nhỏ hơn nhưng vẫn giữ tỷ lệ
```

---

### 2. **WebGL Build Settings**

#### Player Settings → WebGL → Resolution and Presentation:

#### A. **Default Canvas Width/Height:**
- **Width:** 960 (mặc định)
- **Height:** 600 (mặc định)
- Đây là **resolution mặc định** khi load game

#### B. **Run In Background:**
- Có thể ảnh hưởng đến scaling

#### C. **WebGL Memory Size:**
- Không ảnh hưởng scaling, nhưng ảnh hưởng performance

---

### 3. **Screen Resolution Handling**

#### Unity Tự Động:
- **Desktop:** Dùng resolution mặc định hoặc fullscreen
- **Mobile:** Tự động scale để fit màn hình
- **Aspect Ratio:** Giữ nguyên tỷ lệ (không bị méo)

#### Ví Dụ:
```
Desktop: 1920x1080 → Game hiển thị 1920x1080
Mobile: 366x731 → Game tự động scale xuống để fit
```

---

## Các Loại Scaling

### 1. **Scale With Screen Size** (Phổ Biến)

#### Cách Hoạt Động:
- **Reference Resolution:** 1920x1080
- **Match Width Or Height:** 0.5
- **Mobile Screen:** 366x731

#### Kết Quả:
- Unity tính toán **scale factor** dựa trên tỷ lệ
- Game được **scale down** để fit màn hình
- **UI elements** cũng được scale theo

#### Ví Dụ Tính Toán:
```
Reference: 1920x1080
Mobile: 366x731

Scale X: 366/1920 = 0.19
Scale Y: 731/1080 = 0.68

Match 0.5 → Dùng trung bình
→ Game scale xuống ~0.4x (40% size)
```

---

### 2. **Constant Pixel Size**

#### Cách Hoạt Động:
- **Không scale** theo screen size
- **1 pixel = 1 pixel** (trên mọi device)
- **UI sẽ rất nhỏ** trên mobile nếu dùng resolution lớn

#### Khi Nào Dùng:
- ✅ **Không nên dùng** cho mobile
- ✅ Chỉ dùng khi muốn **pixel-perfect** (rất hiếm)

---

### 3. **Constant Physical Size**

#### Cách Hoạt Động:
- Scale theo **DPI** (dots per inch)
- **1 inch = 1 inch** trên mọi device
- Phức tạp hơn, ít dùng

---

## Tại Sao Game Nhỏ Hơn Trên Mobile?

### Lý Do:

#### 1. **Screen Resolution Nhỏ Hơn:**
- **Desktop:** 1920x1080 (2+ triệu pixels)
- **Mobile:** 366x731 (~267k pixels)
- **Tỷ lệ:** Mobile chỉ có **~13%** pixels của desktop

#### 2. **Unity Tự Động Scale:**
- Unity **scale down** game để fit màn hình
- **Giữ nguyên tỷ lệ** (không bị méo)
- **UI elements** cũng được scale

#### 3. **Canvas Scaler Settings:**
- **Reference Resolution:** 1920x1080 (lớn)
- **Mobile Screen:** 366x731 (nhỏ)
- → **Scale factor** nhỏ → Game nhỏ hơn

---

## Cách Kiểm Soát Scaling

### 1. **Điều Chỉnh Canvas Scaler**

#### Trong Inspector:
- **Reference Resolution:** Giảm xuống (ví dụ: 960x540)
- **Match Width Or Height:** Điều chỉnh (0 = width, 1 = height)

#### Ví Dụ:
```
Reference Resolution: 960x540 (thay vì 1920x1080)
Mobile: 366x731

Scale X: 366/960 = 0.38
Scale Y: 731/540 = 1.35

→ Game sẽ lớn hơn trên mobile
```

---

### 2. **Điều Chỉnh WebGL Build Settings**

#### Player Settings → WebGL → Resolution:

#### A. **Default Canvas Width/Height:**
- **Giảm xuống** để game lớn hơn trên mobile
- Ví dụ: 960x540 thay vì 1920x1080

#### B. **WebGL Template:**
- Một số templates có **responsive scaling**
- Có thể điều chỉnh trong template code

---

### 3. **Code-Based Scaling**

#### Tự Động Điều Chỉnh Trong Code:

```csharp
using UnityEngine;

public class MobileScaler : MonoBehaviour
{
    void Start()
    {
        // Kiểm tra nếu là mobile
        bool isMobile = Screen.width < 800 || Screen.height < 800;
        
        if (isMobile)
        {
            // Điều chỉnh Canvas Scaler
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                // Giảm reference resolution để game lớn hơn
                scaler.referenceResolution = new Vector2(960, 540);
                scaler.matchWidthOrHeight = 0.5f;
            }
        }
    }
}
```

---

## Best Practices

### 1. **Design Cho Mobile First:**
- ✅ **Reference Resolution:** 960x540 hoặc 720x1280
- ✅ **Test trên mobile** thường xuyên
- ✅ **UI elements** đủ lớn để dễ nhấn

### 2. **Responsive Design:**
- ✅ **Dùng Canvas Scaler** với Scale With Screen Size
- ✅ **Match Width Or Height:** 0.5 (cân bằng)
- ✅ **Test trên nhiều resolutions** khác nhau

### 3. **Avoid Fixed Sizes:**
- ❌ **Không dùng** fixed pixel sizes
- ✅ **Dùng** anchors và relative sizes
- ✅ **Dùng** Layout Groups (Horizontal, Vertical)

---

## Giải Pháp Cho Vấn Đề "Game Quá Nhỏ"

### Option 1: **Giảm Reference Resolution**

#### Trước:
```
Reference Resolution: 1920x1080
Mobile: 366x731
Scale: ~0.4x (40%)
```

#### Sau:
```
Reference Resolution: 960x540
Mobile: 366x731
Scale: ~0.7x (70%)
→ Game lớn hơn 75%!
```

---

### Option 2: **Điều Chỉnh Match Width Or Height**

#### Match Width (0):
- Scale theo **width**
- Game sẽ **fill width** của màn hình
- Có thể bị **crop** ở top/bottom

#### Match Height (1):
- Scale theo **height**
- Game sẽ **fill height** của màn hình
- Có thể bị **crop** ở left/right

#### Match Balanced (0.5):
- Scale **cân bằng**
- Game sẽ **fit** màn hình (không crop)
- **Nhỏ hơn** nhưng **an toàn**

---

### Option 3: **Multiple Canvas Scalers**

#### Tạo 2 Canvas:
- **Canvas Desktop:** Reference 1920x1080
- **Canvas Mobile:** Reference 960x540
- **Enable/Disable** theo platform

---

## Kiểm Tra Scaling Hiện Tại

### 1. **Trong Editor:**
- **Game View:** Chọn resolution (ví dụ: 366x731)
- **Xem** game scale như thế nào
- **Điều chỉnh** Canvas Scaler settings

### 2. **Trên Mobile Browser:**
- **Mở game** trên mobile
- **Check Screen.width và Screen.height** (trong code)
- **So sánh** với reference resolution

### 3. **Performance Monitor:**
- **Script đã tạo** hiển thị Screen resolution
- **Xem** resolution thực tế trên mobile
- **Điều chỉnh** settings cho phù hợp

---

## Kết Luận

### Unity WebGL Tự Động Scale:
- ✅ **Tự động** scale game để fit màn hình
- ✅ **Giữ nguyên** tỷ lệ (không bị méo)
- ✅ **Có thể kiểm soát** qua Canvas Scaler settings

### Game Nhỏ Hơn Trên Mobile Vì:
- ✅ **Screen resolution** nhỏ hơn (366x731 vs 1920x1080)
- ✅ **Unity scale down** để fit
- ✅ **Reference resolution** lớn (1920x1080)

### Giải Pháp:
- ✅ **Giảm Reference Resolution** (960x540)
- ✅ **Điều chỉnh Match Width Or Height** (0 hoặc 1)
- ✅ **Design mobile-first** (reference resolution nhỏ hơn)

---

## Khuyến Nghị

### Cho Game Của Bạn:

#### 1. **Kiểm Tra Canvas Scaler:**
- Xem **Reference Resolution** hiện tại là gì
- Nếu là 1920x1080 → **Giảm xuống 960x540**

#### 2. **Test Trên Mobile:**
- Build WebGL
- Test trên mobile browser
- **Xem** game có quá nhỏ không

#### 3. **Điều Chỉnh:**
- Nếu quá nhỏ → **Giảm Reference Resolution**
- Nếu vừa → **Giữ nguyên**
- Nếu quá lớn → **Tăng Reference Resolution**

---

## Quick Fix

### Để Game Lớn Hơn Trên Mobile:

1. **Tìm Canvas** trong scene
2. **Canvas Scaler** component
3. **Reference Resolution:** Đổi từ 1920x1080 → **960x540**
4. **Match Width Or Height:** Giữ 0.5 hoặc thử 0 (width)
5. **Build và test** lại

### Kết Quả:
- Game sẽ **lớn hơn ~75%** trên mobile
- Vẫn **giữ tỷ lệ** (không bị méo)
- **UI elements** cũng lớn hơn
