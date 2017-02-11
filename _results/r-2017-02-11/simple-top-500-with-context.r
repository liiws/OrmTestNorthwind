data_cf=c(1.812, 0.152)
data_cfr=c(1.326, 0.087)
data_ado=c(1.004, 0.044)
data_l2d=c(1.129, 0.077)
data_l2dr=c(1.044, 0.030)
data_efc=c(1.707, 0.072)

cols_num <- 6

xx <- rep(0, 2*cols_num)
data <- matrix(c(data_cf, xx, data_cfr, xx, data_ado, xx, data_l2d, xx, data_l2dr, xx, data_efc), ncol=cols_num)

col_data <- rainbow(7)
col_ctx <- rep("#F3F3F3", cols_num)
seq2 = seq(1, 2*cols_num)
col <- ifelse(seq2 %% 2 == 1, col_data[seq2/2], col_ctx[seq2/2])

all_values <- c(data_cf, data_cfr, data_ado, data_l2d, data_l2dr, data_efc)
values = all_values[seq2 %% 2 == 1] + all_values[seq2 %% 2 == 0]

plot_top = max(values)*1.05

names <- c("EF6 CodeFirst","EF6 CodeFirst\nraw SQL","ADO.NET","LINQ to DB","LINQ to DB\nraw SQL","EF Core")

dev.new(width=cols_num, height=5)

x <- barplot(data,
    main="Simple TOP 500 (with context initialization)", ylab="Time, ms",
    names.arg=names, col=col, border="grey", space=1,
    ylim=c(0, plot_top),
    cex.axis=0.7, cex.lab=0.7, cex.main=0.7, cex.names=0.7
    )

text(x, values+plot_top/40, labels=round(values, 2), col="black", cex=0.7)


