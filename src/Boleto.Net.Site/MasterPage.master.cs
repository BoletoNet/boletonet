using System;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkSource.NavigateUrl = "~/" + string.Format("SourceCode.aspx?url={0}", Request.Path);
        }
    }
}
