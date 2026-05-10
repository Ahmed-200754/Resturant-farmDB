
INSERT INTO Crops (CropName) VALUES
('Tomatoes'), ('Leafy Greens'), ('Root Vegetables'), ('Citrus Fruits'),
('Bell Peppers'), ('Herbs'), ('Squash'), ('Berries');



INSERT INTO Farm (FarmName, FarmerName, FarmerPhone, Longitude, Latitude) VALUES
('Delta Green Farms', 'Ahmed Hassan', '01012345678', 31.000000, 30.500000),
('Nile Organic', 'Mahmoud Ali', '01123456789', 31.100000, 30.400000),
('Alexandria Harvest', 'Khaled Ibrahim', '01234567890', 29.900000, 31.200000),
('Banha Fields', 'Mustafa Sayed', '01545678901', 31.150000, 30.450000),
('Fayoum Valley', 'Ali Omar', '01098765432', 30.800000, 29.300000),
('Giza Fresh', 'Hassan Tarek', '01187654321', 31.200000, 29.900000);



INSERT INTO HarvestBatches (HarvestDate, Quantity, FreshnessWindow, CropID, FarmID) VALUES
('2026-05-05', 200, '2026-05-15', 1, 1),
('2026-04-30', 350, '2026-05-09', 2, 1),
('2026-05-08', 150, '2026-05-20', 3, 2),
('2026-04-25', 500, '2026-05-05', 4, 3),
('2026-05-09', 100, '2026-05-17', 5, 4),
('2026-04-20', 250, '2026-04-30', 6, 5),
('2026-05-07', 400, '2026-05-14', 7, 6),
('2026-05-02', 300, '2026-05-12', 8, 1),
('2026-04-05', 200, '2026-04-15', 1, 2),
('2026-03-31', 100, '2026-04-10', 2, 3),
('2026-05-10', 50, '2026-05-24', 3, 4),
('2026-05-04', 120, '2026-05-13', 4, 5),
('2026-04-28', 450, '2026-05-08', 5, 6),
('2026-05-06', 220, '2026-05-16', 6, 1),
('2026-05-01', 310, '2026-05-09', 7, 2);



INSERT INTO Restaurants (RestaurantName, Address, DeliveryWindow) VALUES
('Cairo Kitchen', '12 Zamalek St, Cairo', '10:00:00'),
('The Great Pyramid Diner', '15 Giza Square, Giza', '13:00:00'),
('Nile Breeze Grill', 'Corniche El Nil, Maadi', '18:00:00'),
('Alexandria Seafood', '20 Stanly St, Alexandria', '11:00:00'),
('Delta Delights', '5 Tanta St, Tanta', '14:00:00'),
('Desert Oasis', '10 October City', '19:00:00');



INSERT INTO Orders (OrderDate, RestaurantID) VALUES
('2026-05-06', 1),
('2026-04-26', 2),
('2026-05-09', 3),
('2026-04-21', 4),
('2026-04-06', 1),
('2026-04-01', 2),
('2026-05-03', 3),
('2026-04-29', 4),
('2026-05-08', 1),
('2026-05-05', 2),
('2026-05-07', 1),
('2026-05-02', 2);




INSERT INTO OrderDetails (UnitPrice, Quantity, OrderID, BatchID) VALUES
(20.50, 10.0, 1, 1),
(15.00, 20.0, 1, 3),
(40.00, 5.0, 2, 4),
(25.00, 15.0, 3, 5),
(50.00, 8.0, 3, 7),
(30.00, 12.0, 4, 8),
(18.00, 25.0, 5, 9),
(22.00, 18.0, 6, 10),
(35.00, 30.0, 7, 12),
(28.00, 10.0, 7, 14),
(45.00, 15.0, 8, 15),
(16.00, 20.0, 9, 11),
(21.00, 12.0, 10, 1),
(19.00, 14.0, 11, 3),
(38.00, 22.0, 12, 4),
(26.00, 16.0, 1, 5),
(55.00, 9.0, 2, 7),
(32.00, 11.0, 3, 8),
(17.00, 26.0, 4, 11),
(23.00, 19.0, 5, 12);



INSERT INTO Driver (DriverName, DriverDOB, DriverPhone) VALUES
('Kareem Sobhy', '1990-05-15', '01023456789'),
('Sayed Kamel', '1985-08-20', '01134567890'),
('Osama Fawzy', '1992-11-10', '01245678901'),
('Tarek Nour', '1988-02-25', '01556789012'),
('Magdy Youssef', '1995-09-05', '01034567890');



INSERT INTO TripToRestaurant (TripDate, Distance, RouteTaken, DriverID, FarmID) VALUES
('2026-05-06', 45, 'Ring Road -> Cairo-Alex Desert Road', 1, 1),
('2026-04-26', 30, 'Autostrad', 1, 2),
('2026-05-09', 80, 'Alex Desert Road', 1, 3),
('2026-04-21', 60, 'Agricultural Road', 2, 4),
('2026-04-06', 50, 'Ring Road', 2, 5),
('2026-04-01', 40, 'Mehwar 26 July', 3, 6),
('2026-05-03', 70, 'Cairo-Suez Road', 4, 1),
('2026-04-29', 55, 'Autostrad -> Maadi', 5, 2),
('2026-05-08', 35, 'Ring Road', 1, 3),
('2026-05-05', 65, 'Alex Desert Road', 1, 4),
('2026-05-07', 25, 'Mehwar', 2, 5),
('2026-05-02', 85, 'Agricultural Road', 3, 6),
('2026-05-09', 45, 'Ring Road -> Cairo-Alex Desert Road', 1, 1),
('2026-05-10', 30, 'Autostrad', 1, 2),
('2026-05-10', 50, 'Ring Road', 2, 3);

