using System;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using System.Xml;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using umbraco.IO;
using UmBristol.PageStateIcons.Config;

namespace UmBristol.PageStateIcons.PackageActions
{
	public class AddConfigSection : IPackageAction
	{
		private static readonly string sectionName = "pageStateIconsConfigurationSection";

		public string Alias()
		{
			return "PageStateIcons_AddConfigSection";
		}

		public bool Execute(string packageName, XmlNode xmlData)
		{
			try
			{
				var webConfig = WebConfigurationManager.OpenWebConfiguration("~/");
				if (webConfig.Sections[sectionName] == null)
				{
					webConfig.Sections.Add(sectionName, new PageStateIconsConfigurationSection());

					string configPath = string.Concat("config", Path.DirectorySeparatorChar, "UmBristol.PageStateIcons.config");
					string xmlPath = IOHelper.MapPath(string.Concat("~/", configPath));
					string xml;

					using (var reader = new StreamReader(xmlPath))
					{
						xml = reader.ReadToEnd();
					}

					webConfig.Sections[sectionName].SectionInformation.SetRawXml(xml);
					webConfig.Sections[sectionName].SectionInformation.ConfigSource = configPath;

					webConfig.Save(ConfigurationSaveMode.Modified);
				}

				return true;
			}
			catch (Exception ex)
			{
				string message = string.Concat("Error at install ", this.Alias(), " package action: ", ex);
				Log.Add(LogTypes.Error, -1, message);
			}

			return false;
		}

		public XmlNode SampleXml()
		{
			string xml = string.Concat("<Action runat=\"install\" undo=\"true\" alias=\"", this.Alias(), "\" />");
			return helper.parseStringToXmlNode(xml);
		}

		public bool Undo(string packageName, XmlNode xmlData)
		{
			try
			{
				var webConfig = WebConfigurationManager.OpenWebConfiguration("~/");
				if (webConfig.Sections[sectionName] != null)
				{
					webConfig.Sections.Remove(sectionName);

					webConfig.Save(ConfigurationSaveMode.Modified);
				}

				return true;
			}
			catch (Exception ex)
			{
				string message = string.Concat("Error at undo ", this.Alias(), " package action: ", ex);
				Log.Add(LogTypes.Error, -1, message);
			}

			return false;
		}
	}
}