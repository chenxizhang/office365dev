// 每次加载新页面时都必须运行初始化函数
(function () {
    Office.initialize = function (reason) {
        // 如果你需要初始化，可以在此进行。
    };
})();

var dialog = null;

function showSampleData() {
    Excel.run(function (ctx) {
        var sheet = ctx.workbook.worksheets.getActiveWorksheet();
        var values = [
            [Math.floor(Math.random() * 1000), Math.floor(Math.random() * 1000), Math.floor(Math.random() * 1000)],
            [Math.floor(Math.random() * 1000), Math.floor(Math.random() * 1000), Math.floor(Math.random() * 1000)],
            [Math.floor(Math.random() * 1000), Math.floor(Math.random() * 1000), Math.floor(Math.random() * 1000)]
        ];
        // 将向电子表格写入示例数据的命令插入队列
        sheet.getRange("B3:D5").values = values;

        Office.context.ui.displayDialogAsync("https://localhost:44306/Dialogs/dialog.html", { height: 30, width: 20, displayInIframe: true }, function (result) {
            dialog = result.value;
            dialog.addEventHandler(Microsoft.Office.WebExtension.EventType.DialogMessageReceived, processMessage);
        });

        // 运行排队的命令，并返回承诺表示任务完成
        return ctx.sync();
    });
}



function processMessage(arg) {
    //这里可以处理消息 arg
    dialog.close();
}
