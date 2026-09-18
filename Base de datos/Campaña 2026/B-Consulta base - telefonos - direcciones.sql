SELECT

    --U.id AS UserId,
    U.partnerReference AS Numero_Socio,
    U.firstName AS Nombre,
    U.lastNameF AS Apellido_paterno,
    U.lastNameM AS Apellido_materno,

    U.birthDate AS fecha_nacimiento,

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

    --U.curp,
    --U.rfc,
    --U.ine,

    --U.active AS UserActivo,

    -- Unificar Teléfono y Celular
    CASE
        WHEN SDP.Telefono IS NOT NULL
             AND LTRIM(RTRIM(SDP.Telefono)) <> ''
             AND LTRIM(RTRIM(SDP.Telefono)) <> '00000000'
        THEN LTRIM(RTRIM(SDP.Telefono))

        WHEN SDP.celular IS NOT NULL
             AND LTRIM(RTRIM(SDP.celular)) <> ''
             AND LTRIM(RTRIM(SDP.celular)) <> '00000000'
        THEN LTRIM(RTRIM(SDP.celular))

        ELSE NULL
    END AS Telefono,

    -- Tiene al menos un teléfono
    CASE
        WHEN (
            SDP.Telefono IS NOT NULL
            AND LTRIM(RTRIM(SDP.Telefono)) <> ''
            AND LTRIM(RTRIM(SDP.Telefono)) <> '00000000'
        )
        OR (
            SDP.celular IS NOT NULL
            AND LTRIM(RTRIM(SDP.celular)) <> ''
            AND LTRIM(RTRIM(SDP.celular)) <> '00000000'
        )
        THEN 'Sí'
        ELSE 'No'
    END AS TieneTelefono,

    --SDP.email,
    --SDP.Ocupacion,
    --SDP.Escolaridad,
    --SDP.fechaingreso,

    UA.supermanzana,
    UA.manzana,
    UA.lote,
    UA.street,
    UA.colony,
    UA.postalCode
    --UA.cityId,
    --UA.stateId,
    --UA.zoneId,
    --UA.latitude,
    --UA.longitude

FROM dbo.[user] U

LEFT JOIN dbo.Soc_DatPersonales SDP
    ON U.partnerReference = SDP.Numero

LEFT JOIN dbo.userAddress UA
    ON U.id = UA.userId

WHERE
    U.active = 1

    -- Solo socios cuyo partnerReference contiene exclusivamente números
    AND U.partnerReference IS NOT NULL
    AND U.partnerReference <> ''
    AND U.partnerReference NOT LIKE '%[^0-9]%'

    -- Super manzana
    AND (
		UA.supermanzana = '024'
		OR UA.supermanzana = '027'
	)

	/*
    -- SIN ningún número telefónico disponible
    AND (
        (SDP.Telefono IS NULL OR LTRIM(RTRIM(SDP.Telefono)) IN ('', '00000000'))
        AND
        (SDP.celular IS NULL OR LTRIM(RTRIM(SDP.celular)) IN ('', '00000000'))
    )
	*/
	
	/*
	-- CON al menos un número telefónico disponible
    AND (
        (SDP.Telefono IS NOT NULL 
         AND LTRIM(RTRIM(SDP.Telefono)) NOT IN ('', '00000000'))
        OR
        (SDP.celular IS NOT NULL 
         AND LTRIM(RTRIM(SDP.celular)) NOT IN ('', '00000000'))
    )
	*/

	ORDER BY
    UA.supermanzana ASC,
    U.firstName ASC,
    U.lastNameF ASC,
    U.lastNameM ASC,
    UA.manzana ASC,
    UA.lote ASC;