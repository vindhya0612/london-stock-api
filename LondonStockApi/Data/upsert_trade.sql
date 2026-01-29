IF OBJECT_ID('dbo.UpsertTrade', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpsertTrade;
GO

CREATE PROCEDURE dbo.UpsertTrade
    @TradeId      UNIQUEIDENTIFIER,
    @Ticker       NVARCHAR(20),
    @Price        DECIMAL(19,4),
    @Shares       DECIMAL(19,4),
    @BrokerId     NVARCHAR(64),
    @TimestampUtc DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;

    -- Ignore if trade already exists
    IF EXISTS (SELECT 1 FROM dbo.Trades WHERE TradeId = @TradeId)
        RETURN;

    -- Insert the trade
    INSERT INTO dbo.Trades
    (
        TradeId,
        Ticker,
        Price,
        Shares,
        BrokerId,
        TimestampUtc
    )
    VALUES
    (
        @TradeId,
        @Ticker,
        @Price,
        @Shares,
        @BrokerId,
        @TimestampUtc
    );

    -- Upsert into StockAggregates
    MERGE dbo.StockAggregates AS tgt
    USING (SELECT @Ticker AS Ticker) AS src
        ON tgt.Ticker = src.Ticker
    WHEN MATCHED THEN
        UPDATE SET
            SumPrice        = tgt.SumPrice + @Price,
            CountTrades     = tgt.CountTrades + 1,
            LastUpdatedUtc  = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (Ticker, SumPrice, CountTrades, LastUpdatedUtc)
        VALUES (@Ticker, @Price, 1, SYSUTCDATETIME());
END
GO