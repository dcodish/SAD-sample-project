-- Create the SAD_0 database
CREATE DATABASE SAD_0;
GO

USE SAD_0;
GO

-- Titles — טבלת Lookup לתפקידים
-- טבלת עזר שמכילה את הערכים התקינים לתפקידים
-- הערכים מוגדרים גם כ-Enum ב-C# (Title.cs)
CREATE TABLE Titles (
    titleId INT NOT NULL,
    titleName NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_TITLE PRIMARY KEY (titleId)
);
GO

-- Create Workers table
-- שימו לב: NVARCHAR תומך בעברית ותווים מיוחדים, בניגוד ל-VARCHAR
-- workerTitle נשמר כטקסט (לא כ-FK) — תואם את ערכי ה-Enum ב-C#
CREATE TABLE Workers (
    workerId VARCHAR(20) NOT NULL,
    workerName NVARCHAR(20) NULL,
    workerTitle NVARCHAR(50) NULL,
    CONSTRAINT PK_WORKER PRIMARY KEY (workerId)
);
GO

-- Create Orders table
CREATE TABLE Orders (
    workerId VARCHAR(20) NULL,
    orderId INT NOT NULL,
    orderDate DATE NULL,
    orderTotalPrice INT NULL,
    CONSTRAINT PK_ORDER1 PRIMARY KEY (orderId),
    CONSTRAINT FK_ORDER_ASSOCIATI_WORKER FOREIGN KEY (workerId) REFERENCES Workers(workerId)
);
GO

-- Stored Procedures

CREATE PROCEDURE dbo.Get_all_Titles
AS
    SELECT titleId, titleName FROM dbo.Titles;
GO

CREATE PROCEDURE dbo.Get_all_Workers
AS
    SELECT workerId, workerName, workerTitle FROM dbo.Workers;
GO

CREATE PROCEDURE dbo.Get_all_Orders
AS
    SELECT * FROM dbo.Orders;
GO

CREATE PROCEDURE dbo.SP_add_worker
    @id VARCHAR(20),
    @name NVARCHAR(20),
    @title NVARCHAR(50)
AS
    INSERT INTO dbo.Workers (workerId, workerName, workerTitle) VALUES (@id, @name, @title);
GO

CREATE PROCEDURE dbo.SP_Update_worker
    @id VARCHAR(20),
    @name NVARCHAR(20),
    @title NVARCHAR(50)
AS
    UPDATE dbo.Workers
    SET workerName = @name, workerTitle = @title
    WHERE workerId = @id;
GO

CREATE PROCEDURE dbo.SP_delete_worker
    @id VARCHAR(20)
AS
    DELETE FROM dbo.Orders WHERE workerId = @id;
    DELETE FROM dbo.Workers WHERE workerId = @id;
GO

CREATE PROCEDURE dbo.SP_add_order
    @worker VARCHAR(20),
    @orderId INT,
    @orderDate DATE,
    @OrderTotalPrice INT
AS
    INSERT INTO dbo.Orders (workerId, orderId, orderDate, orderTotalPrice)
    VALUES (@worker, @orderId, @orderDate, @OrderTotalPrice);
GO

-- טבלאות ירושה (Table-per-Subclass)
-- כל טבלת בן מכילה רק את השדות הייחודיים לה + Foreign Key לטבלת האב

-- DeliveryOrders - הזמנות משלוח
CREATE TABLE DeliveryOrders (
    orderId INT NOT NULL,
    deliveryAddress VARCHAR(100) NULL,
    deliveryDate DATE NULL,
    CONSTRAINT PK_DELIVERY_ORDER PRIMARY KEY (orderId),
    CONSTRAINT FK_DELIVERY_ORDER FOREIGN KEY (orderId) REFERENCES Orders(orderId)
);
GO

-- PickupOrders - הזמנות איסוף
CREATE TABLE PickupOrders (
    orderId INT NOT NULL,
    pickupTime DATETIME NULL,
    branchLocation VARCHAR(50) NULL,
    CONSTRAINT PK_PICKUP_ORDER PRIMARY KEY (orderId),
    CONSTRAINT FK_PICKUP_ORDER FOREIGN KEY (orderId) REFERENCES Orders(orderId)
);
GO

-- Stored Procedures עבור טבלאות הירושה

CREATE PROCEDURE dbo.SP_add_delivery_order
    @orderId INT,
    @deliveryAddress VARCHAR(100),
    @deliveryDate DATE
AS
    INSERT INTO dbo.DeliveryOrders (orderId, deliveryAddress, deliveryDate)
    VALUES (@orderId, @deliveryAddress, @deliveryDate);
GO

CREATE PROCEDURE dbo.SP_add_pickup_order
    @orderId INT,
    @pickupTime DATETIME,
    @branchLocation VARCHAR(50)
AS
    INSERT INTO dbo.PickupOrders (orderId, pickupTime, branchLocation)
    VALUES (@orderId, @pickupTime, @branchLocation);
GO

-- שאילתה שמחזירה את כל ההזמנות עם פרטי הירושה
-- LEFT JOIN מאפשר לקבל את כל ההזמנות כולל אלו שאינן משלוח או איסוף
CREATE PROCEDURE dbo.Get_all_Orders_Full
AS
    SELECT o.workerId, o.orderId, o.orderDate, o.orderTotalPrice,
           d.deliveryAddress, d.deliveryDate,
           p.pickupTime, p.branchLocation
    FROM dbo.Orders o
    LEFT JOIN dbo.DeliveryOrders d ON o.orderId = d.orderId
    LEFT JOIN dbo.PickupOrders p ON o.orderId = p.orderId;
GO

-- טבלת מוצרים
CREATE TABLE Products (
    productId INT NOT NULL,
    productName NVARCHAR(50) NULL,
    price FLOAT NULL,
    category NVARCHAR(30) NULL,
    CONSTRAINT PK_PRODUCT PRIMARY KEY (productId)
);
GO

-- טבלת קישור (Association Class) — פריטי הזמנה
-- מייצגת קשר Many-to-Many בין Orders ל-Products
-- המפתח הראשי מורכב משני Foreign Keys
CREATE TABLE OrderItems (
    orderId INT NOT NULL,
    productId INT NOT NULL,
    quantity INT NULL,
    unitPrice FLOAT NULL,
    CONSTRAINT PK_ORDER_ITEM PRIMARY KEY (orderId, productId),
    CONSTRAINT FK_ORDERITEM_ORDER FOREIGN KEY (orderId) REFERENCES Orders(orderId),
    CONSTRAINT FK_ORDERITEM_PRODUCT FOREIGN KEY (productId) REFERENCES Products(productId)
);
GO

-- Stored Procedures עבור Products ו-OrderItems

CREATE PROCEDURE dbo.Get_all_Products
AS
    SELECT * FROM dbo.Products;
GO

CREATE PROCEDURE dbo.SP_add_product
    @id INT,
    @name NVARCHAR(50),
    @price FLOAT,
    @category NVARCHAR(30)
AS
    INSERT INTO dbo.Products VALUES (@id, @name, @price, @category);
GO

CREATE PROCEDURE dbo.Get_all_OrderItems
AS
    SELECT orderId, productId, quantity, unitPrice FROM dbo.OrderItems;
GO

CREATE PROCEDURE dbo.SP_add_order_item
    @orderId INT,
    @productId INT,
    @quantity INT,
    @unitPrice FLOAT
AS
    INSERT INTO dbo.OrderItems VALUES (@orderId, @productId, @quantity, @unitPrice);
GO

-- Sample data

-- Lookup values — תפקידים
INSERT INTO Titles VALUES (1, N'מנהל משמרת');
INSERT INTO Titles VALUES (2, N'ראש צוות');
INSERT INTO Titles VALUES (3, N'עובד חדש');
GO

-- Workers — workerTitle נשמר כטקסט (תואם את ערכי ה-Enum ב-C#)
INSERT INTO Workers VALUES ('1111', N'admin', N'מנהל משמרת');
INSERT INTO Workers VALUES ('123', N'shelly', N'מנהל משמרת');
INSERT INTO Workers VALUES ('345', N'liel', N'ראש צוות');
INSERT INTO Workers VALUES ('678', N'david', N'מנהל משמרת');
GO

-- הזמנת משלוח - שורה בטבלת האב + שורה בטבלת הבן
INSERT INTO Orders VALUES ('123', 1, '2022-12-01', 200);
INSERT INTO DeliveryOrders VALUES (1, 'Rager 12, Beer Sheva', '2022-12-05');

-- הזמנת איסוף - שורה בטבלת האב + שורה בטבלת הבן
INSERT INTO Orders VALUES ('345', 2, '2022-12-02', 300);
INSERT INTO PickupOrders VALUES (2, '2022-12-03 14:00:00', 'Beer Sheva Branch');

-- הזמנת משלוח נוספת
INSERT INTO Orders VALUES ('678', 3, '2022-12-03', 400);
INSERT INTO DeliveryOrders VALUES (3, 'Herzl 5, Tel Aviv', '2022-12-07');
GO

-- מוצרים
INSERT INTO Products VALUES (1, N'מקלדת', 150.00, N'אלקטרוניקה');
INSERT INTO Products VALUES (2, N'עכבר', 80.00, N'אלקטרוניקה');
INSERT INTO Products VALUES (3, N'מסך', 1200.00, N'אלקטרוניקה');
INSERT INTO Products VALUES (4, N'כיסא משרדי', 450.00, N'ריהוט');
GO

-- פריטי הזמנה (Association Class) — קשר Many-to-Many בין Orders ל-Products
-- הזמנה 1 מכילה מקלדת ועכבר
INSERT INTO OrderItems VALUES (1, 1, 2, 150.00);   -- 2 מקלדות
INSERT INTO OrderItems VALUES (1, 2, 1, 80.00);    -- 1 עכבר

-- הזמנה 2 מכילה מסך
INSERT INTO OrderItems VALUES (2, 3, 1, 1200.00);  -- 1 מסך

-- הזמנה 3 מכילה מקלדת וכיסא
INSERT INTO OrderItems VALUES (3, 1, 3, 150.00);   -- 3 מקלדות
INSERT INTO OrderItems VALUES (3, 4, 2, 450.00);   -- 2 כיסאות
GO
