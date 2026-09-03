select * from userGeolocation;
-- Encuentra clientes a menos de 5 kilómetros de un punto dado (ej. una ubicación central)
DECLARE @ubicacionReferencia GEOGRAPHY = GEOGRAPHY::Point(21.1602185761331, -86.8292078114612, 4326);
SELECT TOP 10 *
FROM userGeolocation
WHERE geolocation.STDistance(@ubicacionReferencia) <= (1000 * 500) -- distancia en metros
ORDER BY geolocation.STDistance(@ubicacionReferencia);