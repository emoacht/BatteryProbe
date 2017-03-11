using System;
using System.Runtime.InteropServices;

namespace BatteryInfo.Models
{
	/// <summary>
	/// Alternate wrapper class for GetSystemPowerStatus function
	/// </summary>
	/// <remarks>
	/// The differences from System.Windows.Forms.PowerStatus:
	/// Added Middle status to BatteryChargeStatus.
	/// BatteryFullLifetime, BatteryLifePercent and BatteryLifeRemaining will return null if unknown.
	/// </remarks>
	public class PowerStatus
	{
		#region Win32

		[DllImport("Kernel32.dll", SetLastError = true)]
		private static extern bool GetSystemPowerStatus([In, Out] ref SYSTEM_POWER_STATUS systemPowerStatus);

		[StructLayout(LayoutKind.Sequential)]
		private struct SYSTEM_POWER_STATUS
		{
			public byte ACLineStatus;
			public BatteryFlag BatteryFlag;
			public byte BatteryLifePercent;
			public byte Reserved1;
			public int BatteryLifeTime;
			public int BatteryFullLifeTime;
		}

		[Flags]
		private enum BatteryFlag : byte
		{
			High = 1,
			Low = 2,
			Critical = 4,
			Charging = 8,
			NoSystemBattery = 128,
			Unknown = 255
		}

		#endregion

		#region Property

		/// <summary>
		/// Battery charge status
		/// </summary>
		public static BatteryChargeStatus BatteryChargeStatus
		{
			get
			{
				var powerStatus = SystemPowerStatus;

				if (powerStatus.BatteryFlag.HasFlag(BatteryFlag.NoSystemBattery))
					return BatteryChargeStatus.NoSystemBattery;

				if (powerStatus.BatteryFlag.HasFlag(BatteryFlag.Unknown))
					return BatteryChargeStatus.Unknown;

				if (powerStatus.BatteryLifePercent > 100)
					return BatteryChargeStatus.Unknown;

				BatteryChargeStatus chargeStatus;
				if (powerStatus.BatteryLifePercent > 66)
					chargeStatus = BatteryChargeStatus.High;
				else if (powerStatus.BatteryLifePercent >= 33)
					chargeStatus = BatteryChargeStatus.Middle;
				else if (powerStatus.BatteryLifePercent >= 5)
					chargeStatus = BatteryChargeStatus.Low;
				else
					chargeStatus = BatteryChargeStatus.Critical;

				if (powerStatus.BatteryFlag.HasFlag(BatteryFlag.Charging))
					chargeStatus |= BatteryChargeStatus.Charging;

				return chargeStatus;
			}
		}

		/// <summary>
		/// Number of seconds of battery life when at full charge
		/// </summary>
		public static int? BatteryFullLifetime
		{
			get
			{
				var value = SystemPowerStatus.BatteryFullLifeTime;

				return (value >= 0) ? value : (int?)null; // Original value will be -1 if unknown.
			}
		}

		/// <summary>
		/// Percentage of full battery charge remaining
		/// </summary>
		public static float? BatteryLifePercent
		{
			get
			{
				var value = SystemPowerStatus.BatteryLifePercent;

				return (value <= 100) ? value / 100f : (float?)null; // Original value will be 255 if unknown.
			}
		}

		/// <summary>
		/// Number of seconds of battery life remaining
		/// </summary>
		public static int? BatteryLifeRemaining
		{
			get
			{
				var value = SystemPowerStatus.BatteryLifeTime;

				return (value >= 0) ? value : (int?)null; // Original value will be -1 if unknown.
			}
		}

		/// <summary>
		/// AC power status
		/// </summary>
		public static PowerLineStatus PowerLineStatus => (PowerLineStatus)SystemPowerStatus.ACLineStatus;

		private static DateTime _lastTime = DateTime.MinValue;

		private static SYSTEM_POWER_STATUS SystemPowerStatus
		{
			get
			{
				var currentTime = DateTime.Now;
				if (_lastTime.AddMilliseconds(100) < currentTime)
				{
					_lastTime = currentTime;

					GetSystemPowerStatus(ref _systemPowerStatus);
				}
				return _systemPowerStatus;
			}
		}
		private static SYSTEM_POWER_STATUS _systemPowerStatus;

		#endregion
	}
}