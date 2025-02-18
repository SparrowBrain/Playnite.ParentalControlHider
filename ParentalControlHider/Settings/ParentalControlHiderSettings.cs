using System.Collections.Generic;
using Playnite.SDK.Data;

namespace ParentalControlHider.Settings
{
    public class ParentalControlHiderSettings : ObservableObject
    {
        private string _option1 = string.Empty;
        private bool _option2 = false;
        private bool _optionThatWontBeSaved = false;

        public string Option1 { get => _option1; set => SetValue(ref _option1, value); }
        public bool Option2 { get => _option2; set => SetValue(ref _option2, value); }
        // Playnite serializes settings object to a JSON object and saves it as text file.
        // If you want to exclude some property from being saved then use `JsonDontSerialize` ignore attribute.
        [DontSerialize]
        public bool OptionThatWontBeSaved { get => _optionThatWontBeSaved; set => SetValue(ref _optionThatWontBeSaved, value); }
    }
}