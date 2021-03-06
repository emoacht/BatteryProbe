﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;

namespace BatteryProbe
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public BatteryStatus Status { get; } = new BatteryStatus();
		private DispatcherTimer _updateTimer;

		protected override async void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			await UpdateAsync();

			_updateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
			_updateTimer.Tick += OnUpdateTimerTick;
			_updateTimer.Start();

			SystemEvents.PowerModeChanged += OnPowerModeChanged;
		}

		private async void OnUpdateTimerTick(object sender, EventArgs e)
		{
			await UpdateAsync();
		}

		private async void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			await UpdateAsync();
		}

		protected override void OnClosed(EventArgs e)
		{
			SystemEvents.PowerModeChanged -= OnPowerModeChanged;

			base.OnClosed(e);
		}

		#region Update

		private const string _fileName = "record.csv";

		private readonly string _filePathWorkingFolder = Path.Combine(
			AppDomain.CurrentDomain.BaseDirectory,
			_fileName);

		private readonly string _filePathAppDataFolder = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			Assembly.GetExecutingAssembly().GetName().Name,
			_fileName);

		private float? _batteryLifePercentOld;
		private static readonly DateTime _zeroTime = new DateTime(DateTime.Today.Year, 1, 1);

		private async Task UpdateAsync()
		{
			Status.Update();

			var batteryLifePercentNew = Status.BatteryLifePercent;

			Trace.WriteLine($"BatteryLifePercent: {batteryLifePercentNew}");

			// Record log if BatteryLifePercent is changed.
			if (_batteryLifePercentOld == batteryLifePercentNew)
				return;

			_batteryLifePercentOld = batteryLifePercentNew;

			var currentTime = DateTime.Now;
			var content = string.Format(@"""{0:yyyy MM/dd HH:mm:ss}"",{1},{2},{3}",
				currentTime,
				(long)((currentTime - _zeroTime).TotalSeconds),
				Status.BatteryLifePercent?.ToString("f2") ?? "Unknown",
				Status.BatteryChargeStatus);

			try
			{
				using (var sw = new StreamWriter(_filePathWorkingFolder, true)) // Append
					await sw.WriteAsync(content + Environment.NewLine);
			}
			catch (UnauthorizedAccessException)
			{
				using (var sw = new StreamWriter(_filePathAppDataFolder, true)) // Append
					await sw.WriteAsync(content + Environment.NewLine);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Failed to record log." + Environment.NewLine + ex);
			}
		}

		#endregion
	}
}