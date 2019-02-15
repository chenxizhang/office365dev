Imports Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Tools.Ribbon

    ' 这个方法用来生成销售数据，并且基于这些数据生成一个柱状图表
    ' 作者：陈希章

Public Class Ribbon1

    Private Sub Ribbon1_Load(ByVal sender As System.Object, ByVal e As RibbonUIEventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As RibbonControlEventArgs) Handles Button1.Click
        Dim sh As Worksheet
        Dim app As Application = Globals.ThisAddIn.Application '引用当前应用程序
        sh = app.ActiveWorkbook.Worksheets.Add ' 不需要set语句了

        sh.Activate()

        Dim rng As Range

        rng = sh.Cells(1, 1)
        rng.Value = "Quarterly Sales Report"
        rng.Font.Name = "Century"
        rng.Font.Size = 26
        rng.Resize(1, 5).Merge()
        rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter '枚举不一样了



        Dim headerRow As Range
        headerRow = rng.Offset(1).Resize(, 5)
        headerRow.Value = {"Product", "Qtr1", "Qtr2", "Qtr3", "Qtr4"} '数组表达形式不一样了
        headerRow.Font.Bold = True

        Dim dataRng As Range
        dataRng = rng.Offset(2).Resize(6, 5)
        dataRng.Value = {{"Frames", 5000, 7000, 6544, 4377}, {"Saddles", 400, 323, 276, 651}, {"Brake levers", 12000, 8766, 8456, 9812}, {"Chains", 1550, 1088, 692, 853}, {"Mirrors", 225, 600, 923, 544}, {"Spokes", 6005, 7634, 4589, 8765}}
        dataRng.Columns.AutoFit()

        sh.Range("A1").ColumnWidth = 20
        With sh.Range("B1:E8")
            .ColumnWidth = 15
            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
        End With

        Dim co As ChartObject

        co = sh.ChartObjects().Add(0, sh.Range("A1:A8").Height + 5, sh.Range("A1:E1").Width, 200)

        co.Chart.ChartWizard(Source:=sh.Range("A2:E8"), Title:="Quarterly sales chart", Gallery:=Excel.XlChartType.xlColumnClustered)

    End Sub
End Class