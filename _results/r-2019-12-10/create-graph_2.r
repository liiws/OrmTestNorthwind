CreateGraph2 <- function(title, fileName, inputData)
{
  groupNames <- c(".NET", "Core")
  colorsDotnet <- c("#82D7FF", "#EAF9FF")
  colorsCore <- c("#FA5054", "#FEEBEB")

  data_count <- length(inputData)

  plot_top <- 0
  for (i in seq(1, data_count))
  {
    plot_top <- max(plot_top, sum(as.numeric(inputData[[i]][2:3])), sum(as.numeric(inputData[[i]][4:5])))
  }
  plot_top <- plot_top*1.05

  width <- 700
  height <- 500

  png(
    file = paste0(dirname(sys.frame(1)$ofile), "/", fileName),
    width = width, height = height, type = "cairo")

  barplot(matrix(c("")), main=title, ylab="Time, ms", space=0, xlim=c(0, data_count*3-1), ylim=c(0,plot_top), las=1)
  for (i in seq(1, data_count))
  {
    dataDotnet <- matrix(as.numeric(inputData[[i]][2:3]), ncol=1)
    dataCore <- matrix(as.numeric(inputData[[i]][4:5]), ncol=1)
    x <- barplot(dataDotnet, space=(i-1)*3,   las=1, col=colorsDotnet, add=TRUE, border="grey", names.arg=c(inputData[[i]][[1]]))
    text(x, sum(dataDotnet)+plot_top/40, labels=round(sum(dataDotnet), 2), col="black")
    x <- barplot(dataCore,   space=(i-1)*3+1, las=1, col=colorsCore,   add=TRUE, border="grey")
    text(x, sum(dataCore)+plot_top/40, labels=round(sum(dataCore), 2), col="black")
  }

  legend("topleft",
         legend = groupNames,
         fill = c(colorsDotnet[[1]], colorsCore[[1]]),
         border="grey",
         box.col = "grey")

  dev.off()
}
