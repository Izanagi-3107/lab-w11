/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Giao diện điều phối phản ứng với các tín hiệu khẩn cấp.
using CampusSafetyHub.Core;

namespace CampusSafetyHub.Mediator;

public enum EmergencySignal
{
    FireDetected
}

public interface IEmergencyMediator
{
    void Notify(EmergencySignal signal, AlertEvent alertEvent);
}
