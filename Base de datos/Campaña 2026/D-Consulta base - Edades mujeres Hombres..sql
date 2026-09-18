;WITH Base AS
(
    SELECT
        U.id AS UserId,
        U.partnerReference AS NumeroSocio,

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

        UA.supermanzana

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
        --AND UA.supermanzana IN ('024', '027')
),

/* ============================================================
   CONSOLIDAMOS PERSONAS QUE TIENEN MÁS DE UN PARTNERREFERENCE
   ============================================================ */

SociosUnicos AS
(
    SELECT
        firstName,
        lastNameF,
        lastNameM,

        MAX(UserId) AS UserId,

        COUNT(DISTINCT NumeroSocio) AS CantidadPartnerReferences,

        MAX(Edad) AS Edad,

        MAX(
            CASE
                WHEN Sexo = 'Hombre' THEN 1
                ELSE 0
            END
        ) AS EsHombre,

        MAX(
            CASE
                WHEN Sexo = 'Mujer' THEN 1
                ELSE 0
            END
        ) AS EsMujer,

        MAX(
            CASE
                WHEN Sexo = 'No especificado' THEN 1
                ELSE 0
            END
        ) AS EsNoEspecificado

    FROM Base

    GROUP BY
        firstName,
        lastNameF,
        lastNameM
),

Clasificados AS
(
    SELECT
        *,

        CASE
            WHEN Edad IS NULL
                THEN 'Edad no especificada'

            WHEN Edad < 18
                THEN 'Menor de 18'

            WHEN Edad BETWEEN 18 AND 29
                THEN '18-29'

            WHEN Edad BETWEEN 30 AND 39
                THEN '30-39'

            WHEN Edad BETWEEN 40 AND 49
                THEN '40-49'

            WHEN Edad BETWEEN 50 AND 59
                THEN '50-59'

            WHEN Edad BETWEEN 60 AND 69
                THEN '60-69'

            WHEN Edad BETWEEN 70 AND 79
                THEN '70-79'

            WHEN Edad >= 80
                THEN '80+'
        END AS RangoEdad

    FROM SociosUnicos
)

SELECT
    RangoEdad,

    SUM(EsHombre) AS Hombres,

    SUM(EsMujer) AS Mujeres,

    SUM(EsNoEspecificado) AS NoEspecificado,

    COUNT(*) AS TotalSocios

FROM Clasificados

GROUP BY
    RangoEdad

ORDER BY
    CASE RangoEdad
        WHEN 'Menor de 18' THEN 1
        WHEN '18-29' THEN 2
        WHEN '30-39' THEN 3
        WHEN '40-49' THEN 4
        WHEN '50-59' THEN 5
        WHEN '60-69' THEN 6
        WHEN '70-79' THEN 7
        WHEN '80+' THEN 8
        WHEN 'Edad no especificada' THEN 9
    END;