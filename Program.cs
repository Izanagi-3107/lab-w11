/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Điểm vào chương trình: trình diễn nghiệp vụ hoặc chạy kiểm thử.
using System.Text;
using CampusSafetyHub.Adapter;
using CampusSafetyHub.Core;
using CampusSafetyHub.Legacy;
using CampusSafetyHub.Mediator;
using CampusSafetyHub.Observer;
using CampusSafetyHub.Tests;

namespace CampusSafetyHub;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Contains("--test", StringComparer.OrdinalIgnoreCase))
        {
            Environment.ExitCode = LabTests.Run();
            return;
        }

        RunDemo();
    }

    private static void RunDemo()
    {
        Console.WriteLine("LAB W11 - CAMPUS SAFETY HUB");
        Console.WriteLine("Ngo Ngoc Thai - 202418984");
        Console.WriteLine(new string('-', 45));

        // Chuẩn bị cảm biến cũ và bọc bằng Adapter.
        var legacy = new LegacySmokeDetector(200);
        ISensor sensor = new SmokeSensorAdapter(legacy);

        var alarm = new Alarm();
        var door = new DoorController();
        var ventilation = new VentilationController();
        IEmergencyMediator mediator = new EmergencyMediator(alarm, door, ventilation);

        var dashboard = new DashboardObserver();
        var email = new EmailAlertObserver();
        var auditLog = new AuditLogObserver();
        var emergencyBridge = new EmergencyBridgeObserver(mediator);

        // Đăng ký động bốn Observer cho bộ giám sát.
        var monitor = new SensorMonitor();
        monitor.Attach(dashboard);
        monitor.Attach(email);
        monitor.Attach(auditLog);
        monitor.Attach(emergencyBridge);
        monitor.Attach(dashboard);

        // Ba mức cảnh báo và tình huống hủy đăng ký email.
        Console.WriteLine("\n1. Normal: smoke 20.0%");
        monitor.CheckSensor("Computer Lab A203", sensor);

        Console.WriteLine("\n2. Warning: smoke 60.0%");
        legacy.SetSmokeLevel(600);
        monitor.CheckSensor("Computer Lab A203", sensor);

        Console.WriteLine("\n3. Critical: smoke 73.5%");
        legacy.SetSmokeLevel(735);
        monitor.CheckSensor("Computer Lab A203", sensor);

        Console.WriteLine("\n4. Remove email observer and check again");
        monitor.Detach(email);
        legacy.SetSmokeLevel(800);
        monitor.CheckSensor("Computer Lab A203", sensor);

        Console.WriteLine("\n5. Final state");
        Console.WriteLine($"Alarm active: {alarm.IsActive}");
        Console.WriteLine($"Exit door locked: {door.IsLocked}");
        Console.WriteLine($"Ventilation running: {ventilation.IsRunning}");
        Console.WriteLine($"Audit entries: {auditLog.Entries.Count}");
        Console.WriteLine($"Simulated emails: {email.Messages.Count}");
        Console.WriteLine("\nRun: dotnet run -- --test   to execute automated checks.");
    }
}
