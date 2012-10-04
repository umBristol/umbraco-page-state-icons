using System;
using System.Web;
using System.Xml;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.web;
using umbraco.cms.helpers;
using umbraco.cms.presentation.Trees;
using UmBristol.PageStateIcons.Config;
using umbraco.cms.businesslogic.member;
using umbraco.cms.businesslogic.media;
using umbraco;

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

		public static void RegisterModules()
		{
			if (modulesRegistered)
				return;

			DynamicModuleUtility.RegisterModule(typeof(UmBristol.PageStateIcons.Modules.RegisterClientResources));

			modulesRegistered = true;
		}

		protected void BaseTree_BeforeNodeRender(ref XmlTree sender, ref XmlTreeNode node, EventArgs e)
		{
			XmlNode xmlNode = null;

			switch (node.NodeType.ToUpper())
			{
				case "content":
					xmlNode = this.GetContentXmlNode(node.NodeID);
					break;

				case "media":
					xmlNode = this.GetMediaXmlNode(node.NodeID);
					break;

				case "member":
					xmlNode = this.GetMemberXmlNode(node.NodeID);
					break;

				default:
					break;
			}

			// if we have an XML node...
			if (xmlNode != null && PageStateIconsConfigurationSection.Instance != null)
			{
				// ... match rules
				foreach (RuleElement rule in PageStateIconsConfigurationSection.Instance.Rules)
				{
					// apply XPath
					var xpath = string.Concat("self::*[", rule.XPath, "]");
					if (xmlNode.SelectSingleNode(xpath) != null)
					{
						switch (rule.IconType)
						{
							case IconType.overlay:
								// add custom class for overlay
								node.Style.AddCustom(string.Format("overlay-{0}", Casing.SafeAlias(rule.Name)));
								break;

							case IconType.icon:
								node.Icon = rule.IconPath;
								break;

							default:
								break;
						}
					}
				}
			}
		}

		private XmlNode GetContentXmlNode(string nodeId)
		{
			// try to get node from XML cache
			XmlNode xmlNode = content.Instance.XmlContent.GetElementById(nodeId);

			if (xmlNode == null)
			{
				// if unpublished, get from Document (database)
				var doc = new Document(int.Parse(nodeId));
				if (doc != null)
				{
					// get the preview XML
					xmlNode = doc.ToPreviewXml(new XmlDocument());
				}
			}

			return xmlNode;
		}

		private XmlNode GetMediaXmlNode(string nodeId)
		{
			int id;
			if (int.TryParse(nodeId, out id))
			{
				var media = new Media(id);
				if (media != null)
				{
					return media.ToXml(new XmlDocument(), false);
				}
			}

			return null;
		}

		private XmlNode GetMemberXmlNode(string nodeId)
		{
			int id;
			if (int.TryParse(nodeId, out id))
			{
				var member = new Member(id);
				if (member != null)
				{
					return member.ToXml(new XmlDocument(), false);
				}
			}

			return null;
		}
	}
}