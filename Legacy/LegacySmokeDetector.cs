/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Mô phỏng API cảm biến đời cũ; giữ nguyên cách đọc và cập nhật giá trị.
namespace CampusSafetyHub.Legacy;

public sealed class LegacySmokeDetector
{
    private int smokeLevel;

    public LegacySmokeDetector(int smokeLevel)
    {
        this.smokeLevel = smokeLevel;
    }

    public int GetSmokeLevel() => smokeLevel;

    public string GetDescription() => "Legacy smoke detector";

    public void SetSmokeLevel(int value)
    {
        smokeLevel = Math.Clamp(value, 0, 1000);
    }
}
