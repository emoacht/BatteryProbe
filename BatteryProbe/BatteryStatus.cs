using BatteryProbe.Common;
using BatteryProbe.Models;

namespace BatteryProbe
{
	public class BatteryStatus : BindableBase
	{
		public BatteryChargeStatus BatteryChargeStatus => PowerStatus.BatteryChargeStatus; // Flag
		public int? BatteryFullLifetime => PowerStatus.BatteryFullLifetime;
		public float? BatteryLifePercent => PowerStatus.BatteryLifePercent;
		public int? BatteryLifeRemaining => PowerStatus.BatteryLifeRemaining;
		public bool BatteryIsCharging => BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging);
		public PowerLineStatus PowerLineStatus => PowerStatus.PowerLineStatus;

		internal void Update()
		{
			RaisePropertyChanged(nameof(BatteryChargeStatus));
			RaisePropertyChanged(nameof(BatteryFullLifetime));
			RaisePropertyChanged(nameof(BatteryLifePercent));
			RaisePropertyChanged(nameof(BatteryLifeRemaining));
			RaisePropertyChanged(nameof(BatteryIsCharging));
			RaisePropertyChanged(nameof(PowerLineStatus));
		}
	}
}