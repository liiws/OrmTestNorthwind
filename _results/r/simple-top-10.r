mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","ADO.NET","BLToolkit","ObjectContext\nCompiled"),
    Values=c(1.165, 1.157, 0.957, 0.2053, 0.2423, 0.2253))

dev.new(width=6, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Simple TOP 10 (without context initialization)", ylab="Time, ms", beside=TRUE,
    col=rainbow(6), border="grey",
    ylim=c(0, 1.25),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)

text(x, mydata$Values+1.25/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
