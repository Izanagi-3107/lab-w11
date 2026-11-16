# Lab W11 – Campus Safety Hub

**Môn học:** MI4090 – Lập trình hướng đối tượng  
**Sinh viên:** Ngô Ngọc Thái  
**MSSV:** 202418984  
**Ngôn ngữ:** C# – .NET 8 Console

## Link GitHub mã nguồn

**Đường dẫn repository:** Dán URL GitHub vào đây sau khi tạo repository.

## 1. Giới thiệu

Mô phỏng trung tâm giám sát an toàn cho phòng máy. Hệ thống đọc mức khói từ cảm biến cũ, phát cảnh báo đến nhiều đối tượng theo dõi và điều phối thiết bị phản ứng khi cảnh báo CRITICAL. Chương trình không dùng GUI, cơ sở dữ liệu, mạng hoặc email thật.

## 2. Các mẫu thiết kế

| Design Pattern | Thành phần | Mục đích |
|---|---|---|
| Adapter | `SmokeSensorAdapter` | Tương thích API của `LegacySmokeDetector` với `ISensor` và chuyển thang đo. |
| Observer | `SensorMonitor`, `IAlertObserver` và các observer | Một cảnh báo có thể gửi đến nhiều bên; đăng ký/hủy đăng ký động. |
| Mediator | `IEmergencyMediator`, `EmergencyMediator` | Điều phối Alarm, DoorController, VentilationController mà không liên kết trực tiếp các thiết bị với nhau. |

## 3. Chức năng

- Đọc khói 0–1000 và chuyển đổi thành 0–100%.
- Dưới 50%: bình thường; từ 50% đến dưới 70%: WARNING; từ 70%: CRITICAL.
- Thông báo Console, mô phỏng email và ghi audit log.
- Đăng ký/hủy observer, tránh đăng ký trùng, tiếp tục phát tin dù một observer lỗi.
- Khi CRITICAL: chuông báo bật, cửa thoát hiểm mở khóa, thông gió dừng (giả định bài lab).
- Kiểm thử tự động các trường hợp biên và lỗi.

## 4. Cấu trúc project

```text
LabW11_CampusSafetyHub/
├── CampusSafetyHub.csproj
├── .github/workflows/dotnet.yml
├── Program.cs
├── Core/
│   ├── ISensor.cs
│   └── AlertEvent.cs
├── Legacy/
│   └── LegacySmokeDetector.cs
├── Adapter/
│   └── SmokeSensorAdapter.cs
├── Observer/
│   ├── IAlertObserver.cs
│   ├── SensorMonitor.cs
│   ├── DashboardObserver.cs
│   ├── EmailAlertObserver.cs
│   ├── AuditLogObserver.cs
│   └── EmergencyBridgeObserver.cs
├── Mediator/
│   ├── IEmergencyMediator.cs
│   ├── EmergencyMediator.cs
│   ├── Alarm.cs
│   ├── DoorController.cs
│   └── VentilationController.cs
├── Tests/
│   └── LabTests.cs
└── docs/
    ├── class-diagram.md
    ├── design-decisions.md
    └── test-cases.md
```

## 5. Cách chạy

Cài đặt [.NET SDK 8.0 trở lên](https://dotnet.microsoft.com/download). Mở Terminal tại thư mục gốc project:

```bash
dotnet restore
dotnet run
```

Chạy kiểm thử:

```bash
dotnet run -- --test
```

Chương trình không cần cài thêm package NuGet ngoài .NET SDK.

Khi đẩy project lên GitHub, workflow tại `.github/workflows/dotnet.yml` tự động build và chạy các bài kiểm thử trong tab **Actions**.

## 6. Kết quả mong đợi khi chạy demo

- `200` được đọc thành 20%, không phát cảnh báo.
- `600` được đọc thành 60%, phát WARNING đến các observer.
- `735` được đọc thành 73,5%, phát CRITICAL và kích hoạt mediator.
- Sau khi hủy đăng ký observer mô phỏng email, kiểm tra tiếp không tạo thêm email mới.
- Hệ thống phản ứng cuối cùng: chuông bật, cửa đã mở khóa, thông gió dừng.

## 7. Tài liệu thiết kế và kiểm thử

- [Sơ đồ UML và luồng tương tác](docs/class-diagram.md)
- [Phân tích trách nhiệm, các quyết định và trade-off](docs/design-decisions.md)
- [Bộ trường hợp kiểm thử](docs/test-cases.md)

## 8. Hạn chế

Mediator xử lý một kịch bản cháy khói đơn giản và các lệnh ứng phó được giả định cho mục tiêu học Pattern; không thay thế thiết kế PCCC thực tế. Email chỉ mô phỏng. Dữ liệu audit log ở bộ nhớ nên không tồn tại sau khi thoát chương trình. Ứng dụng Console chưa xử lý đồng thời nhiều luồng cập nhật cảm biến.

## 9. Yêu cầu nộp bài của giảng viên

- Mỗi file `.cs` có phần comment đầu file ghi họ tên và mã sinh viên, cùng comment giải thích các đoạn logic quan trọng.
- Upload **nội dung thư mục project**, gồm `.csproj`, mã nguồn, tài liệu và `.github/workflows/`. Không cần upload `bin/` hoặc `obj/`.
- Nộp một báo cáo Word `Bao_cao_LabW11_Ngo_Ngoc_Thai_202418984.docx`, trong đó cập nhật URL GitHub và bổ sung hình chụp kết quả chạy thực tế sau khi chạy project.
- Chạy `dotnet run` và `dotnet run -- --test`, chụp màn hình Terminal để hoàn thiện minh chứng kiểm thử.

**Ghi chú:** Do không có .NET SDK trong môi trường biên soạn, bảng đầu ra trong báo cáo được ghi rõ là *kết quả dự kiến*; chưa được coi là kết quả kiểm thử thực tế.
