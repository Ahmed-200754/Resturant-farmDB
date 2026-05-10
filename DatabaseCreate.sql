CREATE DATABASE FarmToTableDB;

USE FarmToTableDB;

CREATE TABLE Crops (
    CropID INT IDENTITY(1,1) PRIMARY KEY,
    CropName VARCHAR(50) NOT NULL
);

CREATE TABLE Farm (
    FarmID INT IDENTITY(1,1) PRIMARY KEY,
    FarmName VARCHAR(50) NOT NULL,
    FarmerName VARCHAR(50) NOT NULL,
    FarmerPhone VARCHAR(20) NOT NULL,
    Longitude DECIMAL(9,6) NOT NULL,
    Latitude DECIMAL(9,6) NOT NULL
);

CREATE TABLE HarvestBatches (
    BatchID INT IDENTITY(1,1) PRIMARY KEY,
    HarvestDate DATE NOT NULL,
    Quantity FLOAT NOT NULL,
    FreshnessWindow DATE NOT NULL,
    CropID INT NOT NULL REFERENCES Crops(CropID),
    FarmID INT NOT NULL REFERENCES Farm(FarmID)
);

CREATE TABLE Restaurants (
    RestaurantID INT IDENTITY(1,1) PRIMARY KEY,
    RestaurantName VARCHAR(50) NOT NULL,
    Address VARCHAR(70) NOT NULL,
    DeliveryWindow TIME NOT NULL
);

CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    OrderDate DATE NOT NULL,
    RestaurantID INT NOT NULL REFERENCES Restaurants(RestaurantID)
);

CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    UnitPrice FLOAT NOT NULL,
    Quantity FLOAT NOT NULL,
    OrderID INT NOT NULL REFERENCES Orders(OrderID),
    BatchID INT NOT NULL REFERENCES HarvestBatches(BatchID)
);

CREATE TABLE Driver (
    DriverID INT IDENTITY(1,1) PRIMARY KEY,
    DriverName VARCHAR(50) NOT NULL,
    DriverDOB DATE NOT NULL,
    DriverPhone VARCHAR(20) NOT NULL
);

CREATE TABLE TripToRestaurant (
    TripID INT IDENTITY(1,1) PRIMARY KEY,
    TripDate DATE NOT NULL,
    Distance INT NOT NULL,
    RouteTaken VARCHAR(100) NOT NULL,
    DriverID INT NOT NULL REFERENCES Driver(DriverID),
    FarmID INT NOT NULL REFERENCES Farm(FarmID)
);
