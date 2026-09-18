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
   CONVERTIMOS LOS SEPARADORES DE TELEFONOS EN |
   PERO CONSERVAMOS ESPACIOS Y GUIONES DENTRO DEL NUMERO
   ============================================================ */

Preparado AS
(
    SELECT
        B.*,

        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
            UPPER(ISNULL(B.TelefonoOriginal, '')),
            'TEL:', '|'),
            'TEL-', '|'),
            'TEL', '|'),
            'CEL:', '|'),
            'CEL-', '|'),
            'CEL', '|'),
            '/', '|'),
            ',', '|'),
            ';', '|'),
            ' Y ', '|')
        AS TextoSeparado

    FROM Base B
),

/* ============================================================
   SEPARAMOS CADA TELEFONO
   SIN STRING_SPLIT
   ============================================================ */

Partes AS
(
    SELECT
        P.*,
        LTRIM(RTRIM(X.N.value('.', 'varchar(200)'))) AS Parte

    FROM Preparado P

    CROSS APPLY
    (
        SELECT CAST(
            '<r><x>' +
            REPLACE(
                P.TextoSeparado,
                '|',
                '</x><x>'
            )
            + '</x></r>' AS XML
        ) AS XMLData
    ) XMLD

    CROSS APPLY
        XMLD.XMLData.nodes('/r/x') AS X(N)

    WHERE
        LTRIM(RTRIM(X.N.value('.', 'varchar(200)'))) <> ''
),

/* ============================================================
   LIMPIAMOS CADA PARTE

   Quitamos:
   espacios
   guiones
   paréntesis
   puntos
   :
   ============================================================ */

Limpios AS
(
    SELECT
        P.*,

        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
            P.Parte,
            ' ', ''),
            '-', ''),
            '(', ''),
            ')', ''),
            '.', ''),
            ':', '')
        AS NumeroLimpio

    FROM Partes P
),

/* ============================================================
   VALIDAMOS EL NUMERO
   ============================================================ */

Validos AS
(
    SELECT
        L.*,

        CASE
            WHEN L.NumeroLimpio NOT LIKE '%[^0-9]%'
                 AND LEN(L.NumeroLimpio) = 7
            THEN
                LEFT(L.NumeroLimpio,3)
                + ' '
                + RIGHT(L.NumeroLimpio,4)

            WHEN L.NumeroLimpio NOT LIKE '%[^0-9]%'
                 AND LEN(L.NumeroLimpio) = 10
            THEN
                '('
                + LEFT(L.NumeroLimpio,3)
                + ') '
                + SUBSTRING(L.NumeroLimpio,4,3)
                + ' '
                + RIGHT(L.NumeroLimpio,4)

            ELSE NULL
        END AS TelefonoFormateado,

        CASE
            WHEN L.NumeroLimpio NOT LIKE '%[^0-9]%'
                 AND LEN(L.NumeroLimpio) = 7
                THEN '7 DIGITOS'

            WHEN L.NumeroLimpio NOT LIKE '%[^0-9]%'
                 AND LEN(L.NumeroLimpio) = 10
                THEN '10 DIGITOS'

            WHEN L.NumeroLimpio IN
                (
                    '0',
                    '000',
                    '0000000',
                    '00000000',
                    '0000000000'
                )
                THEN 'SIN TELEFONO'

            ELSE 'REVISAR'
        END AS EstadoTelefono

    FROM Limpios L
),

/* ============================================================
   ELIMINAMOS TELEFONOS DUPLICADOS
   YA DESPUES DE HABERLOS FORMATEADO
   ============================================================ */

TelefonosUnicos AS
(
    SELECT
        V.*,

        ROW_NUMBER() OVER
        (
            PARTITION BY
                V.UserId,
                V.TelefonoFormateado
            ORDER BY
                V.Parte
        ) AS Repeticion

    FROM Validos V

    WHERE
        V.TelefonoFormateado IS NOT NULL
),

/* ============================================================
   ORDENAMOS TELEFONOS UNICOS
   ============================================================ */

Ordenados AS
(
    SELECT
        T.*,

        ROW_NUMBER() OVER
        (
            PARTITION BY T.UserId
            ORDER BY
                T.Parte
        ) AS NumeroOrden

    FROM TelefonosUnicos T

    WHERE
        T.Repeticion = 1
)

/* ============================================================
   RESULTADO FINAL
   ============================================================ */

SELECT
    B.UserId,
    B.NumeroSocio,

    B.firstName,
    B.lastNameF,
    B.lastNameM,

    B.birthDate,
    B.Sexo,
    B.Edad,

    B.curp,
    B.rfc,
    B.ine,

    B.UserActivo,

    B.TelefonoOriginal,

    MAX(
        CASE
            WHEN O.NumeroOrden = 1
            THEN O.TelefonoFormateado
        END
    ) AS Telefono1,

    MAX(
        CASE
            WHEN O.NumeroOrden = 2
            THEN O.TelefonoFormateado
        END
    ) AS Telefono2,

    MAX(
        CASE
            WHEN O.NumeroOrden = 3
            THEN O.TelefonoFormateado
        END
    ) AS Telefono3,

    MAX(
        CASE
            WHEN O.NumeroOrden = 4
            THEN O.TelefonoFormateado
        END
    ) AS Telefono4,

    CASE
        WHEN MAX(CASE WHEN O.NumeroOrden = 1 THEN O.TelefonoFormateado END) IS NOT NULL
        AND MAX(CASE WHEN O.NumeroOrden = 2 THEN O.TelefonoFormateado END) IS NOT NULL
        AND MAX(CASE WHEN O.NumeroOrden = 3 THEN O.TelefonoFormateado END) IS NOT NULL
        AND MAX(CASE WHEN O.NumeroOrden = 4 THEN O.TelefonoFormateado END) IS NOT NULL
        THEN
            MAX(CASE WHEN O.NumeroOrden = 1 THEN O.TelefonoFormateado END)
            + ' / ' +
            MAX(CASE WHEN O.NumeroOrden = 2 THEN O.TelefonoFormateado END)
            + ' / ' +
            MAX(CASE WHEN O.NumeroOrden = 3 THEN O.TelefonoFormateado END)
            + ' / ' +
            MAX(CASE WHEN O.NumeroOrden = 4 THEN O.TelefonoFormateado END)

        WHEN MAX(CASE WHEN O.NumeroOrden = 1 THEN O.TelefonoFormateado END) IS NOT NULL
        AND MAX(CASE WHEN O.NumeroOrden = 2 THEN O.TelefonoFormateado END) IS NOT NULL
        AND MAX(CASE WHEN O.NumeroOrden = 3 THEN O.TelefonoFormateado END) IS NOT NULL
        THEN
            MAX(CASE WHEN O.NumeroOrden = 1 THEN O.TelefonoFormateado END)
            + ' / ' +
            MAX(CASE WHEN O.NumeroOrden = 2 THEN O.TelefonoFormateado END)
            + ' / ' +
            MAX(CASE WHEN O.NumeroOrden = 3 THEN O.TelefonoFormateado END)

        WHEN MAX(CASE WHEN O.NumeroOrden = 1 THEN O.TelefonoFormateado END) IS NOT NULL
        AND MAX(CASE WHEN O.NumeroOrden = 2 THEN O.TelefonoFormateado END) IS NOT NULL
        THEN
            MAX(CASE WHEN O.NumeroOrden = 1 THEN O.TelefonoFormateado END)
            + ' / ' +
            MAX(CASE WHEN O.NumeroOrden = 2 THEN O.TelefonoFormateado END)

        WHEN MAX(CASE WHEN O.NumeroOrden = 1 THEN O.TelefonoFormateado END) IS NOT NULL
        THEN
            MAX(CASE WHEN O.NumeroOrden = 1 THEN O.TelefonoFormateado END)

        ELSE NULL
    END AS TelefonosContacto,

    CASE
        WHEN EXISTS
        (
            SELECT 1
            FROM Ordenados O2
            WHERE O2.UserId = B.UserId
        )
        THEN 'Sí'
        ELSE 'No'
    END AS TieneTelefono,

    B.email,
    B.Ocupacion,
    B.Escolaridad,
    B.fechaingreso,

    B.supermanzana,
    B.manzana,
    B.lote,
    B.street,
    B.colony,
    B.postalCode,
    B.cityId,
    B.stateId,
    B.zoneId,
    B.latitude,
    B.longitude

FROM Base B

LEFT JOIN Ordenados O
    ON B.UserId = O.UserId
WHERE
    -- Solo socios cuyo partnerReference contiene exclusivamente números
    B.NumeroSocio IS NOT NULL
    AND B.NumeroSocio <> ''
    AND B.NumeroSocio NOT LIKE '%[^0-9]%'

GROUP BY

    B.UserId,
    B.NumeroSocio,

    B.firstName,
    B.lastNameF,
    B.lastNameM,

    B.birthDate,
    B.Sexo,
    B.Edad,

    B.curp,
    B.rfc,
    B.ine,

    B.UserActivo,

    B.TelefonoOriginal,

    B.email,
    B.Ocupacion,
    B.Escolaridad,
    B.fechaingreso,

    B.supermanzana,
    B.manzana,
    B.lote,
    B.street,
    B.colony,
    B.postalCode,
    B.cityId,
    B.stateId,
    B.zoneId,
    B.latitude,
    B.longitude
	
ORDER BY
    B.supermanzana,
    B.manzana,
    B.lote,
    B.lastNameF,
    B.lastNameM,
    B.firstName;