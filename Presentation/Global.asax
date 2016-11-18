<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        SiteMap.SiteMapResolve += new SiteMapResolveEventHandler(this.CustomizeNodes);
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
    private SiteMapNode CustomizeNodes(Object sender, SiteMapResolveEventArgs e)
    {
        int ServiceID = 0;

        bool IsServiceIDGood = int.TryParse(System.Web.HttpContext.Current.Request.QueryString["MenuId"], out ServiceID);
           if(ServiceID == 15 || ServiceID == 16 ||ServiceID == 22)
        {
        SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
        SiteMapNode tempNode = currentNode;

        do
        {

            if (tempNode["query"] != null)
            {
                if (tempNode["query"].Contains("MenuId"))
                    tempNode.Url = tempNode.Url + "?MenuId=" + ServiceID;
                if (ServiceID == 15) tempNode.Title = "Student Debit Note";
                if (ServiceID == 16) tempNode.Title = "Student Credit Note";
                if (ServiceID == 22) tempNode.Title = "Sponsor Debit Note";
            }

            if (tempNode.ParentNode.Equals(tempNode.RootNode))
                tempNode.ParentNode = null;

            tempNode = tempNode.ParentNode;
        } while (tempNode != null);

        return currentNode;
        }
        return SiteMap.CurrentNode;
    }
       
</script>
