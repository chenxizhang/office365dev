using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;

namespace Microsoft.Teams.Samples.HelloWorld.Web.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            using (var connector = new ConnectorClient(new Uri(activity.ServiceUrl)))
            {
                if (activity.IsComposeExtensionQuery())//针对Messaging extension的响应
                {
                    var response = MessageExtension.HandleMessageExtensionQuery(connector, activity);
                    return response != null
                        ? Request.CreateResponse<ComposeExtensionResponse>(response)
                        : new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    if(activity.Type == "message")
                    {
                        var text = activity.GetTextWithoutMentions();
                        if (text == "抽奖")
                        {
                            await Conversation.SendAsync(activity, () => new ProactiveMsgTo1to1Dialog());
                        }
                        else
                        {
                            await EchoBot.EchoMessage(connector, activity);
                        }
                    }

                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }
            }
        }
    }


    /// <summary>
    /// This is Proactive Message Dialog Class. Main purpose of this class is to show the Send Proactive Message Example
    /// </summary>
    [Serializable]
    public class ProactiveMsgTo1to1Dialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //Set the Last Dialog in Conversation Data
            context.UserData.SetValue("抽奖", "抽奖数据");

            var userId = context.Activity.From.Id;
            var botId = context.Activity.Recipient.Id;
            var botName = context.Activity.Recipient.Name;

            var channelData = context.Activity.GetChannelData<TeamsChannelData>();
            var connectorClient = new ConnectorClient(new Uri(context.Activity.ServiceUrl));

            var parameters = new ConversationParameters
            {
                Bot = new ChannelAccount(botId, botName),
                Members = new ChannelAccount[] { new ChannelAccount(userId) },
                ChannelData = new TeamsChannelData
                {
                    Tenant = channelData.Tenant
                }
            };

            var conversationResource = await connectorClient.Conversations.CreateConversationAsync(parameters);

            var message = Activity.CreateMessageActivity();
            message.From = new ChannelAccount(botId, botName);
            message.Conversation = new ConversationAccount(id: conversationResource.Id.ToString());
            message.Text ="抽奖消息";

            await connectorClient.Conversations.SendToConversationAsync((Activity)message);

            context.Done<object>(null);
        }
    }
}
