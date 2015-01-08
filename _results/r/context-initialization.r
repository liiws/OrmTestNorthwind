mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","ADO.NET","BLToolkit"),
    Values=c(0.285, 0.3453, 0.649, 0.0847, 0.099))

dev.new(width=5, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Context Initialization", ylab="Time, ms", beside=TRUE,
    col=rainbow(6), border="grey",
    ylim=c(0,0.67),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)
    
text(x, mydata$Values+0.67/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
