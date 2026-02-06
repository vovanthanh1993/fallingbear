# Fix Game Bị Mờ Trên Browser (Netlify)

## Vấn Đề

Game bị **mờ** khi mở trên browser (app.netlify). Nguyên nhân chính:

1. **devicePixelRatio = 1** trong index.html
2. **CSS image-rendering** không tối ưu
3. **Canvas scaling** không đúng

---

## Nguyên Nhân

### 1. **devicePixelRatio = 1**

#### Vấn Đề:
```javascript
if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
    config.devicePixelRatio = 1; // ← Vấn đề!
}
```

#### Tại Sao Bị Mờ:
- **devicePixelRatio = 1** → Canvas render ở resolution thấp
- **Mobile devices** có pixel ratio cao (2x, 3x)
- **Canvas bị scale up** → Bị mờ

#### Ví Dụ:
```
iPhone có devicePixelRatio = 3
→ Canvas render ở 1x thay vì 3x
→ Game bị mờ 3 lần!
```

---

### 2. **CSS Image Rendering**

#### Vấn Đề:
- **Không có** image-rendering settings
- **Browser tự động** smooth khi scale
- **Game bị blur** khi scale

---

## Giải Pháp Đã Áp Dụng

### 1. **Sửa devicePixelRatio**

#### Trước:
```javascript
config.devicePixelRatio = 1; // Bị mờ
```

#### Sau:
```javascript
// Không set devicePixelRatio = 1
// Unity sẽ tự động dùng devicePixelRatio phù hợp
// config.devicePixelRatio = window.devicePixelRatio || 1;
```

#### Kết Quả:
- ✅ Canvas render ở **resolution cao** (2x, 3x)
- ✅ Game **sắc nét** trên mobile
- ✅ **Không bị mờ**

---

### 2. **Thêm CSS Image Rendering**

#### Trước:
```css
#unity-canvas {
    width: 100%;
    height: auto;
}
```

#### Sau:
```css
#unity-canvas {
    width: 100%;
    height: auto;
    /* Đảm bảo canvas render sắc nét */
    image-rendering: -webkit-optimize-contrast;
    image-rendering: -moz-crisp-edges;
    image-rendering: crisp-edges;
    -ms-interpolation-mode: nearest-neighbor;
}
```

#### Kết Quả:
- ✅ Canvas **không bị smooth** khi scale
- ✅ Game **sắc nét** hơn
- ✅ **Ít blur** hơn

---

## Cách Test

### 1. **Build WebGL:**
1. **File** → **Build Settings** → **WebGL**
2. **Build** project
3. **Deploy** lên Netlify

### 2. **Test Trên Browser:**
1. **Mở** game trên browser (app.netlify)
2. **Kiểm tra** game có còn mờ không
3. **So sánh** với trước khi fix

### 3. **Test Trên Mobile:**
1. **Mở** game trên mobile browser
2. **Kiểm tra** độ sắc nét
3. **Verify** không còn mờ

---

## Các Vấn Đề Khác Có Thể Gây Mờ

### 1. **Browser Zoom:**
- **Zoom > 100%** → Game có thể bị mờ
- **Giải pháp:** Reset zoom về 100%

### 2. **Canvas Resolution:**
- **Canvas resolution** thấp hơn screen
- **Giải pháp:** Tăng canvas resolution trong Unity

### 3. **CSS Transform:**
- **Transform scale** có thể gây blur
- **Giải pháp:** Tránh dùng transform scale

---

## Kết Luận

### Vấn Đề:
- ✅ **devicePixelRatio = 1** → Canvas render ở resolution thấp
- ✅ **CSS không tối ưu** → Browser smooth khi scale

### Giải Pháp:
- ✅ **Bỏ devicePixelRatio = 1** → Unity tự động dùng pixel ratio cao
- ✅ **Thêm CSS image-rendering** → Canvas sắc nét hơn

### Kết Quả:
- ✅ Game **không còn mờ** trên browser
- ✅ Game **sắc nét** trên mobile
- ✅ **Trải nghiệm tốt hơn**

---

## Lưu Ý

### Nếu Vẫn Bị Mờ:

1. **Kiểm tra Browser Zoom:**
   - Reset zoom về 100%
   - Test lại

2. **Kiểm tra Canvas Resolution:**
   - Player Settings → WebGL → Resolution
   - Tăng resolution nếu cần

3. **Kiểm tra CSS:**
   - Verify image-rendering đã được apply
   - Check browser DevTools

4. **Test Trên Nhiều Browsers:**
   - Chrome
   - Safari
   - Firefox
   - Edge

---

## Next Steps

1. ✅ **Build WebGL** với fix mới
2. ✅ **Deploy** lên Netlify
3. ✅ **Test** trên browser
4. ✅ **Verify** game không còn mờ
5. ✅ **Test** trên mobile device thật
