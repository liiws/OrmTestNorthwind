mydata <- data.frame(
    row.names=c("EF6 CodeFirst","EF6 CodeFirst\nraw SQL","ADO.NET","LINQ to DB","LINQ to DB\nraw SQL","EF Core"),
    Values=c(0.152, 0.087, 0.044, 0.077, 0.030, 0.072))

dev.new(width=6, height=5)

plot_top = max(mydata$Values)*1.05

x <- barplot(t(as.matrix(mydata)),
    main="Context Initialization", ylab="Time, ms", beside=TRUE,
    col=rainbow(7), border="grey",
    ylim=c(0, plot_top),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)
    
text(x, mydata$Values+plot_top/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
