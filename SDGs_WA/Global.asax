<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Codice eseguito all'avvio dell'applicazione

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Codice eseguito all'arresto dell'applicazione

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Codice eseguito in caso di errore non gestito

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Codice eseguito all'avvio di una nuova sessione

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Codice eseguito al termine di una sessione. 
        // Nota: l'evento Session_End viene generato solo quando la modalità sessionstate
        // è impostata su InProc nel file Web.config. Se la modalità è impostata su StateServer 
        // o SQLServer, l'evento non viene generato.

    }

    protected void Application_AuthenticateRequest(Object sender,
EventArgs e)
{
  if (HttpContext.Current.User != null)
  {
    if (HttpContext.Current.User.Identity.IsAuthenticated)
    {
     if (HttpContext.Current.User.Identity is FormsIdentity)
     {
        FormsIdentity id =
            (FormsIdentity)HttpContext.Current.User.Identity;
        FormsAuthenticationTicket ticket = id.Ticket;

        // Get the stored user-data, in this case, our roles
        string userData = ticket.UserData;
        string[] roles = userData.Split(',');
        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);
     }
    }
  }
}
       
</script>
