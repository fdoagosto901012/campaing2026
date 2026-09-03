$(document).ready(function () {


    function actualizarUbicacionReal(latitud, longitud) {
        const nuevaUbicacion = { lat: latitud, lng: longitud };
        marcador.setPosition(nuevaUbicacion);
        map.panTo(nuevaUbicacion);
    }


    // Inicializa el mapa centrado en una ubicación (ejemplo: Ciudad de México)
    

    function clearMarkers() {
        // Quitar cada marcador del mapa
        for (let marker of markers) {
            marker.setMap(null);
        }
        markers = []; // Vaciar el arreglo
    }

    function addNewMarker(position) {
        clearMarkers(); // Remover los existentes

        // Crear nuevo marcador
        const newMarker = new google.maps.Marker({
            position: position,
            map: map
        });

        markers.push(newMarker); // Agregarlo al arreglo
    }

    const dictionary = {
        es: {
            messages: {
                confirmed: function () {
                    return "Your password is not confirmed."
                },
                email: function () {
                    return "Email no valido."
                },
                alpha: function () {
                    return "No se aceptan números, ni caracteres especiales."
                },
                required: function () {
                    return "Campo requerido."
                },
                numeric: function () {
                    return "Debe ser númerico.";
                },
                min: function (target, value) {
                    return "El minímo de carácteres debe ser de " + value + ".";
                },
                max: function (target, value) {
                    return "El máximo de carácteres debe ser de " + value + "."
                },
                tel: function () {
                    return "Teléfono invalido.";
                }
            }
        }
    };
    VeeValidate.Validator.localize('en', dictionary.es);
    Vue.use(VeeValidate);
    var app = new Vue({
        el: '#content',
        data: {
            data: [],
            map: null,
            markers : []
        },
        methods: {
            load: function () {
                var that = this;
                $.ajax({
                    type: "get",
                    context: this,
                    cache: false,
                    url: "/Geolocalization/points/",
                    success: function (data) {
                        console.log(data);
                        // Renderisamos la información
                        that.data = $.parseJSON(data);
                        $.each(that.data, function (index, item) {
                            console.log("Latitud : " + item.Latitud + ", " + item.Longitud);
                            that.addNewMarker({ lat: item.Latitud, lng: item.Longitud });
                            // Marcar con un alert que no se encontro datos y limpiar
                        });
                    },
                    complete: function (response) {
                        console.log("Complete : ");
                        console.log(response);

                    },
                    error: function (response) {
                        console.log("Error : ");
                        console.log(response);
                        // Marcar con un alert que no se encontro datos y limpiar
                    }
                });
            },
            initMap: function () {
                const ubicacion = { lat: 21.17429, lng: -86.84656 }; // Cambia lat/lng si lo deseas
                this.map = new google.maps.Map(document.getElementById("map"), {
                    center: ubicacion,
                    zoom: 12,
                });
            },
            clearMarkers: function () {
                var that = this;
                // Quitar cada marcador del mapa
                for (let marker of that.markers) {
                    marker.setMap(null);
                }
                that.markers = []; // Vaciar el arreglo
            },
            addNewMarker: function (position) {
                var that = this;
                this.clearMarkers(); // Remover los existentes
                // Crear nuevo marcador
                const newMarker = new google.maps.Marker({
                    position: position,
                    map: that.map
                });
                that.markers.push(newMarker); // Agregarlo al arreglo
            }
        },
        filters: {

        },
        mounted: function () {
            var that = this;
            that.initMap();
            that.load();
        }
    })
});