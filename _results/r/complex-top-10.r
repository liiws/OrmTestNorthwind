mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","ADO.NET","BLToolkit"),
    Values=c(22.38, 22.38, 24.76, 13.52, 13.56))

dev.new(width=5, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Complex TOP 10 (without context initialization)", ylab="Time, ms", beside=TRUE,
    col=rainbow(6), border="grey",
    ylim=c(0, 26),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)

text(x, mydata$Values+26/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
