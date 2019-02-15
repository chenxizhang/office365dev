$("#create-report").click(() => tryCatch(createReport));
//    这个方法用来生成销售数据，并且基于这些数据生成一个柱状图表
//    作者：陈希章


/** Load sample data into a new worksheet and create a chart */
async function createReport() {
    await Excel.run(async (context) => {
        const sheet = context.workbook.worksheets.add();

        try {
            await writeSheetData(sheet);
            sheet.activate();
            await context.sync();
        } catch (error) {
            // Try to activate the new sheet regardless, to show
            // how far the processing got before failing
            sheet.activate();
            await context.sync();

            // Then re-throw the original error, for appropriate error-handling
            // (in this snippet, simply showing a notification)
            throw error;
        }
    });

    OfficeHelpers.UI.notify("Sucess!", "Report generation completed.");
}

async function writeSheetData(sheet: Excel.Worksheet) {
    // Set the report title in the worksheet
    const titleCell = sheet.getCell(0, 0);
    titleCell.values = [["Quarterly Sales Report"]];
    titleCell.format.font.name = "Century";
    titleCell.format.font.size = 26;

    // Create an array containing sample data
    const headerNames = ["Product", "Qtr1", "Qtr2", "Qtr3", "Qtr4"];
    const data = [
        ["Frames", 5000, 7000, 6544, 4377],
        ["Saddles", 400, 323, 276, 651],
        ["Brake levers", 12000, 8766, 8456, 9812],
        ["Chains", 1550, 1088, 692, 853],
        ["Mirrors", 225, 600, 923, 544],
        ["Spokes", 6005, 7634, 4589, 8765]
    ];

    // Write the sample data to the specified range in the worksheet
    // and bold the header row
    const headerRow = titleCell.getOffsetRange(1, 0).getResizedRange(0, headerNames.length - 1);
    headerRow.values = [headerNames];
    headerRow.getRow(0).format.font.bold = true;

    const dataRange = headerRow.getOffsetRange(1, 0).getResizedRange(data.length - 1, 0);
    dataRange.values = data;

    titleCell.getResizedRange(0, headerNames.length - 1).merge();
    dataRange.format.autofitColumns();

    const columnRanges = headerNames.map((header, index) => dataRange.getColumn(index).load("format/columnWidth"));
    await sheet.context.sync();

    // For the header (product name) column, make it a minimum of 100px;
    const firstColumn = columnRanges.shift();
    if (firstColumn.format.columnWidth < 100) {
        console.log("Expanding the first column to 100px");
        firstColumn.format.columnWidth = 100;
    }

    // For the remainder, make them identical or a minimum of 60px
    let minColumnWidth = 60;
    columnRanges.forEach((column, index) => {
        console.log(`Column #${index + 1}: auto-fitted width = ${column.format.columnWidth}`);
        minColumnWidth = Math.max(minColumnWidth, column.format.columnWidth);
    });
    console.log(`Setting data columns to a width of ${minColumnWidth} pixels`);
    dataRange.getOffsetRange(0, 1).getResizedRange(0, -1).format.columnWidth = minColumnWidth;

    // Add a new chart
    const chart = sheet.charts.add(
        Excel.ChartType.columnClustered,
        headerRow.getOffsetRange(0, 0).getResizedRange(6, 0),
        Excel.ChartSeriesBy.columns
    );

    // Set the properties and format the chart
    const chartTopRow = dataRange.getLastRow().getOffsetRange(2, 0);
    chart.setPosition(chartTopRow, chartTopRow.getOffsetRange(14, 0));
    chart.title.text = "Quarterly sales chart";
    chart.legend.position = "right";
    chart.legend.format.fill.setSolidColor("white");
    chart.dataLabels.format.font.size = 15;
    chart.dataLabels.format.font.color = "black";

    const points = chart.series.getItemAt(0).points;
    points.getItemAt(0).format.fill.setSolidColor("pink");
    points.getItemAt(1).format.fill.setSolidColor("indigo");
}

/** Default helper for invoking an action and handling errors. */
async function tryCatch(callback) {
    try {
        await callback();
    } catch (error) {
        OfficeHelpers.UI.notify(error);
        OfficeHelpers.Utilities.log(error);
    }
}
