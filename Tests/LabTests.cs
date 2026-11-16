/**************************************
 * Mã sinh viên: 202418984
 * Họ tên: Ngô Ngọc Thái
 **************************************/

// Bộ kiểm thử độc lập cho Adapter, Observer, Mediator và trường hợp biên.
using CampusSafetyHub.Adapter;
using CampusSafetyHub.Core;
using CampusSafetyHub.Legacy;
using CampusSafetyHub.Mediator;
using CampusSafetyHub.Observer;

namespace CampusSafetyHub.Tests;

public static class LabTests
{
    // Mỗi tình huống kiểm thử trả về thành công hoặc ném ngoại lệ nếu sai.
    private static readonly (string Name, Action Test)[] Cases =
    {
        ("Adapter converts 735 to 73.5%", AdapterConverts),
        ("Adapter reads live legacy state", AdapterReadsCurrentState),
        ("Legacy setter clamps values to 0..1000", LegacyClamps),
        ("Below 50% gives no alert", BelowThreshold),
        ("50% is WARNING", WarningLowerBoundary),
        ("69.9% is WARNING", WarningUpperBoundary),
        ("70% is CRITICAL", CriticalLowerBoundary),
        ("An observer is registered only once", DuplicateAttachment),
        ("Detached observer receives nothing", DetachStopsNotification),
        ("Different observers receive the same alert", MultipleObservers),
        ("Failing observer does not stop another", ObserverFailureIsolation),
        ("Detaching during notification is safe", DetachDuringNotification),
        ("Audit log records separate alerts", AuditHistory),
        ("WARNING does not activate emergency", WarningDoesNotTriggerEmergency),
        ("CRITICAL coordinates alarm door and ventilation", CriticalCoordinatesDevices),
        ("Email observer only simulates delivery", EmailIsSimulation),
        ("Adapter rejects null legacy detector", AdapterRejectsNull),
        ("Monitor rejects blank name", MonitorRejectsBlankName)
    };

    public static int Run()
    {
        int passed = 0;
        Console.WriteLine("LAB W11 - AUTOMATED TESTS");

        // Chạy mọi tình huống, không dừng khi gặp một kiểm thử thất bại.
        foreach (var (name, test) in Cases)
        {
            try
            {
                test();
                passed++;
                Console.WriteLine($"[PASS] {name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] {name}: {ex.Message}");
            }
        }

        Console.WriteLine($"Result: {passed}/{Cases.Length} passed.");
        return passed == Cases.Length ? 0 : 1;
    }

    private static void AdapterConverts()
    {
        ISensor sensor = new SmokeSensorAdapter(new LegacySmokeDetector(735));
        Equal(73.5, sensor.ReadValue());
        Check(sensor.GetUnit() == "% smoke");
    }

    private static void AdapterReadsCurrentState()
    {
        var legacy = new LegacySmokeDetector(500);
        ISensor sensor = new SmokeSensorAdapter(legacy);
        Equal(50.0, sensor.ReadValue());
        legacy.SetSmokeLevel(810);
        Equal(81.0, sensor.ReadValue());
    }

    private static void LegacyClamps()
    {
        var legacy = new LegacySmokeDetector(100);
        legacy.SetSmokeLevel(-10);
        Check(legacy.GetSmokeLevel() == 0);
        legacy.SetSmokeLevel(2000);
        Check(legacy.GetSmokeLevel() == 1000);
    }

    private static void BelowThreshold()
    {
        var observer = Capture(499);
        Check(observer.Events.Count == 0);
    }

    private static void WarningLowerBoundary()
    {
        var observer = Capture(500);
        Check(observer.Events.Count == 1);
        Check(observer.Events[0].Severity == "WARNING");
    }

    private static void WarningUpperBoundary()
    {
        var observer = Capture(699);
        Check(observer.Events.Count == 1);
        Check(observer.Events[0].Severity == "WARNING");
    }

    private static void CriticalLowerBoundary()
    {
        var observer = Capture(700);
        Check(observer.Events.Count == 1);
        Check(observer.Events[0].Severity == "CRITICAL");
    }

    private static void DuplicateAttachment()
    {
        var monitor = new SensorMonitor();
        var observer = new RecordingObserver();
        monitor.Attach(observer);
        monitor.Attach(observer);
        monitor.CheckSensor("Lab", new SmokeSensorAdapter(new LegacySmokeDetector(750)));
        Check(observer.Events.Count == 1);
    }

    private static void DetachStopsNotification()
    {
        var monitor = new SensorMonitor();
        var observer = new RecordingObserver();
        monitor.Attach(observer);
        monitor.Detach(observer);
        monitor.CheckSensor("Lab", new SmokeSensorAdapter(new LegacySmokeDetector(750)));
        Check(observer.Events.Count == 0);
    }

    private static void MultipleObservers()
    {
        var monitor = new SensorMonitor();
        var first = new RecordingObserver();
        var second = new RecordingObserver();
        monitor.Attach(first);
        monitor.Attach(second);
        monitor.CheckSensor("Lab", new SmokeSensorAdapter(new LegacySmokeDetector(600)));
        Check(first.Events.Count == 1 && second.Events.Count == 1);
        Check(ReferenceEquals(first.Events[0], second.Events[0]));
    }

    private static void ObserverFailureIsolation()
    {
        var monitor = new SensorMonitor();
        var healthy = new RecordingObserver();
        monitor.Attach(new FailingObserver());
        monitor.Attach(healthy);
        monitor.CheckSensor("Lab", new SmokeSensorAdapter(new LegacySmokeDetector(600)));
        Check(monitor.NotificationErrors.Count == 1);
        Check(healthy.Events.Count == 1);
    }

    private static void DetachDuringNotification()
    {
        var monitor = new SensorMonitor();
        var healthy = new RecordingObserver();
        var selfRemoving = new SelfRemovingObserver(monitor);
        monitor.Attach(selfRemoving);
        monitor.Attach(healthy);
        var sensor = new SmokeSensorAdapter(new LegacySmokeDetector(600));
        monitor.CheckSensor("Lab", sensor);
        monitor.CheckSensor("Lab", sensor);
        Check(selfRemoving.Count == 1 && healthy.Events.Count == 2);
    }

    private static void AuditHistory()
    {
        var monitor = new SensorMonitor();
        var audit = new AuditLogObserver();
        var legacy = new LegacySmokeDetector(500);
        var sensor = new SmokeSensorAdapter(legacy);
        monitor.Attach(audit);
        monitor.CheckSensor("Lab", sensor);
        legacy.SetSmokeLevel(700);
        monitor.CheckSensor("Lab", sensor);
        Check(audit.Entries.Count == 2);
        Check(audit.Entries[0].Severity == "WARNING");
        Check(audit.Entries[1].Severity == "CRITICAL");
    }

    private static void WarningDoesNotTriggerEmergency()
    {
        var (monitor, alarm, door, ventilation) = SetupEmergency();
        monitor.CheckSensor("Lab", new SmokeSensorAdapter(new LegacySmokeDetector(699)));
        Check(!alarm.IsActive && door.IsLocked && ventilation.IsRunning);
    }

    private static void CriticalCoordinatesDevices()
    {
        var (monitor, alarm, door, ventilation) = SetupEmergency();
        monitor.CheckSensor("Lab", new SmokeSensorAdapter(new LegacySmokeDetector(700)));
        Check(alarm.IsActive && !door.IsLocked && !ventilation.IsRunning);
    }

    private static void EmailIsSimulation()
    {
        var monitor = new SensorMonitor();
        var email = new EmailAlertObserver();
        monitor.Attach(email);
        monitor.CheckSensor("Lab", new SmokeSensorAdapter(new LegacySmokeDetector(750)));
        Check(email.Messages.Count == 1);
    }

    private static void AdapterRejectsNull()
    {
        Throws<ArgumentNullException>(() => new SmokeSensorAdapter(null!));
    }

    private static void MonitorRejectsBlankName()
    {
        var monitor = new SensorMonitor();
        Throws<ArgumentException>(
            () => monitor.CheckSensor("  ", new SmokeSensorAdapter(new LegacySmokeDetector(600))));
    }

    private static RecordingObserver Capture(int rawValue)
    {
        var observer = new RecordingObserver();
        var monitor = new SensorMonitor();
        monitor.Attach(observer);
        monitor.CheckSensor("Lab", new SmokeSensorAdapter(new LegacySmokeDetector(rawValue)));
        return observer;
    }

    private static (SensorMonitor Monitor, Alarm Alarm, DoorController Door,
        VentilationController Ventilation) SetupEmergency()
    {
        var alarm = new Alarm();
        var door = new DoorController();
        var ventilation = new VentilationController();
        IEmergencyMediator mediator = new EmergencyMediator(alarm, door, ventilation);
        var monitor = new SensorMonitor();
        monitor.Attach(new EmergencyBridgeObserver(mediator));
        return (monitor, alarm, door, ventilation);
    }

    private static void Check(bool condition)
    {
        if (!condition)
        {
            throw new InvalidOperationException("Assertion failed.");
        }
    }

    private static void Equal(double expected, double actual)
    {
        Check(Math.Abs(expected - actual) < 0.000001);
    }

    private static void Throws<TException>(Action action) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
    }

    // Các observer giả lập chỉ được sử dụng trong kiểm thử.
    private sealed class RecordingObserver : IAlertObserver
    {
        public List<AlertEvent> Events { get; } = new();

        public void Update(AlertEvent alertEvent)
        {
            Events.Add(alertEvent);
        }
    }

    private sealed class FailingObserver : IAlertObserver
    {
        public void Update(AlertEvent alertEvent)
        {
            throw new InvalidOperationException("Simulated failure");
        }
    }

    private sealed class SelfRemovingObserver : IAlertObserver
    {
        private readonly SensorMonitor monitor;
        public int Count { get; private set; }

        public SelfRemovingObserver(SensorMonitor monitor)
        {
            this.monitor = monitor;
        }

        public void Update(AlertEvent alertEvent)
        {
            Count++;
            monitor.Detach(this);
        }
    }
}
