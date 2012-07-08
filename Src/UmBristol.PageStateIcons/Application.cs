using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof(UmBristol.PageStateIcons.Application), "RegisterModules")]

namespace UmBristol.PageStateIcons
{
	public sealed class Application
	{
		private static bool modulesRegistered;

		private Application()
		{
		}

		public static void RegisterModules()
		{
			if (modulesRegistered)
				return;

			DynamicModuleUtility.RegisterModule(typeof(UmBristol.PageStateIcons.Modules.RegisterClientResources));

			modulesRegistered = true;
		}
	}
}