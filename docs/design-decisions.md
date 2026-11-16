# Phân tích thiết kế – Lab W11

**Sinh viên:** Ngô Ngọc Thái  
**MSSV:** 202418984

## A. Trách nhiệm của các lớp

| Lớp/interface | Trách nhiệm |
|---|---|
| `ISensor` | Quy định giao diện đọc giá trị và đơn vị đo. |
| `LegacySmokeDetector` | Mô phỏng cảm biến cũ, thang đo 0–1000; giữ nguyên API theo đề. |
| `SmokeSensorAdapter` | Bọc cảm biến cũ, chuyển giá trị về 0–100%, cung cấp `ISensor`. |
| `AlertEvent` | Dữ liệu cảnh báo bất biến với giá trị, mức độ, thời gian, tên cảm biến. |
| `IAlertObserver` | Hợp đồng nhận cảnh báo. |
| `SensorMonitor` | Đọc cảm biến, phân loại ngưỡng, phát cảnh báo tới các observer. |
| `DashboardObserver` | Hiển thị thông báo Console. |
| `EmailAlertObserver` | Mô phỏng thông báo email, không gửi email thật. |
| `AuditLogObserver` | Lưu lịch sử các cảnh báo trong bộ nhớ. |
| `EmergencyBridgeObserver` | Chuyển cảnh báo CRITICAL đến mediator. |
| `IEmergencyMediator` | Giao diện nhận tín hiệu khẩn cấp. |
| `EmergencyMediator` | Điều phối alarm, door, ventilation, không để chúng gọi trực tiếp lẫn nhau. |
| `Alarm` | Quản lý trạng thái chuông báo. |
| `DoorController` | Quản lý trạng thái khóa cửa. |
| `VentilationController` | Quản lý trạng thái thông gió. |

## B. Trả lời câu hỏi thiết kế

**1. Adapter nên kế thừa hay chứa `LegacySmokeDetector`?**  
Chọn *composition* (chứa tham chiếu đến cảm biến cũ). `SmokeSensorAdapter` nhận một `LegacySmokeDetector` qua constructor và triển khai `ISensor`. Không phải sửa lớp cũ, không phải giả định cảm biến cũ có quan hệ is-a với giao diện mới. Cách này cũng thuận tiện khi cảm biến cũ là `sealed`.

**2. `SensorMonitor` có nên biết `EmailAlertObserver` cụ thể không?**  
Không. `SensorMonitor` chỉ phụ thuộc `IAlertObserver`, vì vậy có thể thêm hoặc xóa người nhận cảnh báo trong thời gian chạy mà không sửa logic giám sát. Đây là Dependency Inversion và Observer Pattern.

**3. `Alarm` có cần tham chiếu `DoorController` không?**  
Không. Các thiết bị phản ứng chỉ thực hiện hành vi riêng. `EmergencyMediator` nhận thông báo, điều phối nhiều thiết bị. Nhờ vậy các bộ điều khiển không có phụ thuộc chéo.

**4. Ai quyết định giá trị vượt ngưỡng?**  
`SensorMonitor`, vì đây là quy tắc của bộ phận giám sát: dưới 50% bình thường; 50% đến dưới 70% là WARNING; từ 70% trở lên là CRITICAL. Adapter chỉ đổi đơn vị, không áp dụng nghiệp vụ cảnh báo.

## C. Quyết định cho Mediator

Đề W11 mô tả sự tham gia của Alarm, DoorController, VentilationController nhưng chưa chỉ định chính xác hành vi cho từng thiết bị khi có cảnh báo. Trong mô hình thực hành này, khi khói đạt **CRITICAL**:

1. Bật chuông báo động.
2. Mở khóa cửa thoát hiểm.
3. Dừng thông gió trong phòng.

WARNING chỉ thông báo, không kích hoạt thiết bị khẩn cấp. Đây là **giả định triển khai phục vụ bài lab**, không phải quy trình an toàn phòng cháy được chứng nhận. Trong hệ thống thực, phản ứng HVAC/cửa phải theo thiết kế PCCC và quy chuẩn có thẩm quyền.

## D. Trade-off

- Adapter bằng composition thêm một đối tượng trung gian, nhưng giảm phụ thuộc với API cũ.
- Observer cho phép mở rộng subscriber, nhưng cần xử lý ngoại lệ riêng cho từng subscriber. Chương trình bắt lỗi trong `Notify`, ghi lỗi để observer khác vẫn tiếp tục.
- Mediator giảm coupling giữa các bộ điều khiển, đổi lại mediator có thể thành nơi chứa quá nhiều nghiệp vụ nếu hệ thống phát triển. Phạm vi bài lab giữ mediator nhỏ.
- `EmailAlertObserver` chỉ mô phỏng qua Console đúng phạm vi đề bài, tránh tích hợp SMTP hay phụ thuộc dịch vụ bên ngoài.

## E. Tính chất được bảo đảm

- Không đăng ký cùng một observer hai lần trên một monitor.
- Cho phép hủy đăng ký.
- Chụp danh sách `ToArray()` trước khi phát thông báo, để observer tự hủy đăng ký không làm hỏng vòng lặp.
- `AlertEvent` là `sealed record`, dữ liệu không sửa đổi sau khi tạo.
- Lỗi của một observer không chặn những observer khác.
- Không tạo cảnh báo dưới 50%.
- `EmergencyBridgeObserver` chỉ thông báo mediator khi CRITICAL.
