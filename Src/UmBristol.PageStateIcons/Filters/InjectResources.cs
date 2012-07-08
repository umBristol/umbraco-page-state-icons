using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using umbraco;

namespace UmBristol.PageStateIcons.Filters
{
	public sealed class InjectResources : MemoryStream
	{
		private const string HtmlBodyClosing = "</body>";

		private const string HtmlHeadClosing = "</head>";

		private Stream OutputStream = null;

		private StringBuilder HtmlScripts;

		private StringBuilder HtmlStyles;

		public InjectResources(Stream output)
		{
			this.OutputStream = output;

			this.HtmlScripts = new StringBuilder();
			this.HtmlStyles = new StringBuilder();

			// CSS resources
			this.HtmlStyles.AppendFormat("<link type='text/css' rel='stylesheet' href='{0}/{1}' />", GlobalSettings.Path, "plugins/PageStateIcons/CssResourceHandler.ashx").AppendLine();

			// JavaScript resources
			////this.HtmlScripts.AppendFormat("<script type='text/javascript' src='{0}/{1}'></script>", GlobalSettings.Path, "plugins/PageStateIcons/JsResourceHandler.ashx").AppendLine();

			this.HtmlScripts.AppendLine(HtmlBodyClosing);
			this.HtmlStyles.AppendLine(HtmlHeadClosing);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			// get the string from the buffer.
			string content = UTF8Encoding.UTF8.GetString(buffer);

			if (content.Contains(HtmlHeadClosing))
			{
				// append the <link> tags to the closing </head> tag.
				content = content.Replace(HtmlHeadClosing, this.HtmlStyles.ToString());
			}

			if (content.Contains(HtmlBodyClosing))
			{
				// append the <script> tags to the closing </body> tag.
				content = content.Replace(HtmlBodyClosing, this.HtmlScripts.ToString());
			}

			// write the content changes back to the buffer.
			byte[] outputBuffer = UTF8Encoding.UTF8.GetBytes(content);
			this.OutputStream.Write(outputBuffer, 0, outputBuffer.Length);
		}
	}
}