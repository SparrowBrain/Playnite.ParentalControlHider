using System;
using System.Collections.Generic;

namespace ParentalControlHider.Settings.MVVM
{
	public class AgeRatingsViewModel : ObservableObject
	{
		private readonly ParentalControlHiderSettings _settings;

		private Guid _id;
		private bool _isUsed;
		private string _name;
		private int _age;

		public AgeRatingsViewModel(Guid id, bool isUsed, string name, int age, ParentalControlHiderSettings settings)
		{
			_settings = settings;
			Id = id;
			IsUsed = isUsed;
			Name = name;
			Age = age;
		}

		public Guid Id
		{
			get => _id;
			set => SetValue(ref _id, value);
		}

		public bool IsUsed
		{
			get => _isUsed;
			set
			{
				SetValue(ref _isUsed, value);
				UpdateIsUsedInSettings(value);
			}
		}

		public string Name
		{
			get => _name;
			set => SetValue(ref _name, value);
		}

		public int Age
		{
			get => _age;
			set
			{
				SetValue(ref _age, value);
				UpdateAgeInSettings(value);
			}
		}

		private void UpdateIsUsedInSettings(bool isUsed)
		{
			if (isUsed)
			{
				_settings.UsedAgeRatings.Add(Id);
			}
			else if (_settings.UsedAgeRatings.Contains(Id))
			{
				_settings.UsedAgeRatings.Remove(Id);
			}
		}

		private void UpdateAgeInSettings(int age)
		{
			_settings.AgeRatingsWithAge[Id] = age;
		}
	}
}