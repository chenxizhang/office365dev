using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Microsoft.Teams.Samples.HelloWorld.Web
{
    public class EchoBot
    {
        public static async Task EchoMessage(ConnectorClient connector, Activity activity)
        {
            //发送普通消息
            //var reply = activity.CreateReply("You said: " + activity.GetTextWithoutMentions());
            //await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);



            //发送卡片消息
            try
            {
                var keyword = activity.GetTextWithoutMentions();
                var result = MessageExtension.GetAttachment(keyword).FirstOrDefault();

                //发送卡片消息
                Activity reply = activity.CreateReply();

                if (result != null)
                {
                    reply.Attachments.Add(result);//用缩略图
                }
                else
                {
                    HeroCard card = new HeroCard 
                    {
                        Title = "对不起",
                        Text = "我很抱歉，没有找到你想要的内容，请尝试输入一些跟Office 365开发相关的关键字吧",
                        Images = new List<CardImage>(),
                    };
                    card.Images.Add(new CardImage { Url = "http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString() });
                    reply.Attachments.Add(card.ToAttachment());
                }


                await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);
            }
            catch(Exception ex)
            {
                var reply = activity.CreateReply("啊哦，出错了:" + ex.Message);
                await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);
            }

        }
    }
}
