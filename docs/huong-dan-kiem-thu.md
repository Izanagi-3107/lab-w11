# Hướng dẫn kiểm thử và bằng chứng chạy

**Trạng thái:** Chưa có log chạy thực tế từ máy có .NET SDK. Các kết quả dưới đây chỉ là **dự kiến theo mã nguồn**, không phải log đã thực thi.

## Lệnh chạy

```bash
dotnet restore
dotnet build
dotnet run
dotnet run -- --test
```

## Kiểm tra thành công

- `dotnet build` kết thúc với `Build succeeded.`
- `dotnet run` có cảnh báo WARNING ở 60%, CRITICAL ở 73.5%, có 3 bản ghi audit và 2 thông báo email mô phỏng.
- `dotnet run -- --test` kết thúc với `Result: 18/18 passed.` nếu tất cả kiểm thử đạt.
- Cần chụp màn hình terminal của cả hai lệnh `dotnet run` và `dotnet run -- --test`, chèn vào báo cáo Word trước khi nộp.

## Danh sách ca kiểm thử dự kiến

| Mã | Tình huống | Kết quả mong đợi |
|---|---|---|
| T01 | Adapter converts 735 to 73.5% | PASS nếu điều kiện kiểm tra đúng |
| T02 | Adapter reads live legacy state | PASS nếu điều kiện kiểm tra đúng |
| T03 | Legacy setter clamps values to 0..1000 | PASS nếu điều kiện kiểm tra đúng |
| T04 | Below 50% gives no alert | PASS nếu điều kiện kiểm tra đúng |
| T05 | 50% is WARNING | PASS nếu điều kiện kiểm tra đúng |
| T06 | 69.9% is WARNING | PASS nếu điều kiện kiểm tra đúng |
| T07 | 70% is CRITICAL | PASS nếu điều kiện kiểm tra đúng |
| T08 | An observer is registered only once | PASS nếu điều kiện kiểm tra đúng |
| T09 | Detached observer receives nothing | PASS nếu điều kiện kiểm tra đúng |
| T10 | Different observers receive the same alert | PASS nếu điều kiện kiểm tra đúng |
| T11 | Failing observer does not stop another | PASS nếu điều kiện kiểm tra đúng |
| T12 | Detaching during notification is safe | PASS nếu điều kiện kiểm tra đúng |
| T13 | Audit log records separate alerts | PASS nếu điều kiện kiểm tra đúng |
| T14 | WARNING does not activate emergency | PASS nếu điều kiện kiểm tra đúng |
| T15 | CRITICAL coordinates alarm door and ventilation | PASS nếu điều kiện kiểm tra đúng |
| T16 | Email observer only simulates delivery | PASS nếu điều kiện kiểm tra đúng |
| T17 | Adapter rejects null legacy detector | PASS nếu điều kiện kiểm tra đúng |
| T18 | Monitor rejects blank name | PASS nếu điều kiện kiểm tra đúng |

## GitHub

1. Tạo repository tên `LabW11_CampusSafetyHub`.
2. Upload toàn bộ nội dung thư mục này vào gốc repository.
3. Vào tab `Actions`, kiểm tra workflow build/test thành công.
4. Sao chép URL thật vào mục Link GitHub trong README và báo cáo Word.
