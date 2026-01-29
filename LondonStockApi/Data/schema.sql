IF OBJECT_ID('dbo.Trades', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Trades
    (
        TradeId       UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Ticker        NVARCHAR(20)     NOT NULL,
        Price         DECIMAL(19,4)    NOT NULL,
        Shares        DECIMAL(19,4)    NOT NULL,
        BrokerId      NVARCHAR(64)     NOT NULL,
        TimestampUtc  DATETIME2(7)     NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE INDEX IX_Trades_Ticker 
        ON dbo.Trades (Ticker);
END;


IF OBJECT_ID('dbo.StockAggregates', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.StockAggregates
    (
        Ticker          NVARCHAR(20)  NOT NULL PRIMARY KEY,
        SumPrice        DECIMAL(38,6) NOT NULL,
        CountTrades     BIGINT        NOT NULL,
        LastUpdatedUtc  DATETIME2(7)  NOT NULL
    );
END;