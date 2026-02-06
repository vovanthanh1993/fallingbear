# Tại Sao Memory Chỉ Hiển Thị 3MB?

## Vấn Đề

Khi chạy game trên WebGL, script `MobilePerformanceMonitor` chỉ hiển thị **3MB memory**, trong khi game thực tế dùng **200-500 MB**.

---

## Giải Thích

### `System.GC.GetTotalMemory()` Chỉ Trả Về Managed Memory

#### Managed Memory (C# Heap):
- ✅ **Chỉ là C# objects** (GameObjects, Components, Lists, Arrays, etc.)
- ✅ **Không bao gồm:**
  - ❌ Textures (images, sprites)
  - ❌ Meshes (3D models)
  - ❌ Audio clips
  - ❌ WebGL heap (JavaScript heap)
  - ❌ Native memory (Unity engine)

#### Ví Dụ:
```
Total Memory (thực tế): 400 MB
├── Textures: 200 MB (50%)
├── Meshes: 100 MB (25%)
├── Audio: 50 MB (12.5%)
├── WebGL Heap: 40 MB (10%)
└── Managed Memory (C#): 3 MB (0.75%) ← Chỉ có cái này!
```

---

## Tại Sao Managed Memory Nhỏ?

### 1. **Unity Tối Ưu:**
- ✅ **Native code** (C++) xử lý textures, meshes, audio
- ✅ **C# chỉ quản lý** references và metadata
- ✅ **Data thực tế** nằm trong native memory

### 2. **WebGL Architecture:**
- ✅ **WebAssembly (WASM)** chạy native code
- ✅ **JavaScript heap** chứa WebGL data
- ✅ **C# heap** chỉ chứa managed objects

### 3. **Memory Breakdown:**
```
Total Memory: 400 MB
├── Native Memory (Unity Engine): 300 MB
│   ├── Textures: 200 MB
│   ├── Meshes: 100 MB
│   └── Audio: 50 MB
├── WebGL Heap (JavaScript): 40 MB
└── Managed Memory (C#): 3 MB ← Chỉ có cái này!
```

---

## Cách Check Total Memory Thực Tế

### 1. **Browser DevTools** (Khuyến Nghị)

#### Chrome DevTools:
1. **F12** → Mở DevTools
2. **Performance** tab → **Memory** section
3. **Hoặc Memory** tab → **Take heap snapshot**
4. **Xem total heap size**

#### Kết Quả:
- **Total Heap Size:** 400-500 MB (total memory)
- **JS Heap Size:** 40-50 MB (JavaScript heap)
- **ArrayBuffer:** 200-300 MB (textures, meshes)

---

### 2. **Unity Profiler** (Editor Only)

#### Trong Editor:
1. **Window** → **Analysis** → **Profiler**
2. **Memory** tab
3. **Xem breakdown:**
   - Total Allocated
   - Texture Memory
   - Mesh Memory
   - Audio Memory

#### Kết Quả:
- **Total Allocated:** 400-500 MB
- **Texture Memory:** 200 MB
- **Mesh Memory:** 100 MB
- **Audio Memory:** 50 MB

---

### 3. **Unity Stats** (WebGL Build)

#### Trên WebGL Build:
1. **Tap 3 lần** hoặc **nhấn và giữ** màn hình
2. **Unity Stats** hiện ở góc trên bên trái
3. **Xem Memory** section

#### Kết Quả:
- **Memory:** 400-500 MB (total memory)
- **Không phải** managed memory!

---

## Script Đã Được Sửa

### Trước:
```csharp
long memoryMB = System.GC.GetTotalMemory(false) / (1024 * 1024);
// Chỉ hiển thị: "Memory: 3 MB"
// → Gây hiểu lầm (nghĩ là total memory)
```

### Sau:
```csharp
long managedMemoryMB = System.GC.GetTotalMemory(false) / (1024 * 1024);
// Hiển thị: "Managed Mem: 3 MB"
// "⚠️ C# Heap only (not total)"
// "Total: Check DevTools"
// → Rõ ràng đây chỉ là managed memory
```

---

## Kết Luận

### Tại Sao Chỉ 3MB?
- ✅ **Đúng!** Managed memory (C# heap) chỉ 3-10 MB
- ✅ **Nhưng** total memory thực tế là 200-500 MB
- ✅ **Script chỉ đo** managed memory, không đo total memory

### Cách Check Total Memory:
- ✅ **Browser DevTools** (F12 → Memory tab)
- ✅ **Unity Profiler** (Editor only)
- ✅ **Unity Stats** (WebGL build - tap 3 lần)

### Lưu Ý:
- ⚠️ **Managed memory 3 MB là bình thường**
- ⚠️ **Total memory 400 MB cũng bình thường** (cho WebGL)
- ⚠️ **Cần check total memory** trong DevTools để đánh giá chính xác

---

## Khuyến Nghị

### Để Check Memory Chính Xác:

1. **Dùng Browser DevTools:**
   - F12 → Memory tab
   - Take heap snapshot
   - Xem total heap size

2. **Dùng Unity Stats:**
   - Tap 3 lần trên mobile
   - Xem Memory trong Unity Stats

3. **Script Hiện Tại:**
   - Chỉ hiển thị managed memory (C# heap)
   - Có note rõ ràng: "C# Heap only (not total)"
   - Hướng dẫn check DevTools

---

## FAQ

### Q: Tại sao managed memory chỉ 3MB?
**A:** Vì Unity tối ưu - textures, meshes, audio nằm trong native memory, không phải C# heap.

### Q: Total memory thực tế là bao nhiêu?
**A:** Thường 200-500 MB trên WebGL, tùy vào số lượng textures, meshes, audio.

### Q: Làm sao check total memory?
**A:** Dùng Browser DevTools (F12 → Memory) hoặc Unity Stats (tap 3 lần).

### Q: 3MB có phải là vấn đề không?
**A:** Không! Managed memory 3-10 MB là bình thường. Cần check total memory để đánh giá.
