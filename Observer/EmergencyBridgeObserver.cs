/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Cầu nối từ Observer sang Mediator khi có cảnh báo CRITICAL.
using CampusSafetyHub.Core;
using CampusSafetyHub.Mediator;

namespace CampusSafetyHub.Observer;

public sealed class EmergencyBridgeObserver : IAlertObserver
{
    private readonly IEmergencyMediator mediator;

    public EmergencyBridgeObserver(IEmergencyMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public void Update(AlertEvent alertEvent)
    {
        // WARNING chỉ thông báo; CRITICAL mới kích hoạt các thiết bị ứng phó.
        if (alertEvent.Severity == "CRITICAL")
        {
            mediator.Notify(EmergencySignal.FireDetected, alertEvent);
        }
    }
}
