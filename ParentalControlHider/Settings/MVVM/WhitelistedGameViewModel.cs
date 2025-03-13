using System;
using System.Linq;
using System.Windows.Input;
using Playnite.SDK;
using Playnite.SDK.Models;

namespace ParentalControlHider.Settings.MVVM
{
	public class WhitelistedGameViewModel
	{
		private readonly ParentalControlHiderSettingsViewModel _viewModel;

		public WhitelistedGameViewModel(Game game, ParentalControlHiderSettingsViewModel viewModel, IPlayniteAPI playniteApi)
		{
			_viewModel = viewModel;
			Id = game.Id;
			Name = game.Name;
			Library = playniteApi.Database.Sources.FirstOrDefault(x => x.Id == game.SourceId)?.Name ?? "Playnite";
			ReleaseDate = game.ReleaseDate.ToString();
		}

		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Library { get; set; }

		public string ReleaseDate { get; set; }

		public ICommand Remove => new RelayCommand(() =>
		{
			_viewModel.Settings.WhitelistedGameIds.Remove(Id);
			_viewModel.InitializeGames();
		});
	}
}