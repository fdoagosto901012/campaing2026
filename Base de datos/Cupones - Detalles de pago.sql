CREATE TABLE CouponPayments
(
    CouponPaymentId INT IDENTITY(1,1) PRIMARY KEY,
    TotalAmount DECIMAL(18,2) NOT NULL,
    userId INT NULL,
    PaymentMethod TINYINT NOT NULL DEFAULT(1), --1 Efectivo 2 Tarjeta
    Reference NVARCHAR(100) NULL,
    CreatedBy INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
    Active BIT NOT NULL DEFAULT(1),
    Cancelled BIT NOT NULL DEFAULT(0),
    CancelledDate DATETIME NULL,
    CancelledBy INT NULL,
    Comments NVARCHAR(500) NULL,

	CONSTRAINT FK_CouponPayment_users
        FOREIGN KEY (userId)
        REFERENCES [user](id),
);


CREATE TABLE CouponPaymentDetails
(
    CouponPaymentDetailId INT IDENTITY(1,1) PRIMARY KEY,

    CouponPaymentId INT NOT NULL,
    CuponsId INT NOT NULL,

    Amount DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_CouponPaymentDetails_Payment
        FOREIGN KEY (CouponPaymentId)
        REFERENCES CouponPayments(CouponPaymentId),

    CONSTRAINT FK_CouponPaymentDetails_Cupon
        FOREIGN KEY (CuponsId)
        REFERENCES Cupons(CuponsId)
);

ALTER TABLE Cupons
ADD CouponPaymentId INT NULL;

ALTER TABLE Cupons
ADD CONSTRAINT FK_Cupons_CouponPayments
FOREIGN KEY (CouponPaymentId)
REFERENCES CouponPayments(CouponPaymentId);

ALTER TABLE Cupons
ADD Paid BIT NOT NULL DEFAULT(0);

ALTER TABLE Cupons
ADD PaymentDate DATETIME NULL;