source(paste0(dirname(sys.frame(1)$ofile), "/", "create-graph_2.r"))

title <- "Simple TOP 10 (with context initialization)"
fileName <- "simple-top-10.png"
inputData <- list(
  c("ADO", c(0.089666667, 0.044), c(0.064333333, 0.069)),
  c("L2DB", c(0.144666667, 0.050666667), c(0.089666667, 0.118)),
  c("L2DB\nraw SQL", c(0.091333333, 0.044), c(0.087333333, 0.03)),
  c("EFCore", c(0.263666667, 0.100333333), c(0.148333333, 0.262666667)),
  c("EFCore\nraw SQL", c(0.217333333, 0.067333333), c(0.187, 0.101666667))
)
CreateGraph2(title, fileName, inputData)

title <- "Simple TOP 500 (with context initialization)"
fileName <- "simple-top-500.png"
inputData <- list(
  c("ADO", c(0.818666667, 0.044), c(0.767333333, 0.069)),
  c("L2DB", c(0.895333333, 0.050666667), c(0.780666667, 0.118)),
  c("L2DB\nraw SQL", c(0.803666667, 0.044), c(0.760666667, 0.03)),
  c("EFCore", c(1.093666667, 0.100333333), c(0.862, 0.262666667)),
  c("EFCore\nraw SQL", c(1.148666667, 0.067333333), c(0.985, 0.101666667))
)
CreateGraph2(title, fileName, inputData)

title <- "Comple TOP 10 (with context initialization)"
fileName <- "complex-top-10.png"
inputData <- list(
  c("ADO", c(3.832, 0.044), c(3.669666667, 0.069)),
  c("L2DB", c(3.997, 0.050666667), c(3.931333333, 0.118)),
  c("L2DB\nraw SQL", c(3.717666667, 0.044), c(3.788333333, 0.03)),
  c("EFCore", c(4.315666667, 0.100333333), c(3.988666667, 0.262666667)),
  c("EFCore\nraw SQL", c(3.852666667, 0.067333333), c(3.811333333, 0.101666667))
)
CreateGraph2(title, fileName, inputData)

title <- "Complex TOP 500 (with context initialization)"
fileName <- "complex-top-500.png"
inputData <- list(
  c("ADO", c(5.594333333, 0.044), c(5.587666667, 0.069)),
  c("L2DB", c(5.888333333, 0.050666667), c(5.815, 0.118)),
  c("L2DB\nraw SQL", c(5.572666667, 0.044), c(5.560666667, 0.03)),
  c("EFCore", c(6.243333333, 0.100333333), c(5.246333333, 0.262666667)),
  c("EFCore\nraw SQL", c(6.226666667, 0.067333333), c(5.751, 0.101666667))
)
CreateGraph2(title, fileName, inputData)
