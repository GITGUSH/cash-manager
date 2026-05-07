# Cash Manager

Personal cash management system developed with C#, ASP.NET Minimal API and PostgreSQL.

---

# Overview

Cash Manager is a full stack financial management system designed to help organize:

- Accounts
- Categories
- Financial operations
- User authentication

The project follows a simple and scalable architecture using ASP.NET Minimal API and PostgreSQL.

---

# Technologies

- C#
- ASP.NET Minimal API
- PostgreSQL
- Npgsql
- BCrypt.Net
- JWT Authentication

---

## How to run local

1. Clone the repository
2. Create the PostgreSQL database
3. Execute the SQL scripts in /scripts folder
4. Setup the .env file with the database credencials.
5. Run the project with 'dotnet run'

---

# Features

## Authentication

- User registration
- Secure password hashing with BCrypt
- JWT token generation
- Login validation

---

## Account Management

- Create accounts
- Update account information
- Remove accounts
- List user accounts

Examples:
- Wallet
- Bank account
- Savings account
- Investment account

---

## Category Management

Organize operations by categories.

Examples:
- Food
- Salary
- Transport
- Entertainment
- Investments

---

## Financial Operations

Register financial activities such as:

- Income
- Expenses
- Transfers

Each operation can contain:
- Description
- Amount
- Date
- Related account
- Category

---







