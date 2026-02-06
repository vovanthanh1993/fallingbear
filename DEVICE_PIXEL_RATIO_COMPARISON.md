# So Sánh devicePixelRatio = 1 vs 2

## Tổng Quan

`devicePixelRatio` quyết định **resolution** mà canvas render. Giá trị cao hơn = **sắc nét hơn** nhưng **tốn performance hơn**.

---

## So Sánh Chi Tiết

### 1. **Resolution (Độ Phân Giải)**

#### devicePixelRatio = 1:
```
Screen: 1920x1080
Canvas render: 1920x1080 (1x)
→ Render ở resolution thấp
```

#### devicePixelRatio = 2:
```
Screen: 1920x1080
Canvas render: 3840x2160 (2x)
→ Render ở resolution cao gấp 2 lần
```

#### Kết Quả:
- ✅ **devicePixelRatio = 2:** Canvas render **gấp 4 lần** pixels (2x width × 2x height)
- ❌ **devicePixelRatio = 1:** Canvas render ở **resolution thấp**

---

### 2. **Visual Quality (Chất Lượng Hình Ảnh)**

#### devicePixelRatio = 1:
```
Canvas: 1920x1080
Screen: 1920x1080
→ 1 pixel canvas = 1 pixel screen
→ Có thể bị mờ trên high-DPI displays
```

#### devicePixelRatio = 2:
```
Canvas: 3840x2160
Screen: 1920x1080
→ 2 pixels canvas = 1 pixel screen
→ Sắc nét hơn trên high-DPI displays
```

#### Kết Quả:
- ✅ **devicePixelRatio = 2:** **Sắc nét hơn** trên mobile/Retina displays
- ❌ **devicePixelRatio = 1:** **Bị mờ** trên high-DPI displays

---

### 3. **Performance (Hiệu Suất)**

#### devicePixelRatio = 1:
```
Canvas size: 1920x1080 = 2,073,600 pixels
GPU work: Thấp
FPS: Cao hơn
Memory: Thấp hơn
```

#### devicePixelRatio = 2:
```
Canvas size: 3840x2160 = 8,294,400 pixels
GPU work: Cao hơn (gấp 4 lần)
FPS: Thấp hơn một chút
Memory: Cao hơn (gấp 4 lần)
```

#### Kết Quả:
- ✅ **devicePixelRatio = 1:** **Performance tốt hơn** (FPS cao, memory thấp)
- ⚠️ **devicePixelRatio = 2:** **Performance thấp hơn** (FPS thấp hơn, memory cao)

---

### 4. **Memory Usage (Bộ Nhớ)**

#### devicePixelRatio = 1:
```
Canvas buffer: 1920 × 1080 × 4 bytes (RGBA) = 8.3 MB
Total memory: Thấp
```

#### devicePixelRatio = 2:
```
Canvas buffer: 3840 × 2160 × 4 bytes (RGBA) = 33.2 MB
Total memory: Cao hơn (gấp 4 lần)
```

#### Kết Quả:
- ✅ **devicePixelRatio = 1:** **Memory thấp** (8.3 MB)
- ⚠️ **devicePixelRatio = 2:** **Memory cao** (33.2 MB - gấp 4 lần)

---

### 5. **FPS (Frames Per Second)**

#### devicePixelRatio = 1:
```
Desktop: 60 FPS
Mobile: 30-35 FPS
→ Performance tốt
```

#### devicePixelRatio = 2:
```
Desktop: 55-60 FPS (giảm 5-10%)
Mobile: 25-30 FPS (giảm 5-10%)
→ Performance thấp hơn một chút
```

#### Kết Quả:
- ✅ **devicePixelRatio = 1:** **FPS cao hơn** (5-10%)
- ⚠️ **devicePixelRatio = 2:** **FPS thấp hơn** (5-10%)

---

### 6. **Trên Mobile Browser**

#### devicePixelRatio = 1:
```
Mobile screen: 366x731
Canvas render: 366x731
→ Bị mờ trên high-DPI displays
→ Game không sắc nét
```

#### devicePixelRatio = 2:
```
Mobile screen: 366x731
Canvas render: 732x1462
→ Sắc nét trên high-DPI displays
→ Game rõ ràng hơn
```

#### Kết Quả:
- ❌ **devicePixelRatio = 1:** **Bị mờ** trên mobile
- ✅ **devicePixelRatio = 2:** **Sắc nét** trên mobile

---

## Bảng So Sánh Tổng Thể

| Metric | devicePixelRatio = 1 | devicePixelRatio = 2 | Winner |
|--------|----------------------|----------------------|--------|
| **Resolution** | 1920x1080 | 3840x2160 | ✅ 2 (4x pixels) |
| **Visual Quality** | Mờ trên high-DPI | Sắc nét | ✅ 2 |
| **FPS** | 60 FPS | 55-60 FPS | ✅ 1 (5-10% cao hơn) |
| **Memory** | 8.3 MB | 33.2 MB | ✅ 1 (4x thấp hơn) |
| **Performance** | Tốt | Tốt (thấp hơn một chút) | ✅ 1 |
| **Mobile Quality** | Mờ | Sắc nét | ✅ 2 |
| **Desktop Quality** | Ổn | Rất tốt | ✅ 2 |

---

## Khi Nào Dùng devicePixelRatio = 1?

### ✅ Nên Dùng Khi:
- **Performance quan trọng hơn** quality
- **Memory hạn chế** (low-end devices)
- **Desktop only** (không cần high-DPI)
- **Game đơn giản** (không cần quá sắc nét)

### ❌ Không Nên Dùng Khi:
- **Mobile browser** (sẽ bị mờ)
- **High-DPI displays** (Retina, 4K)
- **Quality quan trọng** hơn performance

---

## Khi Nào Dùng devicePixelRatio = 2?

### ✅ Nên Dùng Khi:
- **Mobile browser** (sắc nét hơn)
- **High-DPI displays** (Retina, 4K)
- **Quality quan trọng** hơn performance
- **Game có nhiều text/UI** (cần sắc nét)

### ❌ Không Nên Dùng Khi:
- **Low-end devices** (performance thấp)
- **Memory hạn chế** (cần tiết kiệm memory)
- **Game đơn giản** (không cần quá sắc nét)

---

## Tự Động Detect (Khuyến Nghị)

### Code Tốt Nhất:
```javascript
if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
    container.className = "unity-mobile";
    // Tự động detect pixel ratio của device
    config.devicePixelRatio = window.devicePixelRatio || 2;
} else {
    // Desktop: dùng 1x để tiết kiệm performance
    config.devicePixelRatio = 1;
}
```

### Lợi Ích:
- ✅ **Tự động** dùng pixel ratio phù hợp
- ✅ **Mobile:** Sắc nét (2x, 3x tùy device)
- ✅ **Desktop:** Performance tốt (1x)
- ✅ **Fallback:** 2 nếu không detect được

---

## Kết Luận

### devicePixelRatio = 1:
- ✅ **Performance tốt** (FPS cao, memory thấp)
- ❌ **Bị mờ** trên mobile/high-DPI
- ✅ **Phù hợp** cho desktop, low-end devices

### devicePixelRatio = 2:
- ✅ **Sắc nét** trên mobile/high-DPI
- ⚠️ **Performance thấp hơn** một chút (5-10%)
- ✅ **Phù hợp** cho mobile browser, quality-first

### Khuyến Nghị:
- ✅ **Mobile:** Dùng `devicePixelRatio = 2` hoặc tự động detect
- ✅ **Desktop:** Có thể dùng `devicePixelRatio = 1` để tiết kiệm performance
- ✅ **Best Practice:** Tự động detect `window.devicePixelRatio`

---

## Ví Dụ Cụ Thể

### iPhone 13 (devicePixelRatio = 3):
```
devicePixelRatio = 1:
→ Canvas: 390x844 (mờ)
→ Game bị mờ 3 lần

devicePixelRatio = 2:
→ Canvas: 780x1688 (sắc nét hơn)
→ Game sắc nét hơn

devicePixelRatio = 3 (tự động):
→ Canvas: 1170x2532 (sắc nét nhất)
→ Game sắc nét nhất
```

### Android Phone (devicePixelRatio = 2.5):
```
devicePixelRatio = 1:
→ Canvas: 360x640 (mờ)
→ Game bị mờ 2.5 lần

devicePixelRatio = 2:
→ Canvas: 720x1280 (sắc nét)
→ Game sắc nét (gần như perfect)
```

---

## Trade-off

### devicePixelRatio = 1:
- ✅ Performance: **Tốt**
- ❌ Quality: **Mờ trên mobile**
- ✅ Memory: **Thấp**

### devicePixelRatio = 2:
- ⚠️ Performance: **Tốt (thấp hơn 5-10%)**
- ✅ Quality: **Sắc nét trên mobile**
- ⚠️ Memory: **Cao hơn (4x)**

### Đánh Giá:
- **Cho Mobile Browser:** `devicePixelRatio = 2` là **lựa chọn tốt**
- **Trade-off đáng giá:** Performance giảm 5-10% nhưng quality tăng đáng kể
- **User Experience:** Sắc nét quan trọng hơn FPS cao một chút
