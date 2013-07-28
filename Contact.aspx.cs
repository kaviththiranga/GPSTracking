using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using org.smslib;
public partial class Contact : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SMSGateway gateway = new SMSGateway();

        Label1.Text = "Message List\n";

        foreach (InboundMessage msg in gateway.getNewMessages()) {

            Label1.Text += "\n Messsage From :" + msg.getOriginator() + "\nContent : " + msg.getText();
        }

    }
}