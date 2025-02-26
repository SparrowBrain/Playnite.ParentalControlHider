namespace ParentalControlHider.Settings
{
	internal class PluginSettingsPersistence : IPluginSettingsPersistence
	{
		private readonly ParentalControlHider _plugin;

		public PluginSettingsPersistence(ParentalControlHider plugin)
		{
			_plugin = plugin;
		}

		public T LoadPluginSettings<T>() where T : class
		{
			return _plugin.LoadPluginSettings<T>();
		}

		public void SavePluginSettings<T>(T settings) where T : class
		{
			_plugin.SavePluginSettings(settings);
		}
	}
}