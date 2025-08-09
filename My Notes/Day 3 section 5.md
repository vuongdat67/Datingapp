#learn #links
# Client login and register
## Learning goals
- [x] Creating components using the Angular CLI
- [x] Using Angular Template forms
- [x] Using Angular services
- [x] Understanding Observables
- [x] Using Angular structural directives to confitionally display elemetns on a page
- [x] Component communication from parent to child
- [x] Component communication from child to parent

---

**folder:**
- layout: navbar,...
- features: members related components, messages, accounts,...
- core: angular services
- shared: shared components
**LIFT principle:**
- Locate
- Identify
- Flat
- Try to be Dry
#Angular
```cs
ng generate
OR
ng g --help
ng c --help
```

```cs
ng g c nav --dry-run
```

```cs
ng g c layout/nav --dry-run
```

```cs
ng g c layout/nav --skip-tests --dry-run
```

*Changes angular.json:*
```json
"schematics": {
        "@schematics/angular:component": {
          "skipTests": true,
          "path": "src"
        }
      },
```

```cs
ng g c layout/nav --dry-run
```

```cs
ng g c layout/nav 
```

---
#css #flexbox #Note
[Flexbox Froggy - A game for learning  flexbox](https://flexboxfroggy.com/)
# Justify-content là gì?
Dùng để căn chỉnh các phần tử con bên trong container theo trục chính (main axis).
Mặc định trục chính là ngang từ trái sang phải (flex-direction: row).
🔹 Các giá trị chính:
1. flex-start (MẶC ĐỊNH)
📌 Các phần tử dồn về bên trái (đầu trục chính).
Ví dụ:
justify-content: flex-start;
📷 Kết quả:
🟩🟩🟩⬜⬜⬜⬜⬜⬜
2. flex-end
📌 Các phần tử dồn về bên phải (cuối trục chính).
Ví dụ:
justify-content: flex-end;
📷 Kết quả:
⬜⬜⬜⬜⬜⬜🟩🟩🟩
3. center
📌 Các phần tử căn giữa container theo chiều ngang.
Ví dụ:
justify-content: center;
📷 Kết quả:
⬜⬜⬜🟩🟩🟩⬜⬜⬜
4. space-between
📌 Các phần tử cách đều nhau, phần tử đầu ở bên trái, phần tử cuối ở bên phải. Không có khoảng ở rìa ngoài.
Ví dụ:
justify-content: space-between;
📷 Kết quả:
🟩⬜⬜⬜🟩⬜⬜⬜🟩
(3 phần tử → khoảng cách giữa 2 phần tử là đều nhau, rìa ngoài không có khoảng trắng)
5. space-around
📌 Các phần tử có khoảng cách đều xung quanh.
→ Có nửa khoảng trống ở 2 bên rìa, khoảng giữa thì gấp đôi.
Ví dụ:
justify-content: space-around;
📷 Kết quả:
⬜🟩⬜⬜🟩⬜⬜🟩⬜
(Khoảng ở hai bên nhỏ hơn khoảng giữa)
6. space-evenly
📌 Các phần tử cách nhau đều hoàn toàn (bao gồm cả 2 bên rìa).
(Không phải game Flexbox Froggy nào cũng có giá trị này)
Ví dụ:
justify-content: space-evenly;
📷 Kết quả:
⬜⬜🟩⬜⬜🟩⬜⬜🟩⬜⬜
(Các khoảng cách = nhau tuyệt đối)
## 🧠 Tóm tắt nhanh:

|Giá trị|Căn chỉnh|Khoảng cách rìa ngoài?|
|---|---|---|
|`flex-start`|Dồn về trái|Không|
|`flex-end`|Dồn về phải|Không|
|`center`|Căn giữa|Có (tính toán tự động)|
|`space-between`|Cách đều, không có rìa|Không|
|`space-around`|Cách đều có rìa nhỏ|Có|
|`space-evenly`|Cách đều cả trong & rìa|Có (đều tuyệt đối)|

---
Level 1:
```css
justify-content: flex-end;
```
Level 2:
```css
justify-content: center;
```
Level 3:
```css
justify-content: space-around;
```
Level 4:
```css
justify-content: space-between;
```
---

## align-items là gì?
Dùng để căn các phần tử con theo chiều dọc (trục phụ) trong container.
Trục phụ là vuông góc với trục chính.
Mặc định flex-direction: row → trục phụ là chiều dọc
Mặc định flex-direction: column → trục phụ là chiều ngang
👉 Trong game Flexbox Froggy, thường dùng mặc định (row), nên align-items điều khiển vị trí theo trục dọc (trên ↕️ dưới).
🔹 Các giá trị chính:
1. flex-start
📌 Căn phần tử về đầu trục phụ, tức là trên cùng.
Kết quả: Ếch ở sát đỉnh ao (top)
align-items: flex-start;
2. flex-end
📌 Căn phần tử về cuối trục phụ, tức là dưới cùng.
Kết quả: Ếch ở đáy ao (bottom)
align-items: flex-end;
3. center
📌 Căn giữa theo trục phụ → giữa chiều cao của container
align-items: center;
4. baseline
📌 Căn các phần tử theo dòng cơ sở của chữ (baseline), thường dùng với phần tử có text
⚠️ Trong Flexbox Froggy thì baseline ít thấy hiệu quả vì các ếch không có văn bản
align-items: baseline;
5. stretch (mặc định)
📌 Các phần tử giãn chiều cao (hoặc chiều ngang) để lấp đầy container, nếu không đặt height cụ thể.
align-items: stretch;
🧪 Ví dụ trực quan:
Giả sử bạn có 3 con ếch (divs) trong một hàng (flex-direction: row)
<div class="pond">
  <div class="frog">🐸</div>
  <div class="frog">🐸</div>
  <div class="frog">🐸</div>
</div>
Căn dưới:
.pond {
  display: flex;
  align-items: flex-end;
}
→ Tất cả ếch đứng ở đáy container.
## 🧠 Tóm tắt bảng so sánh:

|Giá trị|Hiệu ứng (khi `flex-direction: row`)|
|---|---|
|`flex-start`|Các item nằm **trên cùng**|
|`flex-end`|Các item nằm **dưới cùng**|
|`center`|Các item nằm **chính giữa dọc**|
|`baseline`|Căn theo dòng chữ (nếu có)|
|`stretch`|**Kéo giãn chiều cao** của item cho bằng container|

---
Level 5:
```css
align-items: flex-end;
```
Level 6:
```css
justify-content: center;
align-items: center;
```
Level 7:
```css
justify-content: space-around;
align-items: flex-end;
```
---
##  Cú pháp:
`flex-direction: <giá trị>;`

## Các giá trị và ý nghĩa:

|Giá trị|Mô tả ngắn|Trục chính|Hướng|
|---|---|---|---|
|`row` _(mặc định)_|Sắp xếp từ **trái ➡ phải**|Ngang (→)|Bình thường (LTR)|
|`row-reverse`|Sắp xếp từ **phải ➡ trái**|Ngang (→)|Ngược|
|`column`|Sắp xếp từ **trên ⬇ dưới**|Dọc (↓)|Bình thường|
|`column-reverse`|Sắp xếp từ **dưới ⬆ trên**|Dọc (↓)|Ngược|

## 🐸 Trong game Flexbox Froggy:
> Khi ếch **không khớp vị trí với lá sen**, thì bạn cần đảo thứ tự bằng `flex-direction`.
### Ví dụ:
Giả sử bạn có 3 con ếch và chúng **đang từ trái qua phải**, nhưng lá sen yêu cầu **từ phải qua trái**, bạn viết:
`flex-direction: row-reverse;`
##  Minh họa trực quan:
Giả sử có HTML:
`<div class="pond">   <div>🐸1</div>   <div>🐸2</div>   <div>🐸3</div> </div>`
### ➡ `flex-direction: row;`
> 🐸1 — 🐸2 — 🐸3 (từ trái qua phải)
### ⬅ `flex-direction: row-reverse;`
> 🐸3 — 🐸2 — 🐸1 (từ phải qua trái)
### ⬇ `flex-direction: column;`
> 🐸1  
> 🐸2  
> 🐸3
### ⬆ `flex-direction: column-reverse;`
> 🐸3  
> 🐸2  
> 🐸1
---
## 🧠 Mẹo ghi nhớ:
- `row` = theo chiều ngang, giống như **văn bản**
- `row-reverse` = đảo thứ tự ngang
- `column` = sắp từ trên xuống
- `column-reverse` = sắp từ dưới lên
---
Level 8:
```css
flex-direction: row-reverse;
```
Level 9:
```css
flex-direction: column;
```
Level 10:
```css
justify-content: flex-end;
flex-direction: row-reverse;
```
Level 11:
```css
flex-direction: column;
justify-content: flex-end;
```
Level 12:
```css
flex-direction: column-reverse;
justify-content: space-between;
```
Level 13:
```css
flex-direction: row-reverse;
justify-content: center;
align-items: flex-end;
```
---
## 🔹 Cú pháp:
`order: <số nguyên>;`
##  Cách hoạt động:
- Mặc định mọi phần tử đều có `order: 0`.
- Phần tử nào có `order` nhỏ hơn thì **đứng trước**, lớn hơn thì **đứng sau**.
- Bạn có thể dùng số âm (`-1`, `-2`) hoặc dương (`1`, `2`, `3`, ...).
---
##  Ví dụ minh họa:
Giả sử có HTML:
`<div class="pond">   <div class="frog green"></div>   <div class="frog yellow"></div>   <div class="frog red"></div> </div>`
### 🐸 Mục tiêu:
Đưa các con ếch đến đúng vị trí **lá sen** theo thứ tự: **red → yellow → green**  
(Trong HTML thì lại là green → yellow → red)
### ➡ CSS:
`.frog.green {   order: 3; } .frog.yellow {   order: 2; } .frog.red {   order: 1; }`
→ Kết quả: red đứng trước, rồi yellow, rồi green.
## 🧠 Ghi nhớ:
- Số `order` **nhỏ hơn** → đứng trước
- Số `order` **lớn hơn** → đứng sau
- Có thể dùng số âm để đẩy phần tử ra phía trước
---
## 📌 Trong Flexbox Froggy:
Bạn sẽ thấy 3 con ếch, ví dụ:
`.green { order: 2; } .yellow { order: 1; } .red { order: 3; }`
→ yellow → green → red

---
Level 14:
```css
order: 2;
```
Level 15:
```css
order:-3
```
---
## 🔹 Cú pháp:
`.item {   align-self: flex-start | flex-end | center | baseline | stretch; }`
## Ý nghĩa các giá trị:

|Giá trị|Giải thích|
|---|---|
|`flex-start`|Căn lên **đầu** của container (trục dọc nếu `flex-direction: row`)|
|`flex-end`|Căn xuống **cuối** của container|
|`center`|Căn **giữa** trục dọc|
|`baseline`|Căn theo dòng chữ (baseline) của nội dung|
|`stretch` _(mặc định)_|Kéo giãn phần tử để **đầy chiều dọc** container|

##  So sánh với `align-items`:

|`align-items`|Áp dụng cho **tất cả phần tử con** trong container|
|---|---|
|`align-self`|Áp dụng cho **một phần tử duy nhất**|

---

## 🐸 Ví dụ trong Flexbox Froggy:

Giả sử có 3 con ếch và bạn muốn **chỉ con màu vàng nằm dưới đáy**, còn các con khác nằm ở giữa:

`.yellow {   align-self: flex-end; }`
Các con khác vẫn giữ theo `align-items: center;` (hoặc mặc định).

---
Level 16:
```css
align-self: flex-end;
```
Level 17:
```css
order: 1;
align-self: flex-end;
```
---
## 🧠 Tổng quan về `flex-wrap`
`.container {   flex-wrap: nowrap | wrap | wrap-reverse; }`
### 💡 Ý nghĩa:

|Giá trị|Giải thích|
|---|---|
|`nowrap` _(mặc định)_|Các phần tử **không xuống dòng**, dù không đủ chỗ (có thể bị bóp nhỏ lại)|
|`wrap`|Các phần tử **tự động xuống dòng** khi không đủ chỗ|
|`wrap-reverse`|Giống `wrap`, nhưng **dòng mới hiện ở trên** thay vì dưới|

## 🐸 Trong Flexbox Froggy:

Giả sử có 6 con ếch, mà `flex-wrap: nowrap` thì **tất cả sẽ nằm trên một dòng**, bị co lại nhỏ xíu để vừa container.
 Khi bạn thêm:
`flex-wrap: wrap;`
→ Chúng sẽ **tự động dàn sang dòng kế tiếp** nếu không đủ không gian trên một dòng.
## 📌 Ghi nhớ:
- **`nowrap`**: ép tất cả phần tử nằm trên 1 dòng.
- **`wrap`**: cho phép xuống dòng (theo hướng bình thường: dòng sau nằm bên dưới).
- **`wrap-reverse`**: xuống dòng nhưng dòng sau nằm **lên trên**.
📍 Bạn có thể thử trực tiếp với đoạn sau trong Flexbox Froggy:
`flex-wrap: wrap;`
→ Các con ếch sẽ bung ra thành nhiều dòng nếu container nhỏ.

---
Level 18:
```css
flex-wrap: wrap;
```
Level 19:
```css
flex-wrap: wrap;
flex-direction: column;
```
---
**shorthand property** (viết tắt) trong CSS gọi là `flex-flow` – dùng để kết hợp hai thuộc tính:
- `flex-direction`
- `flex-wrap`
## 🧠 Cú pháp `flex-flow`:
`.container {   flex-flow: <flex-direction> <flex-wrap>; }`
**Ví dụ:**
`flex-flow: row wrap;`
Tương đương với:
`flex-direction: row; flex-wrap: wrap;`
---
## Các giá trị hợp lệ:

|`flex-direction`|`flex-wrap`|
|---|---|
|`row`|`nowrap` _(mặc định)_|
|`row-reverse`|`wrap`|
|`column`|`wrap-reverse`|
|`column-reverse`||

 --> Chỉ cần nhớ là **thứ tự là: direction trước, wrap sau** (cách nhau bằng dấu cách).

## 🐸 Trong Flexbox Froggy:

Đã học `flex-direction: column-reverse;` và `flex-wrap: wrap-reverse;`  
Thì có thể viết gọn lại thành:
`flex-flow: column-reverse wrap-reverse;`
Tương tự, nếu đề bài yêu cầu:
- Hiển thị từ trên xuống (`column`)
- Và cho phép xuống dòng (`wrap`)
→ Bạn viết:
`flex-flow: column wrap;`
---
## 📝 Tổng kết:

|Viết dài|Viết ngắn (`flex-flow`)|
|---|---|
|`flex-direction: row;`|`flex-flow: row nowrap;`|
|`flex-direction: column;` + `flex-wrap: wrap;`|`flex-flow: column wrap;`|
|`flex-direction: row-reverse;` + `flex-wrap: wrap-reverse;`|`flex-flow: row-reverse wrap-reverse;`|

---

Level 20:
```css
flex-wrap: wrap;
flex-direction: column-reverse;
```
---
## 🧠 **Khác biệt giữa `align-items` và `align-content`**

|`align-items`|`align-content`|
|---|---|
|Căn chỉnh **từng item** theo chiều dọc|Căn chỉnh **các hàng (lines)** theo chiều dọc|
|Áp dụng cho **mọi layout** (kể cả 1 dòng)|Chỉ có **tác dụng khi có nhiều dòng** (`flex-wrap: wrap`)|
|Tác động tới **vị trí của từng item**|Tác động tới **khoảng cách giữa các dòng**|

##  Các giá trị của `align-content`

|Giá trị|Ý nghĩa dễ hiểu|
|---|---|
|`flex-start`|Các dòng nằm sát **trên cùng** của container|
|`flex-end`|Các dòng nằm sát **dưới cùng** của container|
|`center`|Các dòng nằm **giữa chiều cao** của container|
|`space-between`|Khoảng cách **bằng nhau giữa các dòng**, không có ở rìa|
|`space-around`|Khoảng cách **bằng nhau xung quanh mỗi dòng**|
|`stretch`|Mỗi dòng **giãn ra đều để chiếm hết chiều cao container** _(mặc định nếu có không gian)_|

---

### 🎯 Minh họa thực tế:
Giả sử có 3 dòng ếch, mỗi dòng có 3 con 🐸🐸🐸:
- `align-content: flex-start`: tất cả 3 dòng dồn lên trên cùng.
- `align-content: flex-end`: tất cả 3 dòng dồn xuống dưới.
- `align-content: center`: 3 dòng nằm giữa chiều cao.
- `align-content: space-between`: dòng đầu nằm trên cùng, dòng cuối dưới cùng, dòng giữa nằm giữa (cách đều).
- `align-content: space-around`: mỗi dòng có khoảng trắng trên và dưới đều nhau.
- `align-content: stretch`: chiều cao mỗi dòng giãn ra để lấp đầy container.
---
### 🚫 Lưu ý:
> Nếu chỉ có **1 dòng duy nhất**, `align-content` **không có tác dụng**. Trong trường hợp đó, bạn dùng `align-items` thay.
---
### ✅ Tóm lại:

|Khi nào dùng?|Dùng gì?|
|---|---|
|Có nhiều dòng?|`align-content`|
|Chỉ 1 dòng item?|`align-items`|
|Căn từng item riêng lẻ?|`align-self`|

---

Level 21:
```css
align-content: flex-start;
```
Level 22:
```css
align-content: flex-end;
```
Level 23:
```css
flex-direction: column-reverse;
align-content: center;
```
Level 24:
```css
flex-direction: column-reverse;
flex-wrap: wrap-reverse;
justify-content: center;
align-content: space-between;
```

---
## 🧠 **Tóm tắt các thuộc tính cần nhớ:**

|Thuộc tính|Tác dụng chính|
|---|---|
|`justify-content`|Căn item theo **chiều ngang (main axis)**|
|`align-items`|Căn item theo **chiều dọc (cross axis)**|
|`flex-direction`|Đổi **chiều chính**: row / column / reverse|
|`flex-wrap`|Cho phép xuống dòng (wrap) hoặc đảo ngược dòng (wrap-reverse)|
|`flex-flow`|Gộp `flex-direction` + `flex-wrap`|
|`align-content`|Căn chỉnh **các dòng (lines)** khi có nhiều dòng|
|`order`|Đặt lại thứ tự item (áp dụng từng item riêng)|
|`align-self`|Căn chỉnh riêng lẻ từng item (ghi đè `align-items`)|

---

								   🐸 
## 🧠 TỔNG HỢP TẤT CẢ THUỘC TÍNH (giải thích dễ hiểu):
---
### 🔹 `justify-content` → Căn theo **trục chính**
- `flex-start`, `flex-end`, `center`, `space-between`, `space-around`, `space-evenly`
- **Dùng để căn chỉnh ngang hoặc dọc (tùy theo `flex-direction`)**
---
### 🔹 `align-items` → Căn các item theo **trục phụ**
- `stretch`, `flex-start`, `center`, `flex-end`, `baseline`
- **Áp dụng cho toàn bộ item (1 dòng)**
---
### 🔹 `flex-direction` → Đổi **hướng trục chính**
- `row` (ngang trái qua phải – mặc định)
- `row-reverse`, `column`, `column-reverse`
---
### 🔹 `flex-wrap` → Cho phép **xuống dòng**
- `nowrap` (mặc định)
- `wrap` → khi hết chỗ, item xuống dòng mới
- `wrap-reverse` → dòng mới thêm **phía trên**
---
### 🔹 `flex-flow` → Gộp `flex-direction` và `flex-wrap`
`flex-flow: row wrap; flex-flow: column-reverse nowrap;`

---
### 🔹 `align-content` → Căn **các dòng** (chỉ khi có nhiều dòng – nhờ `flex-wrap`)
- `flex-start`, `center`, `flex-end`, `space-between`, `space-around`, `stretch`

> ⚠️ KHÁC `align-items`:
> - `align-items` áp dụng cho **các item trong một dòng**
> - `align-content` áp dụng cho **cả nhiều dòng** (multi-line)
>     
---
### 🔹 `order` → Đổi thứ tự hiển thị của **từng item**
- Mặc định: `order: 0`
- Bạn gán số: `order: -1`, `order: 3`… phần tử nào có số nhỏ sẽ hiển thị trước
---

### 🔹 `align-self` → Ghi đè `align-items` cho **từng item**
`.frog {   align-self: flex-end; }`

---

## 📌 TỔNG KẾT:

|Tên thuộc tính|Tác dụng gì?|
|---|---|
|`flex-direction`|Đổi hướng trục chính|
|`justify-content`|Căn item theo trục chính|
|`align-items`|Căn item theo trục phụ (trong 1 dòng)|
|`align-content`|Căn toàn bộ dòng (nhiều dòng)|
|`flex-wrap`|Cho phép xuống dòng|
|`flex-flow`|Gộp `direction` + `wrap`|
|`order`|Đổi thứ tự xuất hiện từng item|
|`align-self`|Ghi đè `align-items` cho từng item|

---
[Heroicons](https://heroicons.com/)

---

```c
ng g --help
```

```c
ng g s account-service --dry-run
```

Angular.json:
```json
"schematics": {
        "@schematics/angular:component": {
          "skipTests": true,
          "path": "src"
        },
        "@schematics/angular:service": {
          "skipTests": true,
          "path": "src/core/services"
        }
      },

```

```c
ng g s account-service --dry-run

ng g s account-service
```

---

Dropbox Daisy UI:
- [ ] Method 1. details and summary
- [ ] Method 2. popover API and anchor positioning`new`
- [x] Method 3. CSS focus

---

## Obserables
- New sandard for managing async data included in ES7
- Angular v2
- They are lazy collections of multiple values over time
- Think of observables like a newsletter
- Like a Newsletter
	- Only subscribers of the newsletter receive the newsletter
	- If no-one subcribers to the newsletter it probably will not be printed

---
### **So sánh Promise và Observable**

| Tiêu chí                     | **Promise**                                | **Observable**                                                              |
| ---------------------------- | ------------------------------------------ | --------------------------------------------------------------------------- |
| **Nguồn gốc**                | JavaScript ES6                             | RxJS (Reactive Extensions for JavaScript)                                   |
| **Đơn giá trị / Đa giá trị** | **Chỉ trả về một giá trị duy nhất**        | **Trả về nhiều giá trị theo thời gian**                                     |
| **Tính lười (Lazy)**         | **Không lười** — bắt đầu chạy ngay khi tạo | **Lười** — chỉ bắt đầu khi được `subscribe()`                               |
| **Có thể huỷ (cancel)**      | ❌ Không thể huỷ                            | ✅ Có thể huỷ bằng `unsubscribe()`                                           |
| **Toán tử xử lý dữ liệu**    | ❌ Không có (chỉ `.then()` và `.catch()`)   | ✅ Có nhiều toán tử mạnh (`map`, `filter`, `merge`, `reduce`, `retry`, v.v.) |
| **Tái sử dụng**              | ❌ Không tái sử dụng                        | ✅ Có thể tái sử dụng nhiều lần                                              |
| **Kết thúc (complete)**      | Tự động kết thúc sau khi resolve/reject    | Phải chủ động gọi `complete()` hoặc `unsubscribe()`                         |
| **Phổ biến dùng cho**        | Gọi API đơn giản (REST)                    | Gọi API phức tạp, lắng nghe events, xử lý streams                           |

---

### 📌 **Ví dụ minh hoạ**

#### 1. Promise

`getData(): Promise<any> {   return fetch('https://api.example.com/data')     .then(response => response.json()); }`

Sử dụng:
`this.getData().then(data => {   console.log(data); });`

---

#### 2. Observable (RxJS)

`getData(): Observable<any> {   return this.http.get('https://api.example.com/data'); }`

Sử dụng:
`this.getData().subscribe({   next: data => console.log(data),   error: err => console.error(err),   complete: () => console.log('Done') });`

---

### 📚 **Khi nào dùng cái nào?**

| **Tình huống**                                     | **Nên dùng**                    |
| -------------------------------------------------- | ------------------------------- |
| Gọi API 1 lần đơn giản                             | ✅ Promise / Observable đều được |
| Gọi API và cần xử lý nhiều thao tác (retry, delay) | ✅ Observable                    |
| Theo dõi sự kiện (click, input, WebSocket…)        | ✅ Observable                    |
| Thao tác không cần hủy bỏ                          | Promise                         |
| Cần dừng stream khi component bị hủy (ngOnDestroy) | Observable + `takeUntil()`      |

---

### 💡 Tổng kết
- **Promise**: Dễ dùng, thích hợp với thao tác đơn giản, xử lý một lần rồi kết thúc.
- **Observable**: Mạnh mẽ hơn, phù hợp với Angular và các thao tác phức tạp, theo thời gian.
--> Làm việc với Angular + HttpClient → **nên dùng Observable** là mặc định, vì đó là cách Angular hoạt động theo reactive programming.

---
#excalidraw
![[Pasted image 20250808164134.png]]
#Angular #ts
#### Subcribe
```cs
getMembers(){
	this.service.getMembers().subscribe(members =>
	{
		this.member = members
	}, error =>
	{
		console.log(error);
	}, () =>{
		consolo.log('completed');
	}
	)
}
```
#### ToPromise
```cs
getMembers(){
	return this.http.get('api/users).toPromise()
}
```
#### Async Pipe
```html
<li *ngFor='let member of service.getMembers() | async'>{{member.username}}</>
```
--> Automatically subscribes/unscribes from the observable

#### Signals
A signal is a wrapper around a value that notifies interested consumers when that value changes. Signals can contain any value, from primitives to complex data structures

```js
const count = signal(0);
//signals are getter functions- calling them reads their values.
console.log('The count is:' + count());
// Set a new value
count.set(3);
//Update a value
count.update(value=>value+2)
```
- Simplicity and readablility
- Performance
- Predictability
- Intergration
---

Angular commands:
```cs
ng g c features/home --dry-run
ng g c features/home 
ng g c features/account/register
```
---
**Summary:**
- Use of local storage to persitst token --> XSS weakness
- Length of token expiration
- Password salt in Database --> ASP.Net Identity

---
END