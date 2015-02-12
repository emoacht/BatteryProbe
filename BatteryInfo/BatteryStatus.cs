using BatteryInfo.Common;
using BatteryInfo.Models;

namespace BatteryInfo
{
	public class BatteryStatus : NotificationObject
	{
		public BatteryChargeStatus BatteryChargeStatus => PowerStatus.BatteryChargeStatus; // Flag

		public string BatteryFullLifetime => PowerStatus.BatteryFullLifetime?.ToString() ?? UnknownString;

		public string BatteryLifePercent => PowerStatus.BatteryLifePercent?.ToString("f2") ?? UnknownString;

		public string BatteryLifeRemaining => PowerStatus.BatteryLifeRemaining?.ToString() ?? UnknownString;

		public PowerLineStatus PowerLineStatus => PowerStatus.PowerLineStatus;

		public bool BatteryIsCharging => BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging);

		internal string UnknownString { get; set; } = "Unknown";

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