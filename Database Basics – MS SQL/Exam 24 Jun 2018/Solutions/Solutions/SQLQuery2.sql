INSERT INTO Accounts (FirstName, MiddleName, LastName, CityId, BirthDate, Email)
VALUES
('John', 'Smith', 'Smith', 34, '1975-07-21', 'j_smith@gmail.com'),
('Gosho', NULL, 'Petrov', 11, '1978-05-16', 'g_petrov@gmail.com'),
('Ivan', 'Petrovich', 'Pavlov', 59, '1849-09-26', 'i_pavlov@softuni.bg'),
('Friedrich', 'Wilhelm', 'Nietzsche', 2, '1844-10-15', 'f_nietzsche@softuni.bg')

INSERT INTO Trips (RoomId, BookDate, ArrivalDate, ReturnDate, CancelDate)
VALUES
(101, '2015-04-12', '2015-04-14', '2015-04-20', '2015-02-02'),
(102, '2015-07-07', '2015-07-15', '2015-07-22', '2015-04-29'),
(103, '2013-07-17', '2013-07-23', '2013-07-24', NULL),
(104, '2012-03-17', '2012-03-31', '2012-04-01', '2012-01-10'),
(109, '2017-08-07', '2017-08-28', '2017-08-29', NULL)


--3. Update
UPDATE Rooms
SET Price = Price * 1.14
WHERE HotelId IN (5, 7, 9)

--4. Delete
DELETE AccountsTrips
WHERE AccountId = 47

--5. Bulgarian Cities
SELECT c.Id, c.Name
FROM Cities AS c
WHERE c.CountryCode = 'BG'
ORDER BY c.Name

--6. People Born After 1991
SELECT CONCAT(c.FirstName, ' ', ISNULL(c.MiddleName + ' ', ''), C.LastName) AS [Full Name], YEAR(c.BirthDate) as [BirthYear]
FROM Accounts AS c
WHERE YEAR(c.BirthDate) > 1991
ORDER BY YEAR(c.BirthDate) DESC, c.FirstName

--7. EEE-Mails
SELECT a.FirstName, a.LastName, FORMAT(a.BirthDate, 'MM-dd-yyyy'), c.Name AS [Hometown], a.Email
FROM Accounts AS a
JOIN Cities AS c
ON c.Id = a.CityId
WHERE (a.Email LIKE 'e%')
ORDER BY c.Name DESC

--8. City Statistics
SELECT c.Name, COUNT(H.Id) AS Hotels
FROM Cities AS c
LEFT JOIN Hotels AS h
ON h.CityId = c.Id
GROUP BY c.Name
ORDER BY COUNT(H.Id) DESC, c.Name

--9. Expensive First-Class Rooms
SELECT r.Id, r.Price, h.Name AS Hotel, c.Name AS City
FROM Rooms AS r
JOIN Hotels AS h
ON h.Id = r.HotelId
JOIN Cities AS c
ON c.Id = h.CityId
WHERE r.Type = 'First Class'
ORDER BY r.Price DESC, r.Id

--10. Longest and Shortest Trips!!!!!!!!!!!!!!!!!
SELECT MIN(a.Id), CONCAT(a.FirstName, ' ', ISNULL(a.MiddleName + ' ', ''), a.LastName) AS [FullName]
FROM Accounts AS a
JOIN AccountsTrips AS atr
ON atr.AccountId = a.Id
JOIN Trips AS t
ON t.Id = atr.TripId
GROUP BY CONCAT(a.FirstName, ' ', ISNULL(a.MiddleName + ' ', ''), a.LastName)

SELECT MIN(h.AccountId), h.FullName, MAX(h.Days) AS LongestTrip, MIN(h.Days) AS ShortestTrip
FROM (
	SELECT atr.AccountId, CONCAT(a.FirstName, ' ', ISNULL(a.MiddleName + ' ', ''), a.LastName) AS [FullName], DAY(t.ReturnDate - t.ArrivalDate) AS Days
	FROM Trips AS t
	JOIN AccountsTrips AS atr
	ON atr.TripId = t.Id
	JOIN Accounts AS a
	ON a.Id = atr.AccountId
	--ORDER BY FullName
) AS h
GROUP BY h.FullName
ORDER BY LongestTrip DESC, MIN(h.AccountId)

--11. Metropolis
SELECT TOP (5) c.Id, c.Name, c.CountryCode AS Country, COUNT(a.Id)
FROM Cities AS c
JOIN Accounts AS a
ON a.CityId = c.Id
GROUP BY c.Name, c.Id, c.CountryCode
ORDER BY COUNT(a.Id) DESC

--12. Romantic Getaways
--SELECT a.Id, a.Email, c.Name AS City, ch.Name, ROW_NUMBER() OVER(PARTITION BY a.Email ORDER BY a.Email)
--FROM Accounts AS a
--JOIN AccountsTrips AS atr
--ON atr.AccountId = a.Id
--JOIN Trips AS t
--ON t.Id = atr.TripId
--JOIN Rooms AS r
--ON r.Id = t.RoomId
--JOIN Hotels AS h
--ON h.Id = r.HotelId
--JOIN Cities as c
--ON c.Id = h.CityId
--JOIN Cities as ch
--ON ch.id = a.CityId
--WHERE C.Name = CH.Name

SELECT a.Id, a.Email, c.Name AS City, COUNT(a.Id) AS Trips
FROM Accounts AS a
JOIN AccountsTrips AS atr
ON atr.AccountId = a.Id
JOIN Trips AS t
ON t.Id = atr.TripId
JOIN Rooms AS r
ON r.Id = t.RoomId
JOIN Hotels AS h
ON h.Id = r.HotelId
JOIN Cities as c
ON c.Id = h.CityId
JOIN Cities as ch
ON ch.id = a.CityId
WHERE C.Name = CH.Name
GROUP BY a.Id, a.Email, c.Name
ORDER BY Trips DESC, a.Id

--13. Lucrative Destinations
SELECT TOP (10) c.Id, c.Name, SUM(h.BaseRate + r.Price) as [Total Revenue], COUNT(t.Id) AS [Trips]
FROM Hotels AS h
JOIN Rooms AS r
ON r.HotelId = h.Id
JOIN Trips AS t
ON t.RoomId = r.Id
JOIN Cities AS c
ON c.Id = h.CityId
WHERE YEAR(t.BookDate) = 2016
GROUP BY c.Id, c.Name
ORDER BY SUM(h.BaseRate + r.Price) DESC, Trips DESC

--14. Trip Revenues
SELECT t.Id, 
	   h.Name AS [HotelName], 
	   r.Type AS [RoomType], 
	   CASE
	      WHEN t.CancelDate IS NOT NULL THEN 0
	      ELSE (h.BaseRate + r.Price)
	   END AS [Revenue]--, *
FROM Hotels AS h
JOIN Rooms AS r
ON r.HotelId = h.Id
JOIN Trips AS t
ON t.RoomId = r.Id
ORDER BY r.Type, t.Id



SELECT *
FROM Trips AS t
JOIN Rooms AS r
ON r.Id = t.RoomId
JOIN Hotels AS h
ON h.Id = r.HotelId
ORDER BY r.Type, t.Id


SELECT *
FROM AccountsTrips
ORDER BY TripId

SELECT *
FROM Trips T
FULL OUTER JOIN AccountsTrips AT on AT.TripId = T.Id


--15. Top Travelers
SELECT h.Id, h.Email, h.CountryCode, h.Trips
FROM (
	SELECT a.Id, a.Email, c.CountryCode, COUNT(a.Id) AS Trips,
		   ROW_NUMBER() OVER (PARTITION BY c.CountryCode ORDER BY COUNT(a.Id) DESC) AS [Rank]
	  FROM Accounts AS a
	 JOIN AccountsTrips AS atr
	   ON atr.AccountId = a.Id
	 JOIN Trips AS t
	   ON t.Id = atr.TripId
	 JOIN Rooms AS r
	   ON r.Id = t.RoomId
	 JOIN Hotels AS h
	   ON h.Id = r.HotelId
	 JOIN Cities as c
	   ON c.Id = h.CityId
	GROUP BY a.Id, a.Email, c.CountryCode
) AS h
WHERE [Rank] = 1
ORDER BY h.Trips DESC

--16. Luggage Fees
SELECT atr.TripId, atr.Luggage, CONCAT('$', (atr.Luggage * 5)) AS Fee
  FROM AccountsTrips AS atr
  JOIN Trips AS t
    ON t.Id = atr.TripId
 WHERE atr.Luggage > 0
ORDER BY atr.Luggage DESC

SELECT *
  FROM AccountsTrips AS atr
  JOIN Trips AS t
    ON t.Id = atr.TripId
 WHERE atr.Luggage > 0
ORDER BY atr.Luggage DESC


--17. GDPR Violation
   SELECT t.Id,
          CONCAT(a.FirstName, ' ', ISNULL(a.MiddleName + ' ', ''), a.LastName) AS [Full Name],
	      ch.Name AS [From],
	      c.Name AS [To],
	      CASE
	         WHEN t.CancelDate IS NOT NULL THEN 'Canceled'
	         ELSE CONCAT(CAST(DAY(t.ReturnDate - t.ArrivalDate) AS NVARCHAR(30)), ' days')
	      END AS [Duration]
     FROM Accounts AS a
	 JOIN AccountsTrips AS atr
	   ON atr.AccountId = a.Id
	 JOIN Trips AS t
	   ON t.Id = atr.TripId
	 JOIN Rooms AS r
	   ON r.Id = t.RoomId
	 JOIN Hotels AS h
	   ON h.Id = r.HotelId
	 JOIN Cities as c
	   ON c.Id = h.CityId
	 JOIN Cities as ch
       ON ch.id = a.CityId
 ORDER BY CONCAT(a.FirstName, ' ', ISNULL(a.MiddleName + ' ', ''), a.LastName), t.Id


--18. Available Room
CREATE FUNCTION udf_GetAvailableRoom(@HotelId INT, @Date DATETIME, @People INT)
RETURNS INT
AS
BEGIN
	SELECT *
	FROM Hotels AS h
	JOIN Rooms AS 
	DECLARE @Price INT = ()
	RETURN @ReportsCount
END



--20. Cancel Trip
CREATE TRIGGER tr_DeletedTrip ON Trips INSTEAD OF DELETE
AS
BEGIN
    DECLARE @CancelDate DATETIME 
	   SELECT @CancelDate = DELETED.CancelDate       
       FROM DELETED
	IF @CancelDate IS not NULL
	BEGIN
		UPDATE Trips
		SET CancelDate = GETDATE();
		RETURN
	END
END