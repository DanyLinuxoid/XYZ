# Payment Gateway Integration API

This Web API integrates multiple payment systems (Gateways) for processing transactions.

## Setup

1. Import database from db-backup using SQL Server Management Studio (SSMS)
2. Launch Visual Studio

## Prerequisites

1. Start the project in Visual Studio
2. Wait for Swagger UI to load on port 7777

## Usage Scenarios

### Use Case 1: System Health Check

1. Expand the "Utility" section
2. Navigate to the `/smoketest` endpoint
3. Click "Try it out" → "Execute"
4. Expected result: `"isSystemFunctional": true` (database is operational)

### Use Case 2: Payment Validation Error Testing

1. Go to the "Billing" section
2. Navigate to the `/process` endpoint
3. Click "Try it out"
4. Insert the following values in the request body:
   ```json
   {
     "payableAmount": 0.01,
     "orderNumber": 1,
     "userId": 1,
     "paymentGateway": "Unknown",
     "description": "string"
   }
   ```
5. Expected result: Validation errors from the server

### Use Case 3: User Not Found Error Testing

1. Go to the "Billing" section
2. Navigate to the `/process` endpoint
3. Click "Try it out"
4. Insert the following values in the request body:
   ```json
   {
     "payableAmount": 0.01,
     "orderNumber": 1,
     "userId": 1,
     "paymentGateway": "Paypal",
     "description": ""
   }
   ```
5. Expected result: "User not found" error

### Use Case 4: User Creation

1. Go to the "User" section
2. Navigate to the "create new user" endpoint
3. Click "Try it out" → "Execute"
4. Expected result: New user identifier is returned

### Use Case 5: Successful Payment Processing

1. Complete Use Case 4 and note the user identifier
2. Go to the "Billing" section
3. Navigate to the `/process` endpoint
4. Click "Try it out"
5. Insert the following values in the request body:
   ```json
   {
     "payableAmount": 0.01,
     "orderNumber": 1,
     "userId": <user identifier from Use Case 4>,
     "paymentGateway": "Paypal",
     "description": ""
   }
   ```
6. Expected result: Receipt is returned

### Use Case 6: External Gateway Failure Testing

1. Complete Use Case 4 and note the user identifier
2. Go to the "Billing" section
3. Navigate to the `/process` endpoint
4. Click "Try it out"
5. Insert the following values in the request body:
   ```json
   {
     "payableAmount": 0.01,
     "orderNumber": 2,
     "userId": <user identifier from Use Case 4>,
     "paymentGateway": "Paysera",
     "description": ""
   }
   ```
6. Expected result: 502 Bad Gateway error "Paysera Api failed to process order"
