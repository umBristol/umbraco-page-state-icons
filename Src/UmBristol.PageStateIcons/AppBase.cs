namespace UmBristol.PageStateIcons
{
    using System;
    using umbraco.BusinessLogic;
    using umbraco.cms.presentation.Trees;
    using umbraco.NodeFactory;

    public class AppBase :ApplicationBase
    {
        public AppBase()
        {
            BaseTree.BeforeNodeRender += BaseTree_BeforeNodeRender;
        }

        protected void BaseTree_BeforeNodeRender(ref XmlTree sender, ref XmlTreeNode node, EventArgs e)
        {
            if (node.NodeType != "content") return;

            var contentNode = new Node(Convert.ToInt32(node.NodeID));
            //TODO - logic for if node isn't published to use document instead

            foreach (Config.RuleElement rule in Config.PageStateIconsConfigurationSection.Instance.Rules)
            {
                // using r.name everywhere but this will change to relevant property
                if (contentNode.Properties[rule.Name] != null)
                    if (contentNode.Properties[rule.Name].ToString() == "1") // this is where the xpath magic will happen...
                        node.Style.AddCustom(String.Format("overlay-{0}", rule.Name)); // should this automatically add the overlay bit?
            }
        }
    }
}
