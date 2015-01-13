mydata <- data.frame(
    row.names=c("DbContext\nCodeFirst","DbContext\nDesigner","ObjectContext\nEdmGen","LINQ2SQL","ADO.NET","BLToolkit","ObjectContext\nCompiled"),
    Values=c(0.546, 0.538, 0.469, 0.867, 0.093, 0.111, 0.053))
    
dev.new(width=7, height=5)

x <- barplot(t(as.matrix(mydata)),
    main="Simple TOP 10 (without context initialization)", ylab="Time, ms", beside=TRUE,
    col=rainbow(7), border="grey",
    ylim=c(0, 0.9),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7)

text(x, mydata$Values+0.9/40, labels=round(mydata$Values, 2), col="black", cex=0.7)
