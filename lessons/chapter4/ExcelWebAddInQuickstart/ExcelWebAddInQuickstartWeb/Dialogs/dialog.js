(function () {
    Office.onReady().then(function () {
        $(document).ready(function () {
            $("#btclose").click(function () {
                Office.context.ui.messageParent("close");//发送消息到主界面
            });
        });
    });
})();