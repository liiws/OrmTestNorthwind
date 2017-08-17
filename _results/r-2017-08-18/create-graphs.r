source(paste0(dirname(sys.frame(1)$ofile), "/", "create-graph.r"))

title <- "Simple TOP 10 (with context initialization)"
fileName <- "simple-top-10.png"
inputData <- list(
  c("EF6", 0.642333333, 0.238666667),
  c("EF Core 2", 0.485, 0.21),
  c("EF Core 2\nraw SQL", 0.492, 0.179666667),
  c("EF Core 2\nCompiled", 0.280666667, 0.179666667),
  c("ADO.NET", 0.138, 0.057666667),
  c("LINQ to DB", 0.246333333, 0.068666667),
  c("LINQ to DB\nraw SQL", 0.153666667, 0.041)
)
CreateGraph(title, fileName, inputData)

title <- "Simple TOP 500 (with context initialization)"
fileName <- "simple-top-500.png"
inputData <- list(
  c("EF6", 1.990666667, 0.238666667),
  c("EF Core 2", 1.981333333, 0.21),
  c("EF Core 2\nraw SQL", 3.280333333, 0.179666667),
  c("EF Core 2\nCompiled", 1.668, 0.179666667),
  c("ADO.NET", 1.245333333, 0.057666667),
  c("LINQ to DB", 1.416, 0.068666667),
  c("LINQ to DB\nraw SQL", 1.292666667, 0.041)
)
CreateGraph(title, fileName, inputData)

title <- "Complex TOP 10 (with context initialization)"
fileName <- "complex-top-10.png"
inputData <- list(
  c("EF6", 12.36566667, 0.238666667),
  c("EF Core 2", 6.277, 0.21),
  c("EF Core 2\nCompiled", 5.633666667, 0.179666667),
  c("ADO.NET", 5.298666667, 0.057666667),
  c("LINQ to DB", 5.885666667, 0.068666667),
  c("LINQ to DB\nraw SQL", 5.343333333, 0.041)
)
CreateGraph(title, fileName, inputData)

title <- "Complex TOP 500 (with context initialization)"
fileName <- "complex-top-500.png"
inputData <- list(
  c("EF6", 15.282, 0.238666667),
  c("EF Core 2", 8.179333333, 0.21),
  c("EF Core 2\nCompiled", 8.706, 0.179666667),
  c("ADO.NET", 8.154333333, 0.057666667),
  c("LINQ to DB", 8.594333333, 0.068666667),
  c("LINQ to DB\nraw SQL", 8.234333333, 0.041)
)
CreateGraph(title, fileName, inputData)
