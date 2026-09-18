SELECT

    U.id AS UserId,
    U.partnerReference AS NumeroSocio,
    U.firstName,
    U.lastNameF,
    U.lastNameM,

    U.birthDate,

    -- Sexo
    CASE 
        WHEN U.sex = 1 THEN 'Hombre'
        WHEN U.sex = 2 THEN 'Mujer'
        ELSE 'No especificado'
    END AS Sexo,

    -- Edad
    CASE
        WHEN U.birthDate IS NULL THEN NULL
        ELSE
            DATEDIFF(YEAR, U.birthDate, GETDATE())
            - CASE
                WHEN DATEADD(
                    YEAR,
                    DATEDIFF(YEAR, U.birthDate, GETDATE()),
                    U.birthDate
                  ) > GETDATE()
                THEN 1
                ELSE 0
              END
    END AS Edad,

    U.curp,
    U.rfc,
    U.ine,

    U.active AS UserActivo,

    SDP.Telefono,
    SDP.celular,
    SDP.email,
    SDP.Ocupacion,
    SDP.Escolaridad,
    SDP.fechaingreso,

    UA.supermanzana,
    UA.manzana,
    UA.lote,
    UA.street,
    UA.colony,
    UA.postalCode,
    UA.cityId,
    UA.stateId,
    UA.zoneId,
    UA.latitude,
    UA.longitude

FROM dbo.[user] U

LEFT JOIN dbo.Soc_DatPersonales SDP
    ON U.partnerReference = SDP.Numero

LEFT JOIN dbo.userAddress UA
    ON U.id = UA.userId

WHERE
    U.active = 1 

    -- Solo números en partnerReference
    AND U.partnerReference IS NOT NULL
    AND U.partnerReference <> ''
    AND U.partnerReference NOT LIKE '%[^0-9]%'

    -- Filtrado de direcciones
    AND UA.supermanzana <> '000'
    AND UA.supermanzana = '027';