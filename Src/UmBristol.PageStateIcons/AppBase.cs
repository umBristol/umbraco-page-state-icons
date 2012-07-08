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

            if (contentNode.Properties["umbracoNaviHide"] != null)
                if (contentNode.Properties["umbracoNaviHide"].ToString() == "1")
                    node.Style.AddCustom("overlay-hidden");
        }
    }
}
