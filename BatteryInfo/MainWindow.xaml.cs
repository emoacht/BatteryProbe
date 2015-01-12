﻿using Microsoft.Win32;
using System;
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

namespace BatteryInfo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public BatteryStatus Status { get; } = new BatteryStatus();
        private DispatcherTimer updateTimer;

        protected override async void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            await UpdateAsync();

            updateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
            updateTimer.Tick += OnUpdateTimerTick;
            updateTimer.Start();

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

        private string batteryLifePercentOld;
        private const string recordFileName = "record.csv";
        private static readonly DateTime dateTimeZero = new DateTime(DateTime.Today.Year, 1, 1);

        private async Task UpdateAsync()
        {
            Status.Update();

            var batteryLifePercentNew = Status.BatteryLifePercent;

            Trace.WriteLine("BatteryLifePercent: \{batteryLifePercentNew}"); // To be changed in later version

            // Recond log if BatteryLifePercent is changed.
            if (batteryLifePercentOld == batteryLifePercentNew)
                return;

            batteryLifePercentOld = batteryLifePercentNew;

            var dateTimeNow = DateTime.Now;
            var content = String.Format(@"""{0:yyyy MM/dd HH:mm:ss}"",{1},{2},{3}",
                dateTimeNow,
                (long)((dateTimeNow - dateTimeZero).TotalSeconds),
                Status.BatteryLifePercent,
                Status.BatteryChargeStatus);

            var recordFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, recordFileName);

            try
            {
                using (var sw = new StreamWriter(recordFilePath, true)) // Append
                    await sw.WriteAsync(content + Environment.NewLine);
            }
            catch (UnauthorizedAccessException)
            {
                recordFilePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Assembly.GetExecutingAssembly().GetName().Name,
                    recordFileName);

                using (var sw = new StreamWriter(recordFilePath, true)) // Append
                    await sw.WriteAsync(content + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to record log.\r\n\{ex}"); // To be changed in later version
            }
        }

        #endregion
    }
}