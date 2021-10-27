using At.Lagg.ActivityWatchVS2022.Tools;
using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Documents;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022.Services
{
    public class ConsoleService : ExtensionPart
    {
        //TODO: use new URL once the new Support thread is here.
        private const string SUPPORT_THREAD_URL = @"https://tinyurl.com/yzg8aq4o";
        private const string COFFEE_URL = @"https://buymeacoffee.com/LaggAt";

        private OutputWindow _outputWindow;

        public ConsoleService(ExtensionCore container, VisualStudioExtensibility extensibility)
            : base(container, extensibility)
        {
            var initTask = Task.Run(this.InitializeAsync);
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            initTask.Wait();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
        }

        protected async Task InitializeAsync()
        {
            _outputWindow =
                await this.Extensibility.Views().Output.GetChannelAsync(
                "Activity Watch",
                $"{nameof(ActivityWatchVS2022)}-{Guid.NewGuid()}",
                default
            );
            Requires.NotNull(_outputWindow, nameof(_outputWindow));
            sayHello();
        }

        /// <summary>
        /// Say hi. This method is probably too long.
        /// Well, a lot of effort to just say hi, but when do we really talk to our users?
        /// So let's enjoy this.
        /// </summary>
        private void sayHello()
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
                    int hour = now.Hour;
                    string dayTime = "day";
                    if (hour >= 22) { dayTime = "night"; }
                    else if (hour >= 17) { dayTime = "evening"; }
                    else if (hour >= 12) { dayTime = "afternoon"; }
                    else if (hour >= 6) { dayTime = "morning"; }
                    else { dayTime = "night"; }
                    greeting = $"Good {dayTime}";
                    break;
            }

            var welcomeMessages = new List<string>()
            {
                $"{greeting}, Activity Watch VS2022 at your service.",
                $"{greeting}, if you need help go here: {SUPPORT_THREAD_URL}",
                $"{greeting}, have fun coding!",
                $"{greeting}, enjoying the Extension? Tell us your story: {SUPPORT_THREAD_URL}",
                $"{greeting}, enjoying the Extension? Buy me a coffee: {COFFEE_URL}",
                $"{greeting}, did you know you can temporarly disable me? See Extensions - AW: disable tracking.",
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
            _outputWindow.Writer.WriteLine(welcomeMessages[index]);
        }
    }
}
