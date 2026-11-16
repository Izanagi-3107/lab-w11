/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Mediator điều phối ba bộ điều khiển, tránh phụ thuộc trực tiếp giữa chúng.
using CampusSafetyHub.Core;

namespace CampusSafetyHub.Mediator;

public sealed class EmergencyMediator : IEmergencyMediator
{
    private readonly Alarm alarm;
    private readonly DoorController door;
    private readonly VentilationController ventilation;

    public EmergencyMediator(
        Alarm alarm,
        DoorController door,
        VentilationController ventilation)
    {
        this.alarm = alarm ?? throw new ArgumentNullException(nameof(alarm));
        this.door = door ?? throw new ArgumentNullException(nameof(door));
        this.ventilation = ventilation ?? throw new ArgumentNullException(nameof(ventilation));
    }

    public void Notify(EmergencySignal signal, AlertEvent alertEvent)
    {
        ArgumentNullException.ThrowIfNull(alertEvent);

        if (signal != EmergencySignal.FireDetected || alertEvent.Severity != "CRITICAL")
        {
            return;
        }

        // Giả định ứng phó dùng cho mô phỏng, không phải quy trình PCCC thực tế.
        Console.WriteLine($"[MEDIATOR] FIRE_DETECTED at {alertEvent.SensorName}.");
        alarm.Activate();
        door.Unlock();
        ventilation.Stop();
    }
}
