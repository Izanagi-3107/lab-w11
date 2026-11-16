/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Adapter dùng composition để chuyển API cũ sang ISensor.
using CampusSafetyHub.Core;
using CampusSafetyHub.Legacy;

namespace CampusSafetyHub.Adapter;

public sealed class SmokeSensorAdapter : ISensor
{
    private readonly LegacySmokeDetector legacy;

    public SmokeSensorAdapter(LegacySmokeDetector legacy)
    {
        this.legacy = legacy ?? throw new ArgumentNullException(nameof(legacy));
    }

    public double ReadValue()
    {
        // Cảm biến cũ trả 0..1000, chia 10 để lấy phần trăm.
        return legacy.GetSmokeLevel() / 10.0;
    }

    public string GetUnit() => "% smoke";
}
