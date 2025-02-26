using Playnite.SDK;
using Playnite.SDK.Models;
using System;
using System.Linq;

namespace ParentalControlHider.Services
{
	public class ParentalHiderTagProvider : IParentalHiderTagProvider
	{
		private const string TagName = "[ParentalControlHider] Hidden";
		private readonly IPlayniteAPI _api;

		public ParentalHiderTagProvider(IPlayniteAPI api)
		{
			_api = api;
		}

		public Tag GetParentalHiderTag()
		{
			var tag = _api.Database.Tags.FirstOrDefault(x => x.Name == TagName);
			if (tag == null)
			{
				tag = new Tag() { Id = Guid.NewGuid(), Name = TagName };
				_api.Database.Tags.Add(tag);
			}

			return tag;
		}
	}
}