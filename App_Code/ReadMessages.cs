﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using org.smslib;
using org.smslib.modem;
using Comm2IP;

class ReadMessages
{
    public class CallNotification : ICallNotification
    {
        public void process(AGateway gateway, String callerId)
        {
            Console.WriteLine(">>> New call detected from Gateway: " + gateway.getGatewayId() + " : " + callerId);
        }
    }

    public class InboundNotification : IInboundMessageNotification
    {
        public void process(AGateway gateway, org.smslib.Message.MessageTypes msgType, InboundMessage msg)
        {
            if (msgType == org.smslib.Message.MessageTypes.INBOUND) Console.WriteLine(">>> New Inbound message detected from Gateway: " + gateway.getGatewayId());
            else if (msgType == org.smslib.Message.MessageTypes.STATUSREPORT) Console.WriteLine(">>> New Inbound Status Report message detected from Gateway: " + gateway.getGatewayId());
            Console.WriteLine(msg);
            try
            {
                // Uncomment following line if you wish to delete the message upon arrival.
                //gateway.deleteMessage(msg);

            }
            catch (Exception e)
            {
                Console.WriteLine("Oops!!! Something gone bad...");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }

    public class GatewayStatusNotification : IGatewayStatusNotification
    {
        public void process(AGateway gateway, org.smslib.AGateway.GatewayStatuses oldStatus, org.smslib.AGateway.GatewayStatuses newStatus)
        {
            Console.WriteLine(">>> Gateway Status change for " + gateway.getGatewayId() + ", OLD: " + oldStatus + " -> NEW: " + newStatus);
        }
    }

    public InboundMessage[] DoIt()
    {
        // Create new Service object - the parent of all and the main interface to you.
        Service srv;
        srv = Service.getInstance();
        InboundMessage[] list = new InboundMessage[100];

        // Modem Settings
        String comPort = "com6";
        String modemName = "huwawei";
        int port = 12000;
        String manufacturer = "huwawei";
        int portSpeed = 9600;


        // *** The tricky part ***
        // *** Comm2IP Driver ***
        // Create (and start!) as many Comm2IP threads as the modems you are using.
        // Be careful about the mappings - use the same mapping in the Gateway definition.
        Comm2IP.Comm2IP com1 = new Comm2IP.Comm2IP(new byte[] { 127, 0, 0, 1 }, port, comPort, portSpeed);

        try
        {
            Console.WriteLine("Example: Read messages from a serial gsm modem.");
            Console.WriteLine(Library.getLibraryDescription());
            Console.WriteLine("Version: " + Library.getLibraryVersion() + " test");

            // Start the COM listening thread.
            new Thread(new ThreadStart(com1.Run)).Start();

            // Lets set some callbacks.
            srv.setInboundMessageNotification(new InboundNotification());
            srv.setCallNotification(new CallNotification());
            srv.setGatewayStatusNotification(new GatewayStatusNotification());

            // Create the Gateway representing the serial GSM modem.
            // Due to the Comm2IP bridge, in SMSLib for .NET all modems are considered IP modems.
            IPModemGateway gateway = new IPModemGateway("modem.com4", "127.0.0.1", port, manufacturer, modemName);
            gateway.setIpProtocol(ModemGateway.IPProtocols.BINARY);

            // Set the modem protocol to PDU (alternative is TEXT). PDU is the default, anyway...
            gateway.setProtocol(AGateway.Protocols.PDU);

            // Do we want the Gateway to be used for Inbound messages?
            gateway.setInbound(true);

            // Do we want the Gateway to be used for Outbound messages?
            gateway.setOutbound(true);

            // Let SMSLib know which is the SIM PIN.
            gateway.setSimPin("0000");

            // Add the Gateway to the Service object.
            srv.addGateway(gateway);

            // Similarly, you may define as many Gateway objects, representing
            // various GSM modems, add them in the Service object and control all of them.

            // Start! (i.e. connect to all defined Gateways)
            srv.startService();

            // Printout some general information about the modem.
            Console.WriteLine();
            Console.WriteLine("Modem Information:");
            Console.WriteLine("  Manufacturer: " + gateway.getManufacturer());
            Console.WriteLine("  Model: " + gateway.getModel());
            Console.WriteLine("  Serial No: " + gateway.getSerialNo());
            Console.WriteLine("  SIM IMSI: " + gateway.getImsi());
            Console.WriteLine("  Signal Level: " + gateway.getSignalLevel() + "dBm");
            Console.WriteLine("  Battery Level: " + gateway.getBatteryLevel() + "%");
            Console.WriteLine();

            // Read Messages. The reading is done via the Service object and
            // affects all Gateway objects defined. This can also be more directed to a specific
            // Gateway - look the JavaDocs for information on the Service method calls.
            list = srv.readMessages(org.smslib.InboundMessage.MessageClasses.ALL);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
        finally
        {

            com1.Stop();
            srv.stopService();
        }

        return list;
    }
}


