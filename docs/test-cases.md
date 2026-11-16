# Các tình huống kiểm thử

Chạy toàn bộ kiểm thử bằng lệnh:

```bash
dotnet run -- --test
```

Các tình huống được cài đặt trong `Tests/LabTests.cs`:

| STT | Kiểm thử | Kết quả mong đợi |
|---:|---|---|
| 1 | Adapter nhận giá trị 735 | 73,5% và đơn vị `% smoke` |
| 2 | Thay đổi giá trị cảm biến sau khi tạo adapter | Adapter đọc giá trị mới, không sao chép dữ liệu |
| 3 | Dữ liệu cập nhật ngoài 0..1000 | Legacy setter giới hạn về biên |
| 4 | Mức khói 49,9% | Không cảnh báo |
| 5 | Mức khói 50,0% | WARNING |
| 6 | Mức khói 69,9% | WARNING |
| 7 | Mức khói 70,0% | CRITICAL |
| 8 | Attach cùng observer hai lần | Chỉ nhận một cảnh báo |
| 9 | Detach observer | Không nhận cảnh báo sau đó |
| 10 | Nhiều observer | Cùng nhận một đối tượng cảnh báo |
| 11 | Một observer phát sinh ngoại lệ | Observer khác vẫn nhận sự kiện |
| 12 | Observer tự Detach khi nhận thông báo | Vòng lặp phát thông báo hoạt động bình thường |
| 13 | Hai cảnh báo liên tiếp | Audit log lưu cả hai |
| 14 | WARNING tới EmergencyBridgeObserver | Chưa bật hệ thống ứng phó |
| 15 | CRITICAL tới EmergencyBridgeObserver | Bật chuông, mở khóa cửa, dừng thông gió |
| 16 | EmailAlertObserver nhận cảnh báo | Chỉ mô phỏng email và lưu trong bộ nhớ |
| 17 | Adapter nhận null | Ném ArgumentNullException |
| 18 | Monitor nhận tên cảm biến rỗng | Ném ArgumentException |

**Lưu ý:** Bảng trên là các kết quả mong đợi. Kết quả PASS/FAIL thực tế chỉ xác nhận sau khi chạy bằng .NET SDK.
