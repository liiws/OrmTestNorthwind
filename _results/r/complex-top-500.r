mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","LINQ2SQL","ADO.NET","BLToolkit"),
    Values=c(13.0, 13.0, 14.6, 11.2, 6.6, 7.0))

dev.new(width=6, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Complex TOP 500 (without context initialization)", ylab="Time, ms", beside=TRUE,
    col=rainbow(7), border="grey",
    ylim=c(0, 16),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)

text(x, mydata$Values+16/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
