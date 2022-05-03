using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.IoC;
using System;
using System.IO;
using System.Reflection;

namespace AutoGGs
{
    public class AutoGGs : IDalamudPlugin
    {
        public string Name => "GGNoRe";

        private const string CommandName = "/GGNoRe";

        private Configuration Configuration { get; init; }
        private AutoGGsUI UI { get; init; }

        public AutoGGs([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            DalamudContainer.Initialize(pluginInterface);

            this.Configuration = DalamudContainer.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(DalamudContainer.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"goat.png");
            var goatImage = DalamudContainer.PluginInterface.UiBuilder.LoadImage(imagePath);
            this.UI = new AutoGGsUI(this.Configuration, goatImage);

            DalamudContainer.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display i2n /xlhelp."
            });

            DalamudContainer.PluginInterface.UiBuilder.Draw += DrawUI;
            DalamudContainer.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.UI.Dispose();

            DalamudContainer.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            this.UI.Visible = true;
        }

        private void DrawUI()
        {
            this.UI.Draw();
        }

        private void DrawConfigUI()
        {
            this.UI.SettingsVisible = true;
        }
    }
}
