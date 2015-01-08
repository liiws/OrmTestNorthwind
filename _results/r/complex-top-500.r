mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","ADO.NET","BLToolkit"),
    Values=c(27.57, 27.47, 29.90, 18.19, 18.57))

dev.new(width=5, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Complex TOP 500 (without context initialization)", ylab="Time, ms", beside=TRUE,
    col=rainbow(6), border="grey",
    ylim=c(0, 32),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)

text(x, mydata$Values+32/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
