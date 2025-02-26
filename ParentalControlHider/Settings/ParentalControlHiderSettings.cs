using System;
using System.Collections.Generic;

namespace ParentalControlHider.Settings
{
    public class ParentalControlHiderSettings : ObservableObject
    {
        public List<Guid> BlacklistedTagIds { get; set; } = new List<Guid>();
	}
}