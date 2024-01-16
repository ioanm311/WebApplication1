IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShoppingCart]') AND type in (N'U'))
    DROP TABLE ShoppingCart;

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[products]') AND type in (N'U'))
    DROP TABLE products;

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[users]') AND type in (N'U'))
    DROP TABLE users;

CREATE TABLE users
(
    Id INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(255) NOT NULL,
    LastName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Salt NVARCHAR(255) NOT NULL
);

CREATE TABLE products (
    Id INT PRIMARY KEY IDENTITY,
    Image NVARCHAR (MAX)  NULL,
    Name NVARCHAR (255)  NOT NULL,
    Description NVARCHAR (MAX)  NULL,
    Price DECIMAL (18, 2) NOT NULL,
    ProductType NVARCHAR (255)  NULL
);

CREATE TABLE ShoppingCart
(
    ShoppingCartId INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    ProductName NVARCHAR(255) NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES users(Id)
);
