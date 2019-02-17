# 课程笔记 —— Microsoft Teams 应用开发实战

> 陈希章 于 2019-2-10

1. 官方案例链接 <https://docs.microsoft.com/en-us/microsoftteams/platform/get-started/get-started-dotnet-app-studio>
1. Bot Framework <https://dev.botframework.com/>
1. Bot Service <https://azure.microsoft.com/en-us/services/bot-service/>
1. Bot Framework Emulator <https://github.com/Microsoft/BotFramework-Emulator>
1. App Studio <https://docs.microsoft.com/zh-cn/microsoftteams/platform/get-started/get-started-app-studio>
1. Bot Overview <https://docs.microsoft.com/zh-cn/microsoftteams/platform/concepts/bots/bots-overview>
1. Tab Overview <https://docs.microsoft.com/zh-cn/microsoftteams/platform/concepts/tabs/tabs-overview>
1. Messaging Extension <https://docs.microsoft.com/zh-cn/microsoftteams/platform/concepts/messaging-extensions/messaging-extensions-overview>
1. Incoming Webhook中发送普通消息

    ``` json
    {
        "text":"这是普通消息"
    }
    ```

1. Incoming Webhook中发送卡片消息

    ``` json
    {
        "@type": "MessageCard",
        "@context": "https://schema.org/extensions",
        "summary": "开发入门指南培训申请",
        "themeColor": "0075FF",
        "sections": [
            {
                "heroImage": {
                    "image": "https://messagecardplayground.azurewebsites.net/assets/FlowLogo.png"
                }
            },
            {
                "startGroup": true,
                "title": "请您审批Office 365开发入门指南培训",
                "activityImage": "https://connectorsdemo.azurewebsites.net/images/MSC12_Oscar_002.jpg",
                "activityTitle": "由张三发起的申请",
                "activitySubtitle": "zhangsan@xxx.com",
                "facts": [
                    {
                        "name": "提交时间:",
                        "value": "06/27/2018, 2:44 PM"
                    },
                    {
                        "name": "详细信息:",
                        "value": "请您帮忙审批，我很需要这方面的培训."
                    },
                    {
                        "name": "链接:",
                        "value": "[培训大纲](https://awesomedocument)"
                    }
                ]
            },
            {
                "potentialAction": [
                    {
                        "@type": "ActionCard",
                        "name": "批准",
                        "inputs": [
                            {
                                "@type": "TextInput",
                                "id": "comment",
                                "isMultiline": true,
                                "title": "原因 (可选)"
                            }
                        ],
                        "actions": [
                            {
                                "@type": "HttpPOST",
                                "name": "提交",
                                "target": "http://..."
                            }
                        ]
                    },
                    {
                        "@type": "ActionCard",
                        "name": "拒绝",
                        "inputs": [
                            {
                                "@type": "TextInput",
                                "id": "comment",
                                "isMultiline": true,
                                "title": "原因 (可选)"
                            }
                        ],
                        "actions": [
                            {
                                "@type": "HttpPOST",
                                "name": "提交",
                                "target": "http://..."
                            }
                        ]
                    }
                ]
            },
            {
                "startGroup": true,
                "activitySubtitle": "请审批流程"
            }
        ]
    }
    ```