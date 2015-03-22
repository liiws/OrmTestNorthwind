data_cf=c(11.71, 0.225)
data_cfr=c(5.018, 0.137)
data_ado=c(4.619, 0.047)
data_blt=c(4.707, 0.053)
data_l2d=c(5.116, 0.119)
data_l2dr=c(4.407, 0.034)

cols_num <- 6

xx <- rep(0, 2*cols_num)
data <- matrix(c(data_cf, xx, data_cfr, xx, data_ado, xx, data_blt, xx, data_l2d, xx, data_l2dr), ncol=cols_num)

col_data <- rainbow(7)
col_ctx <- rep("#F3F3F3", cols_num)
seq2 = seq(1, 2*cols_num)
col <- ifelse(seq2 %% 2 == 1, col_data[seq2/2], col_ctx[seq2/2])

all_values <- c(data_cf, data_cfr, data_ado, data_blt, data_l2d, data_l2dr)
values = all_values[seq2 %% 2 == 1] + all_values[seq2 %% 2 == 0]

plot_top = max(values)*1.05

names <- c("CodeFirst", "CodeFirst\nraw SQL", "ADO.NET", "BLToolkit\nraw SQL", "LINQ to DB", "LINQ to DB\nraw SQL")

dev.new(width=cols_num, height=5)

x <- barplot(data,
    main="Complex TOP 10 (with context initialization)", ylab="Time, ms",
    names.arg=names, col=col, border="grey", space=1,
    ylim=c(0, plot_top),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7
    )

text(x, values+plot_top/40, labels=round(values, 2), col="black", cex=0.7)


