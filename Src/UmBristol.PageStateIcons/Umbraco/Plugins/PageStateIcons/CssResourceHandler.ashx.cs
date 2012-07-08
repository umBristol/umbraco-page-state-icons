using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using umbraco.cms.helpers;

namespace UmBristol.PageStateIcons
{
    /// <summary>
    ///   Dynamically generates the CSS to be used for applying icon overlays to the Umbraco page tree.
    /// </summary>
    public class CssResourceHandler : IHttpHandler
    {
        /// <summary>
        ///   Gets the starting z index for the custom overlays.
        /// </summary>
        private static readonly int ZIndexStartingIndex = 600;

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            int zindex = ZIndexStartingIndex;

            StringBuilder sb = new StringBuilder();
            
            // Enumerate the rules to generate the CSS which is required for the overlap images.
            foreach (Config.RuleElement rule in Config.PageStateIconsConfigurationSection.Instance.Rules)
            {
                zindex += 1;

				sb.AppendFormat("div.overlay-{0} ", Casing.SafeAlias(rule.Name));
                sb.Append("{");
                sb.AppendFormat("left: {0}px; ", rule.Left);
                sb.AppendFormat("top: {0}px; ", rule.Top);
                sb.AppendFormat("background: no-repeat url(\"{0}\") 0 0; ", rule.OverlayIconPath);
                sb.AppendFormat("z-index: {0}", zindex);

				sb.AppendFormat("_filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}'); ", rule.OverlayIconPath);
				sb.Append("}");
                sb.AppendLine(string.Empty);
            }

            context.Response.ContentType = "text/css";

            context.Response.Write(sb.ToString());

            context.Response.End();
        }
    }
}
