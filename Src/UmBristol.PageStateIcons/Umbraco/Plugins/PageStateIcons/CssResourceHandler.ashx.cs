using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace UmBristol.PageStateIcons
{
    class CssResourceHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/css";

            StringBuilder sb = new StringBuilder();

            int zIndex = 600;

            foreach (Config.RuleElement rule in Config.PageStateIconsConfigurationSection.Instance.Rules)
            {
                zIndex += 1;
                
                sb.AppendFormat(".overlay-{0} ", rule.Name);
                sb.Append("{");
                sb.AppendFormat("background: no-repeat url(\"{0}\") 0 0; ", rule.OverlayIconPath);
                sb.AppendFormat("z-index: {0}", zIndex);
                sb.Append("}");
                sb.AppendLine(string.Empty);
            }

            context.Response.Write(sb.ToString());
        }
    }
}
