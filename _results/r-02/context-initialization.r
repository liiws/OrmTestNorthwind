mydata <- data.frame(
    row.names=c("CodeFirst","CodeFirst\nraw SQL","ADO.NET","BLToolkit\nraw","LINQ to DB","LINQ to DB\nraw SQL"),
    Values=c(0.225, 0.137, 0.047, 0.053, 0.119, 0.034))

dev.new(width=6, height=5)

plot_top = max(mydata$Values)*1.05

x <- barplot(t(as.matrix(mydata)),
    main="Context Initialization", ylab="Time, ms", beside=TRUE,
    col=rainbow(7), border="grey",
    ylim=c(0, plot_top),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)
    
text(x, mydata$Values+plot_top/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
