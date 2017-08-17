CreateGraph <- function(title, fileName, inputData)
{
  cols_num <- length(inputData)
  
  zeros <- rep(0, cols_num*2)
  
  names <- c()
  cols_for_matrix <- c()
  all_values <- c()
  for (i in seq(1, cols_num))
  {
    names <- c(names, inputData[[i]][1:1])
    cols_for_matrix <- c(cols_for_matrix, inputData[[i]][2:3])
    all_values <- c(all_values, inputData[[i]][2:3])
    if (i != cols_num)
      cols_for_matrix <- c(cols_for_matrix, zeros)
  }
  data <- matrix(cols_for_matrix, ncol=cols_num)
  
  col_data <- rainbow(7)
  col_ctx <- rep("#F3F3F3", cols_num)
  seq2 = seq(1, 2*cols_num)
  col <- ifelse(seq2 %% 2 == 1, col_data[seq2/2], col_ctx[seq2/2])
  
  values <- as.numeric(all_values[seq2 %% 2 == 1]) + as.numeric(all_values[seq2 %% 2 == 0])
  
  plot_top = max(values)*1.05
  
  width <- 700
  height <- 500
  
  png(
    file = paste0(dirname(sys.frame(1)$ofile), "/", fileName),
    width = width, height = height, type = "cairo")
  
  cex <- 0.9
  
  x <- barplot(data,
               main=title, ylab="Time, ms",
               names.arg=names, col=col, border="grey", space=1,
               ylim=c(0, plot_top),
               cex.axis=cex, cex.lab=cex, cex.main=cex, cex.names=cex
  )
  
  text(x, values+plot_top/40, labels=round(values, 2), col="black", cex=cex)
  
  dev.off()
}
