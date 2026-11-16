/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Observer lưu các cảnh báo trong bộ nhớ để kiểm tra lịch sử.
using CampusSafetyHub.Core;

namespace CampusSafetyHub.Observer;

public sealed class AuditLogObserver : IAlertObserver
{
    private readonly List<AlertEvent> entries = new();

    public IReadOnlyList<AlertEvent> Entries => entries.AsReadOnly();

    public void Update(AlertEvent alertEvent)
    {
        entries.Add(alertEvent);
        Console.WriteLine(
            $"[AUDIT] {alertEvent.OccurredAt:yyyy-MM-dd HH:mm:ss} " +
            $"{alertEvent.SensorName} {alertEvent.Severity} {alertEvent.Value:F1}%");
    }
}
