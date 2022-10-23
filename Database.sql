CREATE DATABASE IF NOT EXISTS Pantree;
USE Pantree;

/* -------------------------------------------------------------------- */
-- tbl_FriendStatus
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_FriendStatus
(
	FriendStatusID			INT				NOT NULL			AUTO_INCREMENT,
	FriendStatus			VARCHAR(50)		NOT NULL,
	
  PRIMARY KEY (FriendStatusID),
  UNIQUE KEY UN_tbl_FriendStatus_FriendStatus (FriendStatus)
);

INSERT INTO tbl_FriendStatus (FriendStatus) 
VALUES
('Requested'),
('Accepted'),
('Declined'),
('Blocked'),
('CurrentUser'),
('NotFriends');

/* -------------------------------------------------------------------- */
-- tbl_Users
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_Users 
(
	UserID 					INT				NOT NULL 	AUTO_INCREMENT,
	UserName 				VARCHAR(255) 	NOT NULL,
	PasswordHash 			VARCHAR(255) 	NOT NULL,
	DisplayName 			VARCHAR(512) 	NOT NULL,
	EmailAddress 			VARCHAR(512) 	NOT NULL,

	PRIMARY KEY (UserID),
	UNIQUE KEY UN_tbl_Users_UserName (UserName)
);

/* -------------------------------------------------------------------- */
-- tbl_UserFriends
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_UserFriends 
(
	UserFriendID 			INT				NOT NULL 	AUTO_INCREMENT,
	UserID_Requester 		INT				NOT NULL,
	UserID_Addressee 		INT				NOT NULL,
	FriendStatusID 		    INT				NOT NULL,
	DateRequested 			DATETIME		NOT NULL 	DEFAULT CURRENT_TIMESTAMP(),
	DateAccepted 			DATETIME 		NULL,

	PRIMARY KEY (UserFriendID),
	CONSTRAINT FK_tbl_UserFriends_FriendStatusID FOREIGN KEY (FriendStatusID) REFERENCES tbl_FriendStatus (FriendStatusID),
	CONSTRAINT FK_tbl_UserFriends_UserID_Addressee FOREIGN KEY (UserID_Addressee) REFERENCES tbl_Users (UserID),
	CONSTRAINT FK_tbl_UserFriends_UserID_Requester FOREIGN KEY (UserID_Requester) REFERENCES tbl_Users (UserID)
);

/* -------------------------------------------------------------------- */
-- tbl_ProfileImages
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_ProfileImages
(
	ProfileImageID 		    INT	 			NOT NULL 	AUTO_INCREMENT,	
	UserID 					INT	 			NOT NULL,
	ProfileImage 			LONGBLOB 		NOT NULL,
	AlternativeText 		VARCHAR(255) 	NOT NULL,
  
	PRIMARY KEY (ProfileImageID),
	UNIQUE KEY UN_tbl_ProfileImages_UserID (UserID),
	CONSTRAINT FK_tbl_ProfileImages_UserID FOREIGN KEY (UserID) REFERENCES tbl_Users (UserID)
);

/* -------------------------------------------------------------------- */
-- tbl_Locations
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_Locations 
(
	LocationID 				INT	 			NOT NULL	AUTO_INCREMENT,
	UserID 					INT	 			NOT NULL,
	LocationName 			VARCHAR(100) 	NOT NULL,
	Description 			VARCHAR(512)	NULL,
	CreatedDate 			DATETIME 		NOT NULL 	DEFAULT CURRENT_TIMESTAMP(),
	Deleted 				BOOL			NOT NULL 	DEFAULT 0,
	DeletedDate 			DATETIME 		NULL,

	PRIMARY KEY (LocationID),
	CONSTRAINT FK_tbl_Locations_UserID FOREIGN KEY (UserID) REFERENCES tbl_Users (UserID)
);

/* -------------------------------------------------------------------- */
-- tbl_SharedLocations
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_SharedLocations 
(
	SharedID 				INT				NOT NULL 	AUTO_INCREMENT,
	UserID 					INT				NOT NULL,
	LocationID 				INT				NOT NULL,
	
	PRIMARY KEY (SharedID),
	UNIQUE KEY UN_tbl_SharedLocations_Unique (UserID, LocationID),
	CONSTRAINT FK_tbl_SharedLocations_LocationID FOREIGN KEY (LocationID) REFERENCES tbl_Locations (LocationID),
	CONSTRAINT FK_tbl_SharedLocations_UserID FOREIGN KEY (UserID) REFERENCES tbl_Users (UserID)
);

/* -------------------------------------------------------------------- */
-- tbl_Stores
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_Stores
(
	StoreID 				INT				NOT NULL 	AUTO_INCREMENT,
	UserID 					INT				NOT NULL,
	LocationID 				INT				NOT NULL,
	StoreName 				VARCHAR(100) 	NOT NULL,
	Description 			VARCHAR(512) 	NULL,
	CreatedDate 			DATETIME		NOT NULL 	DEFAULT CURRENT_TIMESTAMP(),
	Deleted 				BOOL 			NOT NULL 	DEFAULT 0,
	DeletedDate 			DATETIME		NULL,
	
	PRIMARY KEY (StoreID),
	CONSTRAINT FK_tbl_Stores_LocationID FOREIGN KEY (LocationID) REFERENCES tbl_Locations (LocationID),
	CONSTRAINT FK_tbl_Stores_UserID FOREIGN KEY (UserID) REFERENCES tbl_Users (UserID)
);

/* -------------------------------------------------------------------- */
-- tbl_Products
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_Products 
	ProductID 				INT				NOT NULL 	AUTO_INCREMENT,
	ProductCode 			VARCHAR(25)		NOT NULL,
	ProductName 			VARCHAR(50)		NOT NULL,
	IngredientList 		    VARCHAR(512)	NOT NULL,
	ImageUrl 				VARCHAR(255)	NOT NULL,
	LookupsSinceScan 		INT	 			NOT NULL 	DEFAULT 0,
	CreatedDate 			DATETIME		NOT NULL 	DEFAULT CURRENT_TIMESTAMP(),
	Deleted 				BOOL			NOT NULL 	DEFAULT 0,
	DeletedDate 			DATETIME		NULL,

	PRIMARY KEY (ProductID)
);

/* -------------------------------------------------------------------- */
-- tbl_Items
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_Items
(
	ItemID					INT				NOT NULL 	AUTO_INCREMENT,
	UserID					INT				NOT NULL,
	ProductID				INT				NOT NULL,
	ProductName				VARCHAR(50) 	NULL,
	IngredientList			VARCHAR(512) 	NULL,
	ImageUrl				VARCHAR(255) 	NULL,
	Notes					VARCHAR(512) 	NULL,
	CreatedDate				DATETIME		NOT NULL	DEFAULT CURRENT_TIMESTAMP(),
	Deleted					BOOL		 	NOT NULL 	DEFAULT 0,
	DeletedDate				DATETIME 		NULL,
	
	PRIMARY KEY (ItemID),
	CONSTRAINT FK_tbl_Items_ProductID FOREIGN KEY (ProductID) REFERENCES tbl_Products (ProductID),
	CONSTRAINT FK_tbl_Items_UserID FOREIGN KEY (UserID) REFERENCES tbl_Users (UserID)
);

/* -------------------------------------------------------------------- */
-- tbl_StoredItems
/* -------------------------------------------------------------------- */

CREATE TABLE IF NOT EXISTS tbl_StoredItems 
(
	StoreID 				INT	 			NOT NULL,
	ItemID 					INT				NOT NULL,
	Quantity 				INT				NOT NULL,
	
	PRIMARY KEY (StoreID, ItemID),
	CONSTRAINT FK_tbl_StoredItems_ItemID FOREIGN KEY (ItemID) REFERENCES tbl_Items (ItemID),
	CONSTRAINT FK_tbl_StoredItems_StoreID FOREIGN KEY (StoreID) REFERENCES tbl_Stores (StoreID)
);

/* -------------------------------------------------------------------- */
-- uvw_Items
/* -------------------------------------------------------------------- */

CREATE VIEW IF NOT EXISTS uvw_Items AS

SELECT		T2.ItemID AS 'ItemID',
			T1.ProductID AS 'ProductID',
			T2.UserID AS 'UserID',
			T1.ProductCode AS 'ProductCode',
			COALESCE(T2.ProductName, T1.ProductName) AS 'ProductName',
			COALESCE(T2.IngredientList, T1.IngredientList) AS 'IngredientList',
			COALESCE(T2.ImageUrl, T1.ImageUrl) AS 'ImageUrl',
			T2.Notes AS 'Notes',
			T1.LookupsSinceScan AS 'LookupsSinceScan',
			CASE 
				WHEN T3.Quantity IS NULL THEN 0 
				ELSE SUM(T3.Quantity) 
			END AS 'StoredQuantity' 
			
FROM 		tbl_Products T1 
LEFT JOIN 	tbl_Items T2 ON T1.ProductID = T2.ProductID
LEFT JOIN 	tbl_StoredItems T3 ON T2.ItemID = T3.ItemID
GROUP BY 	T1.ProductID, T2.UserID;

/* -------------------------------------------------------------------- */
-- uvw_Items_Store
/* -------------------------------------------------------------------- */

CREATE VIEW IF NOT EXISTS uvw_Items_Store AS

SELECT		T2.ItemID AS 'ItemID',
			T1.ProductID AS 'ProductID',
			T2.UserID AS 'UserID',
			T3.StoreID AS 'StoreID',
			T1.ProductCode AS 'ProductCode',
			COALESCE(T2.ProductName, T1.ProductName) AS 'ProductName',
			COALESCE(T2.IngredientList, T1.IngredientList) AS 'IngredientList',
			COALESCE(T2.ImageUrl, T1.ImageUrl) AS 'ImageUrl',
			T2.Notes AS 'Notes',
			T1.LookupsSinceScan AS 'LookupsSinceScan',
			CASE 
				WHEN T3.Quantity IS NULL THEN 0 
				ELSE SUM(T3.Quantity) 
			END AS 'Quantity'
				
FROM 		tbl_Products T1 
LEFT JOIN 	tbl_Items T2 ON T1.ProductID = T2.ProductID 
LEFT JOIN 	tbl_StoredItems T3 on T2.ItemID = T3.ItemID 
WHERE 		T3.StoreID IS NOT NULL 
GROUP BY 	T3.StoreID, T2.UserID, T1.ProductID;

/* -------------------------------------------------------------------- */
-- uvw_Items_Store_Search
/* -------------------------------------------------------------------- */

CREATE VIEW IF NOT EXISTS uvw_Items_Store_Search AS

SELECT		T1.ItemID AS 'ItemID',
			T1.ProductID AS 'ProductID',
			T1.UserID AS 'UserID',
			T1.StoreID AS 'StoreID',
			T4.LocationID AS 'LocationID',
			T4.LocationName AS 'LocationName',
			T3.StoreName AS 'StoreName',
			T1.ProductCode AS 'ProductCode',
			T1.ProductName AS 'ProductName',
			T1.IngredientList AS 'IngredientList',
			T1.ImageUrl AS 'ImageUrl',
			T1.Notes AS 'Notes',
			T1.LookupsSinceScan AS 'LookupsSinceScan',
			T1.Quantity AS 'Quantity',
			T2.TotalQuantity AS 'TotalQuantity' 
			
FROM 		uvw_Items_Store T1 
LEFT JOIN 	(SELECT	ItemID AS ItemID,
					UserID AS UserID,
					SUM(Quantity) AS TotalQuantity 
					FROM uvw_Items_Store 
					GROUP BY ItemID, UserID) T2 ON T1.ItemID = T2.ItemID AND T1.UserID = T2.UserID
JOIN 		tbl_Stores T3 ON T1.StoreID = T3.StoreID
JOIN 		tbl_Locations T4 ON T3.LocationID = T4.LocationID;