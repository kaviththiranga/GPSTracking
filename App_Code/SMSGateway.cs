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
    ReadMessages service ;

	public SMSGateway()
	{
        service = new ReadMessages();
	}

    public List<InboundMessage> getNewMessages() {

        List<InboundMessage> list= new List<InboundMessage>();

        InboundMessage[] msgList = service.DoIt();

        foreach (InboundMessage msg in msgList) {

            list.Add(msg);
        }
        return list;
    }

    public void sendMessage(string number, string msg) { 
    
    }
}