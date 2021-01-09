
namespace BatteryProbe.Models
{
	/// <summary>
	/// AC power status
	/// </summary>
	/// <remarks>
	/// These statuses are equivalent to:
	/// System.Windows.Forms.PowerLineStatus
	/// </remarks>
	public enum PowerLineStatus
	{
		/// <summary>
		/// AC power status is offline.
		/// </summary>
		Offline = 0,

		/// <summary>
		/// AC power status is online.
		/// </summary>
		Online = 1,

		/// <summary>
		/// AC power status is unknown.
		/// </summary>
		Unknown = 255
	}
}