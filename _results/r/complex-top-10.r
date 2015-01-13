mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","LINQ2SQL","ADO.NET","BLToolkit"),
    Values=c(10.5, 10.6, 12.2, 8.8, 4.1, 4.1))
    
dev.new(width=6, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Complex TOP 10 (without context initialization)", ylab="Time, ms", beside=TRUE,
    col=rainbow(7), border="grey",
    ylim=c(0, 13),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)

text(x, mydata$Values+13/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
