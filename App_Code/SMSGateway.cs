using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using org.smslib;
using org.smslib.modem;
using Comm2IP;
/// <summary>
/// Summary description for SMSGateway
/// </summary>
public class SMSGateway
{
	public SMSGateway()
	{

	}

    public static List<InboundMessage> getNewMessages() {

        return new List<InboundMessage>();
    }

    public static void sendMessage(string number, string msg) { 
    
    }
}