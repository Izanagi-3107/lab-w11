/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Subject: quản lý observer, so sánh ngưỡng và phát cảnh báo.
using CampusSafetyHub.Core;

namespace CampusSafetyHub.Observer;

public sealed class SensorMonitor
{
    private readonly List<IAlertObserver> observers = new();
    private readonly List<string> notificationErrors = new();

    public IReadOnlyList<string> NotificationErrors => notificationErrors.AsReadOnly();

    public void Attach(IAlertObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void Detach(IAlertObserver observer)
    {
        observers.Remove(observer);
    }

    public void CheckSensor(string sensorName, ISensor sensor)
    {
        if (string.IsNullOrWhiteSpace(sensorName))
        {
            throw new ArgumentException("Sensor name cannot be empty.", nameof(sensorName));
        }

        ArgumentNullException.ThrowIfNull(sensor);

        // Ngưỡng nghiệp vụ: <50 bình thường, [50,70) WARNING, >=70 CRITICAL.
        double value = sensor.ReadValue();
        string? severity = value switch
        {
            >= 70 => "CRITICAL",
            >= 50 => "WARNING",
            _ => null
        };

        if (severity is null)
        {
            return;
        }

        var alertEvent = new AlertEvent(
            sensorName,
            value,
            sensor.GetUnit(),
            severity,
            DateTime.Now);

        Notify(alertEvent);
    }

    private void Notify(AlertEvent alertEvent)
    {
        // Sao chép danh sách để observer có thể tự hủy đăng ký khi nhận cảnh báo.
        foreach (IAlertObserver observer in observers.ToArray())
        {
            try
            {
                observer.Update(alertEvent);
            }
            // Một observer lỗi không được ngăn các observer còn lại nhận tin.
            catch (Exception ex)
            {
                string message = $"{observer.GetType().Name}: {ex.Message}";
                notificationErrors.Add(message);
                Console.Error.WriteLine($"[OBSERVER ERROR] {message}");
            }
        }
    }
}
