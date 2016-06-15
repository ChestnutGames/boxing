using System.Collections;
using System.Collections.Generic;

namespace Framework
{
	public sealed class I18NManager : UnitySingleton<I18NManager> 
	{
		protected I18NManager() 
		{
		}
		
		public string GetString(string id)	
		{
			if(id == null)
			{
				return null;
			}

			return I18NStrings.Get(id);
		}
	}
}