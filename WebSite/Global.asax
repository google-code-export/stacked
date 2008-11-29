<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        // Initializing Castle
        Castle.ActiveRecord.ActiveRecordStarter.Initialize(
            Castle.ActiveRecord.Framework.Config.ActiveRecordSectionHandler.Instance,
            new Type[] { 
                typeof(Entities.Operator),
                typeof(Entities.Favorite),
                typeof(Entities.Vote),
                typeof(Entities.QuizItem)
            });

        try
        {
            // This one will throw an exception if the schema is incorrect or not created...
            int dummyToCheckIfDataBaseHasSchema = Entities.Operator.GetCount();
        }
        catch
        {
            // Letting ActiveRecord create our schema since the schema was obviously NOT correct
            // or created (warning; this logic might corrupt your database if you do changes to 
            // the schema)
            Castle.ActiveRecord.ActiveRecordStarter.CreateSchema();

            // Creating default operator
            Entities.Operator oper = new Entities.Operator();
            oper.Username = "admin";
            oper.Password = "admin";
            oper.FriendlyName = "default";
            oper.IsAdmin = true;
            oper.Create();
        }
    }
    
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Application_EndRequest(object sender, EventArgs e)
    {
        if (Request.Path.ToLowerInvariant().EndsWith("webresource.axd") &&
            HttpContext.Current.Response.ContentType.ToLowerInvariant() == "text/javascript")
        {
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddYears(3));
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);

            HttpContext.Current.Response.AppendHeader("Content-Encoding", "gzip");
        }
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
       
</script>
