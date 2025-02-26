﻿namespace ParentalControlHider.Settings
{
	public interface IPluginSettingsPersistence
	{
		T LoadPluginSettings<T>() where T : class;
		void SavePluginSettings<T>(T settings) where T : class;
	}
}