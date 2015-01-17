data_cf=c(1.67, 0.172)
data_d=c(1.69, 0.212)
data_oc=c(1.70, 0.376)
data_l2s=c(1.90, 0.093)
data_ado=c(0.94, 0.033)
data_blt=c(1.22, 0.044)
data_occ=c(1.19, 0.376)


xx <- rep(0, 2*7)
data <- matrix(c(data_cf, xx, data_d, xx, data_oc, xx, data_l2s, xx, data_ado, xx, data_blt, xx, data_occ), ncol=7)

col_data <- rainbow(7)
col_ctx <- rep("#F3F3F3", 7)
seq2 = seq(1, 7*2)
col <- ifelse(seq2 %% 2 == 1, col_data[seq2/2], col_ctx[seq2/2])

all_values <- c(data_cf, data_d, data_oc, data_l2s, data_ado, data_blt, data_occ)
values = all_values[seq2 %% 2 == 1] + all_values[seq2 %% 2 == 0]

plot_top = max(values)*1.05

names <- c("DbContext\nCodeFirst", "DbContext\nDesigner", "ObjectContext\nEdmGen", "LINQ2SQL", "ADO.NET", "BLToolkit", "ObjectContext\nCompiled")

dev.new(width=7, height=5)

x <- barplot(data,
    main="Simple TOP 500 (with context initialization)", ylab="Time, ms",
    names.arg=names, col=col, border="grey", space=1,
    ylim=c(0, plot_top),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7
    )

text(x, values+plot_top/40, labels=round(values, 2), col="black", cex=0.7)


