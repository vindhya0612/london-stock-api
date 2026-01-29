# LondonStockApi
The London Stock API is an MVP system designed for the London Stock Exchange to receive real‑time trade notifications from authorized brokers and expose updated stock values. The API provides endpoints to submit trades and retrieve stock prices individually, in bulk, or by ticker list.
A relational database is used to persist all transactions, and the current value of a stock is defined as the average price across all recorded trades for that stock

The system receives real-time trade events containing:

Ticker symbol
Traded price (in GBP)
Number of shares (decimal allowed)
Broker ID
Stored in a relational database as individual transactions

The API exposes:

Current value of a single stock
Values of all stocks
Values for a list of ticker symbols (batch query)

The value is determined by the average transaction price for each stock.

Below are clean steps for running the API locally using .NET 8.
1. Clone the Repository
2. Install Dependencies
	Restore .NET Dependencies
3. Database Setup
	Update the connection string in appsettings.json
4. Run the Application
	Start the API
	API will run at: http://localhost:7145;http://localhost:5145
5. Open Swagger UI
	Navigate to: http://localhost:7145/swagger
	You’ll see:
		POST /api/trades
		GET /api/stocks/{ticker}
		GET /api/stocks
		POST /api/stocks/batch
6. Run Unit Tests
	dotnet test
		