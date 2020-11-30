/**
* Sample code for the GetService Canada Post service.
* 
* The GetService service  returns details of a given postal service in 
* terms of the min/max weight and dimensions offered by the postal service. 
* Also returned are details about the available add-on options. 
*
* This sample is configured to access the Developer Program sandbox environment. 
* Use your development key username and password for the web service credentials.
* 
**/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Security.Authentication;

namespace GetService
{
    class GetService
    {
        //TLS 1.2 support for .NET 3.5
        public const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        public const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = Tls12;

            // Your username and password are imported from the following file
            // CPCWS_Rating_DotNet_Samples\REST\rating\user.xml 
            var username = ConfigurationSettings.AppSettings["username"];
            var password = ConfigurationSettings.AppSettings["password"];

            var url = "https://ct.soa-gw.canadapost.ca/rs/ship/service/DOM.EP?country=CA"; // REST URL

            var method = "GET"; // HTTP Method
            String responseAsString = ".NET Framework " + Environment.Version.ToString() + "\r\n\r\n";

            try
            {

                // Create REST Request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                

                // Set Basic Authentication Header using username and password variables
                string auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));
                request.Headers = new WebHeaderCollection();
                request.Headers.Add("Authorization", auth);
                request.Headers.Add("Accept-Language", "en-CA");
                request.Accept = "application/vnd.cpc.ship.rate-v4+xml";


                // Execute REST Request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Deserialize response to service object
                XmlSerializer serializer = new XmlSerializer(typeof(service));
                TextReader reader = new StreamReader(response.GetResponseStream());
                service service = (service)serializer.Deserialize(reader);

                // Retrieve values from service object
                responseAsString += "Service Name: " + service.servicename + "\r\n";
                responseAsString += "Service Code: " + service.servicecode + "\r\n\r\n";


                foreach (var option in service.options)
                {
                    responseAsString += "Option Name: " + option.optionname + "\r\n";
                    responseAsString += "Option Code: " + option.optioncode + "\r\n";
                    responseAsString += "Href: " + option.link.href + "\r\n\r\n";
                }

            }
            catch (WebException webEx)
            {
                HttpWebResponse response = (HttpWebResponse)webEx.Response;

                if (response != null)
                {
                    responseAsString += "HTTP  Response Status: " + webEx.Message + "\r\n";

                    // Retrieve errors from messages object
                    try
                    {
                        // Deserialize xml response to messages object
                        XmlSerializer serializer = new XmlSerializer(typeof(messages));
                        TextReader reader = new StreamReader(response.GetResponseStream());
                        messages myMessages = (messages)serializer.Deserialize(reader);


                        if (myMessages.message != null)
                        {
                            foreach (var item in myMessages.message)
                            {
                                responseAsString += "Error Code: " + item.code + "\r\n";
                                responseAsString += "Error Msg: " + item.description + "\r\n";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Misc Exception
                        responseAsString += "ERROR: " + ex.Message;
                    }
                }
                else
                {
                    // Invalid Request
                    responseAsString += "ERROR: " + webEx.Message;
                }

            }
            catch (Exception ex)
            {
                // Misc Exception
                responseAsString += "ERROR: " + ex.Message;
            }

            Console.WriteLine(responseAsString);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
