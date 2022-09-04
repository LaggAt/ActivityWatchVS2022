using At.Lagg.ActivityWatchVS2022.Tools;
using Microsoft;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Documents;
using Microsoft.VisualStudio.Threading;

namespace At.Lagg.ActivityWatchVS2022.Services
{
    public class ConsoleService : ExtensionPart
    {
        #region Fields

        //TODO: use new URL once the new Support thread is here.
        private const string SUPPORT_THREAD_URL = @"https://tinyurl.com/yzg8aq4o";

        private const string COFFEE_URL = @"https://buymeacoffee.com/LaggAt";
        //private readonly TraceSource _logger;

        //TODO: define trace level in settings
        private LogLevel _logLevel = LogLevel.Trace;

        private OutputWindow? _outputWindow;

        #endregion Fields

        #region CTor

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "<Pending>")]
        public ConsoleService(ExtensionCore container, VisualStudioExtensibility extensibility) //, TraceSource traceListener
            : base(container, extensibility)
        {
            _ = Task.Run(this.InitializeAsync);
        }

        #endregion CTor

        #region Methods

        public async Task WriteLineDebugAsync(string str, params object?[] args)
        {
            await this.WriteLineAsync(LogLevel.Debug, str, args);
        }

        public async Task WriteLineInfoAsync(string str, params object?[] args)
        {
            await this.WriteLineAsync(LogLevel.Information, str, args);
        }

        public async Task WriteLineErrorAsync(string str, params object?[] args)
        {
            await this.WriteLineAsync(LogLevel.Error, str, args);
        }

        public async Task WriteLineAsync(LogLevel level, string str, params object?[] args)
        {
            if (level >= this._logLevel && _outputWindow != null)
            {
                string fullStr = $"{level}: {str}";
                await _outputWindow.Writer.WriteLineAsync(string.Format(fullStr, args));
            }
        }

        private async Task InitializeAsync()
        {
            _outputWindow =
                await this.Extensibility.Views().Output.GetChannelAsync(
                $"{nameof(ActivityWatchVS2022)}-{Guid.NewGuid()}",
                "EXTENSION_NAME",
                default
            );
            Requires.NotNull(_outputWindow, nameof(_outputWindow));

            await sayHelloAsync();
            await tellVersionAsync();
        }

        private async Task tellVersionAsync()
        {
            //TODO: read version from Package.
            string version = "0.0.TODO";
            if (_outputWindow != null)
            {
                await _outputWindow.Writer.WriteLineAsync($"ActivityWatchVS2022 v{version} ready.");
            }
        }

        private string GetDayTime(DateTime now)
        {
            int hour = now.Hour;
            if (hour >= 22 || hour < 6) { return "night"; }
            if (hour >= 17) { return "evening"; }
            if (hour >= 12) { return "afternoon"; }
            // hour >= 6 < 17
            return "morning";
        }

        /// <summary>
        /// Say hi. This method is probably too long. Well, a lot of effort to just say hi, but when
        /// do we really talk to our users? So let's enjoy this.
        /// </summary>
        private async Task sayHelloAsync()
        {
            var random = new Random();
            DateTime now = DateTime.Now;

            string greeting = string.Empty;

            switch (random.Next(3))
            {
                case 0:
                    greeting = "Hello";
                    break;

                case 1:
                    greeting = "Hey";
                    break;

                default: // Good morning
                    greeting = $"Good {GetDayTime(now)}";
                    break;
            }

            var welcomeMessages = new List<string>()
            {
                $"{greeting}, Activity Watch VS2022 at your service.",
                $"{greeting}, if you need help go here: {SUPPORT_THREAD_URL}",
                $"{greeting}, have fun coding!",
                $"{greeting}, enjoying the Extension? Tell us your story: {SUPPORT_THREAD_URL}",
                $"{greeting}, enjoying the Extension? Buy me a coffee: {COFFEE_URL}",
                $"{greeting}, did you know weh have a privacy mode? See 'Menu - Extensions - AW: disable tracking'. Tracking will be re-enabled on restart, or when you choose to enable it again.",
            };

            // extension has it's birthday (showing for a week). Counting from the first version for VS2019.
            if (now.Month == 3 && now.Day <= 23 && now.Day >= 23 - 7)
            {
                int age = (now.Year - 2019);
                string ageOrd = age.Ordinal();
                welcomeMessages.Add($"Happy birthday! We 're {age} years now. Support us: {COFFEE_URL}");
                welcomeMessages.Add($"It's the watcher's {ageOrd} birthday! Leave a message: {SUPPORT_THREAD_URL}");
                welcomeMessages.Add($"Party with us! It's the {ageOrd} watcher's BDay on 23.03.! Meet us here: {SUPPORT_THREAD_URL}");
            }

            int index = random.Next(welcomeMessages.Count);

            if (_outputWindow != null)
            {
                await _outputWindow.Writer.WriteLineAsync(welcomeMessages[index]);
            }
        }

        #endregion Methods
    }
}
