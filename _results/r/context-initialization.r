mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","LINQ2SQL","ADO.NET","BLToolkit"),
    Values=c(0.172, 0.212, 0.376, 0.093, 0.033, 0.044))

dev.new(width=6, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Context Initialization", ylab="Time, ms", beside=TRUE,
    col=rainbow(7), border="grey",
    ylim=c(0,0.4),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)
    
text(x, mydata$Values+0.4/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
