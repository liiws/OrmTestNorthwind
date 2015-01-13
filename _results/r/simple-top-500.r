mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","LINQ2SQL","ADO.NET","BLToolkit","ObjectContext\nCompiled"),
    Values=c(1.67, 1.69, 1.70, 1.90, 0.94, 1.22, 1.19))

dev.new(width=7, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Simple TOP 500 (without context initialization)", ylab="Time, ms", beside=TRUE,
    col=rainbow(7), border="grey",
    ylim=c(0, 2.0),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)

text(x, mydata$Values+2.0/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
