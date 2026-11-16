/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Observer mô phỏng gửi email; không sử dụng dịch vụ email thật.
using CampusSafetyHub.Core;

namespace CampusSafetyHub.Observer;

public sealed class EmailAlertObserver : IAlertObserver
{
    private readonly List<AlertEvent> messages = new();

    public IReadOnlyList<AlertEvent> Messages => messages.AsReadOnly();

    public void Update(AlertEvent alertEvent)
    {
        messages.Add(alertEvent);
        Console.WriteLine(
            $"[EMAIL DEMO] {alertEvent.Severity}: {alertEvent.SensorName} " +
            $"({alertEvent.Value:F1}{alertEvent.Unit})");
    }
}
