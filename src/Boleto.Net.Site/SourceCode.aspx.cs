using System;
using System.IO;

public partial class SourceCode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string url = Request.QueryString["url"];

            if (!string.IsNullOrEmpty(url))
            {
                PrintSource(url);
            }
        }
    }

    private void PrintSource(string url)
    {
        string file = Server.MapPath(url);

        if (!string.IsNullOrEmpty(file))
        {
            string output = string.Empty;

            using (StreamReader sr = new StreamReader(file))
            {
                output = sr.ReadToEnd();
            }

            code.InnerText = output;
            Header.Title = string.Format("Code : {0}", url);
        }
    }
}
