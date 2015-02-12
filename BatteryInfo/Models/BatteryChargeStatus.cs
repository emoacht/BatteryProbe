using System;

namespace BatteryInfo.Models
{
	/// <summary>
	/// Battery charge status
	/// </summary>
	/// <remarks>
	/// Except Middle, these statuses are equivalent to:
	/// System.Windows.Forms.BatteryChargeStatus enumeration
	/// BatteryFlag values in SYSTEM_POWER_STATUS structure
	/// </remarks>
	[Flags]
	public enum BatteryChargeStatus
	{
		/// <summary>
		/// Indicates a high level of battery charge (more than 66 percent).
		/// </summary>
		High = 1,

		/// <summary>
		/// Indicates a middle level of battery charge (between low and high).
		/// </summary>
		/// <remarks>In BatteryFlag, this value is 0.</remarks>
		Middle = 2,

		/// <summary>
		/// Indicates a low level of battery charge (less than 33 percent).
		/// </summary>
		/// <remarks>In BatteryFlag, this value is 2.</remarks>
		Low = 4,

		/// <summary>
		/// Indicates a critically low level of battery charge (less than 5 percent).
		/// </summary>
		/// <remarks>In BatteryFlag, this value is 4.</remarks>
		Critical = 8,

		/// <summary>
		/// Indicates a battery is charging.
		/// </summary>
		/// <remarks>In BatteryFlag, this value is 8.</remarks>
		Charging = 16,

		/// <summary>
		/// Indicates that no battery is present.
		/// </summary>
		NoSystemBattery = 128,

		/// <summary>
		/// Indicates an unknown battery condition.
		/// </summary>
		Unknown = 255
	}
}