using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;

namespace Microsoft.Teams.Samples.HelloWorld.Web
{
    public class MessageExtension
    {
        public static ComposeExtensionResponse HandleMessageExtensionQuery(ConnectorClient connector, Activity activity)
        {
            ComposeExtensionResponse invokeResponse = new ComposeExtensionResponse();
            // This helper method gets the query as an object.
            var query = activity.GetComposeExtensionQueryData();

            var results = new ComposeExtensionResult()
            {
                AttachmentLayout = "list",
                Type = "result",
                Attachments = new List<ComposeExtensionAttachment>()
            };


            if (query.CommandId != null && query.Parameters != null && query.Parameters.Count > 0)
            {
                //用户没有设置参数，而是初始化
                if (query.Parameters[0].Name.Equals("initialRun"))
                {
                    results.Attachments = GetAttachment();
                }
                else
                {
                    //根据传入的参数来返回结果的个数
                    string keyword = query.Parameters[0].Value.ToString();
                    results.Attachments = GetAttachment(keyword);

                }


            }


            invokeResponse.ComposeExtension = results;

            return invokeResponse;
        }

        internal static List<ComposeExtensionAttachment> GetAttachment(string keyword="")
        {
            XNamespace ns = "http://www.w3.org/2005/Atom";
            var document = XDocument.Load("http://feed.cnblogs.com/blog/u/12626/rss");
            var result = document.Root.Elements(ns+"entry").Where(x =>
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return true;
                }
                else
                {
                    return x.Element(ns+"title").Value.ToLower().Contains(keyword.ToLower());
                }
            }).Select(x =>
            {
                var item = new ComposeExtensionAttachment()
                {
                    Content = new ThumbnailCard(
                        title: x.Element(ns+"title").Value,
                        text: x.Element(ns+"summary").Value,
                        images: new[] { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) },
                        tap: new CardAction()
                        {
                            Type = "OpenUrl",
                            Value = x.Element(ns+"id").Value
                        }
                    ),
                    ContentUrl = x.Element(ns+"id").Value,
                    ContentType = "application/vnd.microsoft.card.thumbnail",
                    Name = "result",
                    Preview = new ThumbnailCard(title: x.Element(ns + "title").Value, text: x.Element(ns + "summary").Value, images: new[] { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }).ToAttachment()
                };
                return item;
            }).ToList();



            return result;
        }
    }
}
