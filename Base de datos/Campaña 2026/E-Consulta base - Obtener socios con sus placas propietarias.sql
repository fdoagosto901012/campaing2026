;WITH Base AS
(
    SELECT
        U.id AS UserId,
        LTRIM(RTRIM(U.partnerReference)) AS NumeroSocio,

        U.firstName,
        U.lastNameF,
        U.lastNameM,

        U.birthDate,

        CASE
            WHEN U.sex = 1 THEN 'Hombre'
            WHEN U.sex = 2 THEN 'Mujer'
            ELSE 'No especificado'
        END AS Sexo,

        CASE
            WHEN U.birthDate IS NULL THEN NULL
            ELSE
                DATEDIFF(YEAR, U.birthDate, GETDATE())
                -
                CASE
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

        SDP.Telefono AS TelefonoOriginal,
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
        ON CONVERT(VARCHAR(50), SDP.Numero) =
           LTRIM(RTRIM(U.partnerReference))

    LEFT JOIN dbo.userAddress UA
        ON U.id = UA.userId

    WHERE
        U.active = 1
        AND U.partnerReference IS NOT NULL
        AND LTRIM(RTRIM(U.partnerReference)) <> ''
        AND LTRIM(RTRIM(U.partnerReference)) NOT LIKE '%[^0-9]%'
        AND UA.supermanzana IN ('024', '027')
),

/* ============================================================
   SOCIOS UNICOS

   La persona se identifica exclusivamente por:

       firstName
       lastNameF
       lastNameM

   Se eliminan NumeroSocio repetidos.
   ============================================================ */

SociosConNumero AS
(
    SELECT DISTINCT
        firstName,
        lastNameF,
        lastNameM,
        NumeroSocio
    FROM Base
),

/* ============================================================
   AGRUPAMOS LOS NUMEROS DE SOCIO

   Los colocamos de MENOR A MAYOR.
   ============================================================ */

NumerosAgrupados AS
(
    SELECT
        S.firstName,
        S.lastNameF,
        S.lastNameM,

        STUFF
        (
            (
                SELECT
                    ' / ' + S2.NumeroSocio
                FROM SociosConNumero S2
                WHERE
                    ISNULL(S2.firstName, '') =
                    ISNULL(S.firstName, '')

                    AND ISNULL(S2.lastNameF, '') =
                    ISNULL(S.lastNameF, '')

                    AND ISNULL(S2.lastNameM, '') =
                    ISNULL(S.lastNameM, '')

                ORDER BY
                    LEN(S2.NumeroSocio),
                    S2.NumeroSocio

                FOR XML PATH('')
            ),
            1,
            3,
            ''
        ) AS NumerosSocio,

        COUNT(*) AS CantidadNumeroSocio

    FROM SociosConNumero S

    GROUP BY
        S.firstName,
        S.lastNameF,
        S.lastNameM
)

/* ============================================================
   RESULTADO
   ============================================================ */

SELECT
    firstName,
    lastNameF,
    lastNameM,

    NumerosSocio,

    CantidadNumeroSocio

FROM NumerosAgrupados

ORDER BY
	CantidadNumeroSocio desc,
    lastNameF,
    lastNameM,
    firstName;