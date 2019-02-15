Sub ReportGeneration()
    ' 这个方法用来生成销售数据，并且基于这些数据生成一个柱状图表
    ' 作者：陈希章
    
    Dim sh As Worksheet
    Set sh = ThisWorkbook.Worksheets.Add
    
    sh.Activate
    
    Dim rng As Range
    
    Set rng = sh.Cells(1, 1)
    rng.Value = "Quarterly Sales Report"
    rng.Font.Name = "Century"
    rng.Font.Size = 26
    rng.Resize(1, 5).Merge
    rng.HorizontalAlignment = xlCenter
    
    Dim headerRow As Range
    Set headerRow = rng.Offset(1).Resize(, 5)
    headerRow.Value = [{"Product", "Qtr1", "Qtr2", "Qtr3", "Qtr4"}]
    headerRow.Font.Bold = True
    
    Dim dataRng As Range
    Set dataRng = rng.Offset(2).Resize(6, 5)
    dataRng.Value = [{"Frames", 5000, 7000, 6544, 4377;"Saddles", 400, 323, 276, 651;"Brake levers", 12000, 8766, 8456, 9812;"Chains", 1550, 1088, 692, 853;"Mirrors", 225, 600, 923, 544;"Spokes", 6005, 7634, 4589, 8765}]
    dataRng.Columns.AutoFit
    
    sh.Range("A1").ColumnWidth = 20
    With sh.Range("B1:E8")
        .ColumnWidth = 15
        .HorizontalAlignment = xlCenter
    End With
    
    Dim co As ChartObject
    
    Set co = sh.ChartObjects().Add(0, sh.Range("A1:A8").Height + 5, sh.Range("A1:E1").Width, 200)
    
    co.chart.ChartWizard Source:=sh.Range("A2:E8"), Title:="Quarterly sales chart", Gallery:=xlColumnClustered
   
End Sub