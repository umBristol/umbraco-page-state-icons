using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using umbraco.BusinessLogic;
using umbraco.cms.presentation.Trees;
using umbraco.NodeFactory;
using System.Xml;
using umbraco.cms.businesslogic.web;
using UmBristol.PageStateIcons.Config;
using umbraco.cms.helpers;

[assembly: PreApplicationStartMethod(typeof(UmBristol.PageStateIcons.Application), "RegisterModules")]

namespace UmBristol.PageStateIcons
{
	public class Application : ApplicationBase
	{
		private static bool modulesRegistered;

		public Application()
		{
			BaseTree.BeforeNodeRender += new BaseTree.BeforeNodeRenderEventHandler(this.BaseTree_BeforeNodeRender);
		}

		public void BaseTree_BeforeNodeRender(ref XmlTree sender, ref XmlTreeNode node, EventArgs e)
		{
			if (node.NodeType != "content") return;

			// try to get node from XML cache
			XmlNode xmlNode = umbraco.content.Instance.XmlContent.GetElementById(node.NodeID);

			if (xmlNode == null)
			{
				// if unpublished, get from Document (database)
				var doc = new Document(int.Parse(node.NodeID));
				if (doc != null)
				{
					// get the preview XML
					xmlNode = doc.ToPreviewXml(new XmlDocument());
				}
			}

			// if we have an XML node...
			if (xmlNode != null)
			{
				// ... match rules
				foreach (RuleElement rule in PageStateIconsConfigurationSection.Instance.Rules)
				{
					// apply XPath
					var xpath = string.Concat("self::*[", rule.XPath, "]");
					if (xmlNode.SelectSingleNode(xpath) != null)
					{
						// add custom class
						node.Style.AddCustom(String.Format("overlay-{0}", Casing.SafeAlias(rule.Name)));
					}
				}
			}
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