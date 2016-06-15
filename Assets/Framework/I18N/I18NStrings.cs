#define ZH_CN
//#define EN_US

using System.Collections;
using System.Collections.Generic;

namespace Framework
{
	public static class I18NStrings
	{
		private static readonly Dictionary<string, string> _strings = new Dictionary<string, string>() {

			{"", ""},
			{"", ""},
			{"", ""},
			{"", ""},
			{"", ""}
		};
	 
		public static string Get(string id)
		{
			return id != null? _strings[id] : null;
		}
	}
}
