using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Chronos.WebView
{
    internal static class Program
    {
        private const string ServerBaseUri = "http://localhost:5000/";
        private static readonly TimeSpan ProcessExitTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan HttpTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var aspExePath = Path.Combine(AppContext.BaseDirectory, @"..\Chronos.Asp\Chronos.Asp.exe");

            if(!TryStartAspServer(aspExePath, out var aspProcess))
            {
                return;
            }

            try
            {
                Application.Run(new MainForm(ServerBaseUri));
            }
            finally
            {
                StopAspServer(aspProcess);
            }
        }

        private static bool TryStartAspServer(string aspExePath, out Process? aspProcess)
        {
            try
            {
                aspProcess = StartAspServer(aspExePath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to start the web server process at '{aspExePath}':\n\n{ex.Message}",
                    "Chronos - Startup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                aspProcess = null;
                return false;
            }
        }

        private static Process StartAspServer(string exePath)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(exePath) ?? AppContext.BaseDirectory
            };

            var process = Process.Start(startInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Process.Start returned no process instance.");
            }

            return process;
        }

        private static void StopAspServer(Process? process)
        {
            if (process == null || process.HasExited)
            {
                return;
            }

            try
            {
                StopActiveTrackingViaApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to gracefully stop tracking via the web server API:\n\n{ex.Message}",
                    "Chronos - Shutdown Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            try
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                    process.WaitForExit((int)ProcessExitTimeout.TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to terminate the web server process:\n\n{ex.Message}",
                    "Chronos - Shutdown Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void StopActiveTrackingViaApi()
        {
            using var client = new HttpClient { Timeout = HttpTimeout };

            var payload = JsonSerializer.Serialize(new { end = TimeOnly.FromDateTime(DateTime.Now) });
            using var content = new StringContent(payload, Encoding.UTF8, "application/json");

            using var response = client.PostAsync(new Uri(new Uri(ServerBaseUri), "api/tracking/stop"), content)
                .GetAwaiter()
                .GetResult();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Server responded with status code {(int)response.StatusCode} ({response.ReasonPhrase}).");
            }
        }
    }
}
