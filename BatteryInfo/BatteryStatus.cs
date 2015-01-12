using BatteryInfo.Common;
using BatteryInfo.Models;

namespace BatteryInfo
{
    public class BatteryStatus : NotificationObject
    {
        public BatteryChargeStatus BatteryChargeStatus => PowerStatus.BatteryChargeStatus; // Flag

        public string BatteryFullLifetime => PowerStatus.BatteryFullLifetime?.ToString() ?? "Unknown";

        public string BatteryLifePercent => PowerStatus.BatteryLifePercent?.ToString() ?? "Unknown";

        public string BatteryLifeRemaining => PowerStatus.BatteryLifeRemaining?.ToString() ?? "Unknown";

        public PowerLineStatus PowerLineStatus => PowerStatus.PowerLineStatus;

        public bool BatteryIsCharging => BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging);

        internal void Update()
        {
            RaisePropertyChanged(nameof(BatteryChargeStatus));
            RaisePropertyChanged(nameof(BatteryFullLifetime));
            RaisePropertyChanged(nameof(BatteryLifePercent));
            RaisePropertyChanged(nameof(BatteryLifeRemaining));
            RaisePropertyChanged(nameof(PowerLineStatus));
            RaisePropertyChanged(nameof(BatteryIsCharging));
        }
    }
}