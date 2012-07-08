using System;
using System.Web;
using umbraco;
using UmBristol.PageStateIcons.Filters;

namespace UmBristol.PageStateIcons.Modules
{
	public class RegisterClientResources : IHttpModule
	{
		public void Dispose()
		{
		}

		public void Init(HttpApplication context)
		{
			context.PostMapRequestHandler += new EventHandler(this.context_PostMapRequestHandler);
		}

		private void context_PostMapRequestHandler(object sender, EventArgs e)
		{
			var context = sender as HttpApplication;
			var executionPath = context.Request.CurrentExecutionFilePath;
			var umbracoPath = string.Concat(GlobalSettings.Path, "/");
			var defaultAspx = string.Concat(umbracoPath, "default.aspx");
			var umbracoAspx = string.Concat(umbracoPath, "umbraco.aspx");

			if (executionPath.StartsWith(umbracoPath, StringComparison.CurrentCultureIgnoreCase))
			{
				if (executionPath.Equals(defaultAspx, StringComparison.InvariantCultureIgnoreCase))
				{
					// /umbraco/default.aspx forwards to umbraco.aspx with Server.Transfer, which doesn't work because it doesn't have the right page, i.e. umbraco.aspx
					context.Response.Redirect("umbraco.aspx");
					return;
				}
				else if (executionPath.Equals(umbracoAspx, StringComparison.InvariantCultureIgnoreCase))
				{
					context.Response.Filter = new InjectResources(context.Response.Filter);
				}
			}
		}
	}
}