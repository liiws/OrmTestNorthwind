mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","ADO.NET","BLToolkit","ObjectContext\nCompiled"),
    Values=c(3.324, 3.299, 3.081, 2.186, 2.386, 2.375))

dev.new(width=6, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Simple TOP 500 (without context initialization)", ylab="Time, ms", beside=TRUE,
    col=rainbow(6), border="grey",
    ylim=c(0, 3.5),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)

text(x, mydata$Values+3.5/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
