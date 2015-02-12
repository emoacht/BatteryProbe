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
				UpdateSystemPowerStatus();

				if (systemPowerStatus.BatteryFlag.HasFlag(BatteryFlag.NoSystemBattery))
					return BatteryChargeStatus.NoSystemBattery;

				if (systemPowerStatus.BatteryFlag.HasFlag(BatteryFlag.Unknown))
					return BatteryChargeStatus.Unknown;

				if (systemPowerStatus.BatteryLifePercent > 100)
					return BatteryChargeStatus.Unknown;

				BatteryChargeStatus status;
				if (systemPowerStatus.BatteryLifePercent > 66)
					status = BatteryChargeStatus.High;
				else if (systemPowerStatus.BatteryLifePercent >= 33)
					status = BatteryChargeStatus.Middle;
				else if (systemPowerStatus.BatteryLifePercent >= 5)
					status = BatteryChargeStatus.Low;
				else
					status = BatteryChargeStatus.Critical;

				if (systemPowerStatus.BatteryFlag.HasFlag(BatteryFlag.Charging))
					status |= BatteryChargeStatus.Charging;

				return status;
			}
		}

		/// <summary>
		/// Number of seconds of battery life when at full charge
		/// </summary>
		public static int? BatteryFullLifetime
		{
			get
			{
				UpdateSystemPowerStatus();

				var buff = systemPowerStatus.BatteryFullLifeTime;

				return (buff >= 0) ? buff : (int?)null;	// Original value will be -1 if unknown.
			}
		}

		/// <summary>
		/// Percentage of full battery charge remaining
		/// </summary>
		public static float? BatteryLifePercent
		{
			get
			{
				UpdateSystemPowerStatus();

				var buff = (int)systemPowerStatus.BatteryLifePercent;

				return (buff <= 100) ? buff / 100f : (float?)null; // Original value will be 255 if unknown.
			}
		}

		/// <summary>
		/// Number of seconds of battery life remaining
		/// </summary>
		public static int? BatteryLifeRemaining
		{
			get
			{
				UpdateSystemPowerStatus();

				var buff = systemPowerStatus.BatteryLifeTime;

				return (buff >= 0) ? buff : (int?)null;	// Original value will be -1 if unknown.
			}
		}

		/// <summary>
		/// AC power status
		/// </summary>
		public static PowerLineStatus PowerLineStatus
		{
			get
			{
				UpdateSystemPowerStatus();

				return (PowerLineStatus)systemPowerStatus.ACLineStatus;
			}
		}

		#endregion


		private static SYSTEM_POWER_STATUS systemPowerStatus;

		private static void UpdateSystemPowerStatus()
		{
			GetSystemPowerStatus(ref systemPowerStatus);
		}
	}
}