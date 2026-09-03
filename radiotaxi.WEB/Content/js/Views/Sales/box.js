$(function () {
    /*
    $(document).bind("contextmenu", function (e) {
        return false;
    });*/
});

$(document).ready(function () {
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
            id_ticket: "",
            car: [],
            _car: [],
            taxi: "",
            reference: "",
            message: 'Hello from Vue!',
            services: {
                id: 0,
                date: null,
            },
            date: '',
            day: null,
            month: null,
            year: null,
            id: 0,
            dateService: null,
            events: [],
            time: '',
            gafet: '',
            data: null,
            selected: 3,
            cargos: [],
            total: 0,
            car_eco: 0,
            loading: false,
            datadetail: null,
            inventarios: null,
            servpaga: 1,
            reportedoble: 0,
            reportedobleUnitario: 0,
            reportedoblepaga: 0,
            payallcheck: true,
            blocks: [1,2,3]
        },
        methods: {
            pay: function (e) {
                console.log("");
                var that = this;
                e.preventDefault();
                Swal.fire({
                    title: "¿Desea continuar con el cobro?",
                    icon: "question",
                    iconHtml: "?",
                    showDenyButton: true,
                    showCancelButton: false,
                    confirmButtonText: "Continuar",
                    denyButtonText: "Cancelar"
                }).then((result) => {
                    /* Read more about isConfirmed, isDenied below */
                    if (result.isConfirmed) {
                        that.loading = true;
                        //Swal.fire("Continuar!", "", "success");
                        // ENVIAR POR AJAX TODO LOS PRESUPUESTADO...
                        var url = "/ticket/json/pay/";
                        // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
                        console.log("Continuar con el pago");
                        var data =
                        {
                            pays: that.cargos,
                            taxi: that.taxi,
                            reference: that.reference,
                            chashcashier: ""
                        };
                        console.log(data);
                        $.ajaxSetup({
                            headers: { 'RequestVerificationToken': csrfToken }
                        });
                        $.ajax({
                            type: "POST",
                            dataType: 'json',
                            url: url,
                            data: { jsonInput: JSON.stringify(data) },
                            context: this,
                            cache: false,
                            success: function (data) {
                                if (data.code == 201) {
                                    that.updateDebs();
                                    Swal.fire({
                                        title: "Pagado",
                                        text: "El cargo fue realizado correctamente.",
                                        icon: "success",
                                        confirmButtonText: 'Continuar'
                                    });
                                    that.printDiv(data);

                                } else if (data.code == 405) {
                                    Swal.fire({
                                        title: '¡Oh, ha ocurrido un error!',
                                        text: 'El cargo no pudo realizarse.',
                                        icon: 'warning',
                                        confirmButtonText: 'Continuar'
                                    });
                                }
                            },
                            error: function (response) {
                                that.order = null;
                                Swal.fire({
                                    title: '¡Oh, No se encontro el servicio!',
                                    text: 'El número ticket es incorrecto, verifique nuevamente.',
                                    icon: 'warning',
                                    confirmButtonText: 'Continuar'
                                });
                            },
                            complete: function (response) {
                                console.log("Complete");
                                that.loading = false;
                            }
                        });
                    } else if (result.isDenied) {
                        Swal.fire("Changes are not saved", "", "info");
                        that.loading = false;
                    }
                });
            },
            printDiv: function () {
                var newWin = window.open('', 'Print-Window');
                newWin.document.open();

                var ticketnumber = "t-00000";
                var date = "20/09/2020";
                var Usuario = "Fernando Agosto Cruz";
                var Taxi = "8624";
                var ECO = "8624";
                var nameSoc = "Fernando Agosto Cruz";
                var row = '<tr><td class="text-center">1</td><td class="text - center">1</td><td>Producto 1</td><td class="text - end">$10.00</td></tr>';
                var reportdate = "20/09/2024";
                var porcentaje = "100%";
                var total = "$150.00";

                var img = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAOEAAADhCAYAAAA+s9J6AAAAAXNSR0IArs4c6QAAIABJREFUeF7sfQecXWWZ9//0c3uZPplJMumFkoRJpS8rNQFBo58iLn6u7Vt311W3uUoR193fimBdFUEURBQFERBIQgiQEHrCpLdJnz73zu33nvaeb5/33EkmLCWNuRjv4RfuzJ1zznvO8z7/93nepwqoHlUKVClQUQoIFR29OniVAlUKoArCKhNUKVBhClRBWOEJqA5fpUAVhFUeqFKgwhSogrDCE1AdvkqBKgirPFClQIUpUAVhhSegOnyVAlUQVnmgSoEKU6AKwgpPQHX4KgWqIKzyQJUCFaZAFYQVnoDq8FUKVEFY5YEqBSpMgSoIKzwB1eGrFKiCsMoDVQpUmAJVEFZ4AqrDVylQBWGVB6oUqDAFqiCs8ARUh69SoArCKg9UKVBhClRBWOEJqA5fpUAVhFUeqFKgwhSogrDCE1AdvkqBKgirPFClQIUpUAVhhSegOnyVAlUQVnmgSoEKU6AKwgpPQHX4KgWqIKzyQJUCFaZAFYQVnoDq8FUKVEFY5YEqBSpMgSoIKzwB1eGrFKiCsMoDVQpUmAJVEFZ4AqrDVylQBWGVB6oUqDAFqiCs8ARUh69SoArCKg9UKVBhClRBWOEJqA5fpUAVhFUeqFKgwhSogrDCE1AdvkqBKgirPFClQIUpUAVhhSegOnyVAlUQVnmgSoEKU6AKwgpPQHX4KgWqIKzyQJUCFaZAFYQVnoDq8FUKVEFY5YEqBSpMgSoIKzwB1eGrFKiCsMoDVQpUmAJVEFZ4AqrDVynwngHhugeW1iX7d8TCAcEfC/kEWbJdJpQcQRBMRRQZmOVCAzRocBXTPWLqXMEFdO8rrQQYgKvQd4AguoLLvJ+9Q+ffDf+miUwQBFegayAq5e/pFw38RpoGPrb3G1xXP3wvZrkaDVvyRi/ROT4meGeWH8eg+wx/o0Er0d8MQBVcflb5VNcRXEE6/Fz8Inqu8jvAKj+bBgjW8Hnevd940LuXn56/hSm6gmYA9CgjP+mPBtTy5SYg0LN799TKD2YooovyOZpJf+H/g6vo3vMPvx2j8w4fh5/s8Fn8Oj5Xhw+DiKaWrzVp/BHH8JSWv+LzdGhEDSq/15H3x8jnGDHvLvN4RhDVw2MQH5hMEHyqUHIF17VlJkmaUXfOiu43Jey79GVFQXjJdDR9ZInvS3PnTLomFBDr/QEmy2JRkdyiqKsuZEWAa40gsggwwYYr2iD6C7AhOi4EQQFzFTARcDmTMAiC5DELsyHCBQQHcCUIBBdXBAS7TFLxEGlFl3k/j+ATt8wfLrzzyrDgP3tzbENmIkRbohmGoTiwZROi60BigMQUCEyBLcoQmAuZ2RDgwqbnASBBGsbam06x69L7jZimkT9DAmNHrkdvdpMjrn/DCaz8Xky04UFApCfiz0mHKcmgdxdcEZJrQ4LBz2NQwAQRrkA0Y/yd+NVE6vIjDT/1SHjStfRO9B2/j0tzJoJof+R5dG8iqUd3YcQYhyeC/laes+MECLNsCLIEya8jkyrCZTpsec51tWev+OVx3vKYL6soCP/r/6Fx4fmNL01oqw8JQimmKBZ8qgtNcsEcC2axBFXW4NKkujKfFCK5K3oTz6ePWZxx6J83wRYkDlAJoiCA2Q79xWMUl+CociQRYzoiQDKOA0ygvxNL0mD0M8388O/ez288OAQFFxIToVkaXMgwZAZHdCC53j1kV+bMagveGDJz+CdnQMEDYRnZbwnCN/5hJKjcd2BCYnjOxEeAd8Qd6XnKh7fAeO+puAp/LoIdBwMIlA7oFAKeIzL+KdPrcJqNYCVa5Ph5Ih93eAHjoOXS0OXvTqCTXfHQlZLrlunmPQNdbxHtRAkOBzy8hcB1ILueRHagwikvuMfK/fx+mg5m5SAaKfhVGXB0JLITHqw9f/0Hj/V+x3v+qIPwMkCb9D6srpmHCcWAJjWOiasTJo7x+f2SILglOE4RtmlAkSQoogrXJmQRI5DaVJ7UMiCIr1xYh1ZeDjZGUlCAKEpcGiqSzEEogCQhMQcxBQGDQCBy1qIpPwQ+MC4JPFAOL+llUB7BrB4zkkQTmAzV9vHntPkCQczles/lemC3RReCSMzjMT2NwpmSj+E9wfF8euB6a2kwDMI3YxAPKvQ8Hl34vQRa1ADR0SDSwkfqBd1fsMA43UUwkUBoeYuKI/JFaPigdyojiIOGA9stL5DDC2WZ64g+Ki2IIyQnLV4kFWmW6FrbJRASGD3NhuaJgCi73vi2oHCA8gWWz9mRnyTX3+z74fNsl8HvZDBJTSImlmBnDVji9Gf8CzZeeLygOtbrRh2EP/kqvjbufe1f/+aqXuwsBTjYVEWCZRcAZkIhQeXQ+itBFGllkrnS5nJdaaRK6Ek3YnACC5c8XILRai3BlVQIBDKb1D+STAQ3T26QfLMFT7LKfB0eZmK6x5sfh5U+j6G8lZrBlvhmEorlg+gSsBgkYi5Sk13AKUtcxpmWQGhzpnTgK0sPEiX0bMcGQre8PXo7VZM/4/BC8iavxZmTtAV+oupJe8Hw1EJXg8gkDjD+OwchvQ+poQxMNLjeKTLtCHV65I6Pg9BTYw5pMR7dvIehkZWyaurQfHDqe3QgEHrz6S0KHsGJ9gTD8mJa3pK8HcjeDoTEF6ptYKKaws1XTsYEoR9B14bjjl8tz1t/3rGC6XjPHzUQ7n547gZmb44Volpsh29S4O/+mESmbh6Y6fBJJtsLMamiCiiVinyKJEHmaihXdUaoLcOqKFdPR6yiBMbhNdkVZE8VOiRpaLPiqVDDaq2nxI64yZsx6qE7euoQ/W947+QKDlzJYxKJSw66G+N7QaX8YEwQYIku3yeS1JEZg0uS3aW9qQxB5AYRDliu9h3Dp/e4h6XQWzHBWwGRw6MsuWlPTYNLXM0j0EgQSM1m0qG9Gr0LV//4XFne8zJlxAIyYjtd1lZcWhS5tPaASHN3CIQuqZdl6VrWDIYXOFqmuDLM1fphdZcAKHMtgj69vT8tgsenCZBaq4OhPr0Jd31oPKa5u6EWM5Dl1jXC/C3nHi+ojvW6UQNh70NThxoac2HTp4rLBnR86RkbXf7TENLDME2TLJawrCIUXYXtmBD4kiqCb6HK+xna63HCk2pHk0SrOGlTggqHrKZ8f0WSj0FhnuriuN7+y5ZISnn7PwkWZLfIzzsE8uHF9g2Sg1TassmBS1Cuzo5Qp0jqeZKDmIJ+prNo7+cxnSmRKspgyp4xiTMVU+CyQFmClPjiM2yoeLtPVxSOOI8D8A0G1aNhgEOgJEl2SEqJXBBKcLiks0UCAYPukDQkwnl7LyZ4qvzwc9KiRucPG1XKcpWDzlsiDmsZHsCGf/eAJTIVBG56Em+fSRoEScKyQa28AJMhiO4mMk934UY0PrZTNg4dzZv/73NMx8aE0jbce00NzlQOwEr0AlLr8+o5+845vjse+1WjAsIHli6VLvvM1qGg2hUyND9eMZrwiT8kkaidC8FmBBVomoKCUYQsixw4/JMYvgwKb5IJNLZnMClbMr09iA4m+MG/EiyItGdwBC7laHPvCIAlkSrlyT4ZDhTkSCn11DBuNS0bA/hebiTs3kiiskpcViZF5gGI7u0xp6eODks0i/YzEoGR8efg0pJbTD11VCJVtGwpfbPpGynF3lT1PAHjoMAYRMlTzxk9lytCZTZXNS3Z4AudZpNUp8VN9ZTE8h6PzqV5ICOUpw6+PfONXCsOOSII0ARCkmpk9S5bm0fSg6Set9h4n0Q/0jQOzRcH4jtbiN+UtpwvRIwvbMCvlvgwvbQVsibDtmrWKAsOnHqSsPu507Ix38GgI8WwZjCGLzxloD96Gjd5e5Y3T3IMq4qHJA4xhyAc2qsJXDR6qyXf7UnEDAqJOg4ARSqBMQZmaxBlDQ7NjwRYDgFchij4UCgUEApJsM0SNFHlZkpyZTDLhGNbgGNDFhyu9IiM2wf5pItlCeDtT4j5bLgogckMNl806FkllPIm/HoIji1w654aDCLvkJjRoKgBMEuCW3KgKjqcsiHk2NdP/hCHLL/Hdz0gyyoy+RJkXwzkptRLedgsg5KSA3OL8DkCJFuAY1kI+HSA6EQWZ5tBUyQYzODLF5+j8n6NFj7aszpkdBEluGQkE1WIkgTQPy5RyeiiwnB9Hh3MLHeayrJnyIKko2QxyJIC5jiQHBuK5BlxLIdxQ42q6oBNi6739oe3H0dHDUd0+eLYVuzAb6/0YZq9A67jwBRb1+hz9596IEy83u7q4l44bhjP90fx98sKGIxOh1w2WAyzlAfCQx6mQ+bt4cVuhB3OW8FVCaWSCc2VPNeGm6HZgCBHwJjMJanAitClPASryH16NHEuK0KCCTefh0+R4dc1+H06/KrEf/epdD8g5NdBJgOSnoR3mTMbLQqkOtlgso2MlUeRWdwIo8tBuLYEmax2tgBXkpG1JexJFLC9NwvH1wDFH+eLBt3H4vr08a3kZRl23L4ybmhxbKhaELajQcpn0MQSmNKqQYuZ0FUG3RTKrnoXhWIOPk2HKotghgVdU4jUnkGHjCbcLyvAJh8gGZ9cAYWiBYcJKFo2SpaNYslEsWSgaFoo2gJsOYSsYUEh4ioSlGAEibwDW4uDSZ7FmWgt2wZcZkFRFIiKghK5g0wLmuhJxjc73gmUFrmoJKCtuAG/W6JzEJK11BLHrPG37zu1QLhq1U3yGcHfWqLQBSYG8FIqii+syGEoPguwy34qbloe3kUMq3yHwThM5MOaB0kdFyXH4Cu0RhNvl+C4OZjMgeX6AUmGDgtO9iAa9TSUbBfGBjSc1tYMv2xgXH0YzQEZfplBV2WoMrk0yLbKQPEptHEnfyUB0LPJlX1uBP+yCmrQcysSJNVzN7Aiqboqd8xrms73oknHj40pFfc8twObC1FktQaUHAkOqX/yiewIPIvwcR98P2WSCRNOBmiyk/hkux9XnBmG35eElR+A31bATAu+sIpsIQ9JCUKUVZiuxemhON4eeDg4YtgN5HDjkwBZUcHIdcMAh+jjgC88ls1gQkRvwUBvxsC+ZB6dCRPbB22kxVpkxFowNc73+hSQENJIkgL5fIbvIeHTYZQsaJL8pqrwsOr+dtZh2uMaoobxxa1cEk53dsBxLFhC8+rgvK2nlnV05+OXaY0N+0qu2wVL8uPlTA0+/8QQhuKzIXLdY9i3NGz+9xjzSPngMflh127ZZ+g6UGm/V8wCVgZhvwhdsmCUcoCRRUA0MH/mOMxo8uG05jDqJROaMYSwbHMHrY4CVO6+KKuYFCnikE/Rc/KrEo3rgY7GH/5HZlkyBAlKALbjwBFsSBQVY6tQ1SDsfAYOMyApQMlfjy7fZNz62GY8l67BgN4MR9IgyTqYU3YJHCeSTgTCZN2VNBuWYcBn+VGf3on/uqIWc2LdCCq9sDIGwr4YrEwBokYaooxi3gETJUi6AIP28JAgl4MfhoHozZHn17MtzxpKfxMp+oVoVDa00R65BAeWFuQ0SgoR7M+r6C4oeHlLNzZ19mMoL0AJNyBvSShCgRKKoMQcFCwDgVAEtmEe0pveMTDhDTTmxiBoGFfcgvuvimCaswMWs2CLY54Lzd10/nFOyTFfdiJzeNSDdT+6xB9r3pVnQjcMMYCX0/X47B+HkIjP4X66N99XD4eQHbauDaus3sAeeP1yENnkAMJ+hrhagt3biSZlCBfOjGH2+DDGxf0IygIkJsEpFaDYWYQFEwHX4oYH0/GYanhPQRZYUZKSoiAMQRBsx7YHuJbF130hTVtXCgRwBDCJia4sKyGnWKTVmclaCKm06EqiDohmwKcZ8xU5I2clPwYCM/Afj2zEs5lGDOqNXJXmai3z9pvHd5DBYsQidow3IXXaFMkdJMKPMOoGt+CHV9dgurYTYf8QN8hkegPbY5GxyURmUK+tiYgwbBmuKUG3BNuy4Eo+wSUYcreow43a9E5w3SC3GYt8duNgNnNdl/6F+J6dG7QMyIoD1zKR0yLICRqkQJjPB0nRgqVge5+JZ7YMYu1BoFeqR9FXB0v1QygbsxyakDdYtN/RmFWmE0XoUHheW2ELfnFNHBPZHliuDVNoeS40d8epBcLeZRcHQqEdOVfqhSFH8XK2CZ97Io9kzRwIzFNrOAgOib7hiMbheEbPx0cMN7xnpElSGIOSL6IhJMLKdKJeyeOquafhvIlh1Nq7ELJ7EVUlFLM2RKUZ+RLFo7rQBAmSIb6QL9hrXV94tyvKuwQBPQxyksnMkZivEA8GKbQYmDlzOMi0DNSbPN+2e9P/cP9N//PEN41YyG5yubW9fPSsPv3bEbXni0URGPTPwK1P7sLKbDP6tRa+vyRm8Yw+x3kcAUDPF+eFjx3dpyO4sCWbG2fMjIOWzBb8aEkYC+q6wawu2IYGXVp4rr/9mTXe+/4PPX67WcAMSNgCB0sBbIaEmf/zM32+xZHM5XgotmSJkiRaSskRJEVwmyEYcamQnhIMq6cn8umr/GGt0TIHEfAxwEiT1QiGWosBsR477CY8vT2DJzYcQIL5oUWiyJkAU4NvGrZ2NFKRthsqK2F8cTPuvroBE909sN0SLLH12eBZnRcc56wc82XHPf/HMtLAH64M1Y7ZknHQjYIYxupUAz6/wkZv9AzI5A8qh3JxL1zZFeHNuO0ZVrg7rKyiCgLfXxAIA1YO8cJ+jNeG8L65NThvehNiBRMhK424mgQzUijlGVR9LLLF5meZXPtgTnafKzrxAzMX3ZU8lnc4nnMPvnjZ16JCx9dFzUG/fzK+89Q+PDzQgF6pGaqiwHUYZPGtNIGjG/FwvOexRdx4HjwBRdNBKBQCK5loTnfgvy/VsTDeC4v1wmZxDGUXzhh//h+3Ht3THP9Z7gNLpe4GqVnBrvcF9cxHJTd5EcUSW44FQQ9j0NQwJNehR2zAH9ftw6qtA7Ai4zFgh2BKQa7qHhFTW946vH1EEVm8LYwtbMPPrmnAFLYLYGTYa3zWf9beUwuEO385PzxpxlDasQ+gIEfwbLIGX1gloidyGo/Q4CFKrueI9gKXyFjjGR1oP0a+AS5iuPomQ2RF+Ow04kYPrpwWw4UzAphUX4SUH4SaE+GDAsey4djSSlGpu7NgRlY2nfcEqZWjeux6+oqvNfk6vi76bXSJY/G9Vfvxh4EmDPrHw69psClI4ZDcPHoJNlLSeVkMx3eQD85xFR5nC8dCY+IV3PP+OM7QdkGU0zDRiLx52cymBXdvOb4Rjv+qrmfPa9XE9KcDcuEzAlJ1kmyiKKtIKbXIas14eZ+BP7xyADvyPmSUOhhyGEzSKdixHCBOaqoX6sY1Fx6SR8H2h+UO+RdJJW8pbcPPr67HFLYTkp2FgKZnfXNPMUnYueIvI+PCu1JMHEBBCeO5ZA2+uMJGX3QmIJK7gACoQ3Zpk2+DORRFYkMk8NkOZE2FIbiwTAd+KYBQqQs1ybX4l6Vz0R4TEBfSsJFAsWRDEJuQLcR/7joTvzf5/HvWHz8bnPiVA2uu+VpQXP91ppvo1yfg9uU78WiiEUOhcRAtCzJF8VC8KR/q2CXZiabxkD+PAqCJUcnH2pB8DT9bEkJ76CCYMwQLjbCES2fUnnXnuy4J34raB578ZFzTd10ri9u+5w8XYUkCt3wzoQHdZggPbx3E/a8cQKFhNnJiLWC43I3hUDaL48W2cleMaPBIK8Um+wD97gGTon3GFjfj59fEMdXpBMgVIrQ865+7/dSShATC1siuFIQB5NUonkvG8OXlJfRFzwCTyTkOKI6fg1B0TTDX4pH6ZEyjVKZCKQ9BpjjGEtR0PxY1u/jSFS1oLHai0S0iOzAIPdqAAuIPJostN04+59HNJw6hE79DYvWVN/rkjpsc3cSgNgm3Le/Eo8l6pIIeCCWJgsS8uNhKHDyaiGcZUDgYOAjvujKE9uBBuGUQ2uL5M2vm3DfqkvCN9OhetaRW8+/9iigM/gPZSQOCDFOPoVOIYU2viDufO4BBcQwEfxPfK5bcEhRy5ju0HaWQOKMcTuh5ob1cSNoKOByEv/hAGFOcTjCbzEstzwbnbj21QJh4/LJwqHbrEIQBcSQIeyMz4SqUCuTyfDyZh16YfJUyRBWM9HxJhJnLIq5L8CW24P1TBVy3qBljxQG4qX3chQChAZn0pCUtFy5/rBLM/FZj9jx/5U1B5dUbHY1AOBm3L9uDR5LNGAq1AWVJyK2jx+urP8GX5UauchgauRQaEmUQhvbDYSnYqEcR581smvPbioNw+FX3v3zNeQF593c1tn+WYWUghMYgKdVj/aCIO57aio1mM8S6qcjkc/AHQjBMkvKSF0oIB65owOFB/F5qHIUdji+8EYTjngueai6KMgiTEAakN4JQoKxQAqHtEYoH5EoCDFEBxV0yy0FMMuHv3YEPtcfwfxdpCKa2IWpbkEQfcm74qXyx5bNjz3668wR58qRf3vXiFTcHpNducDWbg/C2ZfvwSLIRqeAEMArDEgVI5ZSnkz74Ud7Q21OSbqagLtmBu5YEcVb4ABgbhCnUwnAveE+BkF6rf9PSICvsuSUkDX5BkWwUICCJIHawWnzzyU7sMuOQAvVIFwUIWohH3VBAP8UdO5IBRtscV/O8mS4bAcI9ZUnYemqCMFy3bchF/xGSsC98Oo+C4MHYh4KmabVyYdGm2XW5ryqe2oNPztBw7dwwFKMDdT4Gc0hGvth0V6o464uTL78vc5Q8N6qndb285Ga/sI6DcECbgtuW7cGjyUakg+N4JI5KWRHOiVlHT/iFuOtHBpiOusRG3HlluAzCfphiDQwses+BcPidu59Zel1Y2niPT0/CEIBsqBGvWbX49sPr0OO2ISU1oCQFeGQyBfbzvNIyq/EKC7T8CGVJeE0UU9iwOnrqgjDlol8YKQkHQmdA4jVEKNnW4f9RpCBtoKlWjGJlEMkmcNXUKP5+bgiR/BYEAmlkczZsc/IP9vrP/mJ7+x0jsj5PmCVP6g16XvzAzT7x1Rvs8p6QDDOPJQiEpI46UHkKQ4V0Uc6YZcvqIRBu5iBsJ3XU7YMtxFCUFp3WNOvB98Qe+80mJ7128TzN2f6wpueahmwDhbo2rBvy4buPdKJLHIeEXosShfuIJZ5dI5F13SWrsJd6JnHDzEb8YiQI0bI6OG/zqRW2RoaZsdFOAuERhhkCoVIOebJEymj2IvG5S9ApoK64F1dF0vjCpdOhpjahLiqgmB5CSWj9QXzRrr89qYh5F27Wt/a6m33iCzfY/jwG9LH47rJdHIQZ/yQKFoDMI288C11FDr438gpgwfWhLrEBdy2JcBC6bg8soQYWO3tBbfsfXqrI8x3loH2rPzrBL3U8oiiJmUwGkmItVncF8MOVe7AnPAMpLQpZNHjtIQIhqd5wVC/SRrQxtrQBv7gmclgS/rmBUKUNs0CSkKrFkBVLgwoTAbMPrelNuPujs9FU2opgxEE+W4RhNf5mj/8vrnsvS8Bh3hlYe93NmrT2BkfPIaGNx3eX78Bjgy2HQEjWUVp8KgdCqlxnedZZ5kddciN+emUYc4MHAdYHx60ldfQ9D0Kid+9LS9tUd9PzQXQ3Cb4A+qUJeLTTxfdeGUSfrwmCoEOgYG/yNxMQGRUQo6BW43+BkAktawJzN59aWRTJFUsjoeirKeb2oajHuYvii08WkIiezl0SZKCQLRl5S4ATroVu9GNcah3+5ZLJuCCeRMTaj6KbgaONfXKgtOjDkxe8N/eAb1y4e1648qag+NqNrmYhoU7Ed5ftxmOJMUgHJsC1HM9FUVFJaANiEYwCJRBCPNGBny6OYX60D6KbRMmMwXYvmFc7775XjlIoVfS0A68snVfrrnnJMXsg10xBp9OIuzpSeLgjBSF6JtK2Clcuceu7KIW431kRHYwvHpaEFLxFIPS3b/rzAOFA7HSIQpFbCHWbSlSoKKkatKFt+PJcDVdPABrMvVDdIvJ6xDiYbph8+vmvHKjoTB/D4D0vXnFDUFx/M1QbQ/J03L6M/ITNSIbGgjkmJJlW5VGJHHzzp+ZZ/YYXGigEEE9s5CCcFx2AyNIomXFkjIXt48791WvH8NrveCrFoXauOzBBQ8GvqgqypSzzS75sNwsOtLffUXjHG7zNCX0vnH5tXNv/y6LFYEVbsdGM4UfL92Ll3ji0ptPhaiYyRh4SJVfbVGiKYXxx0yF1tAzC5/3tm06t8hZvJQkJhKSTC44N3ZFBRbOLRhKnBRP4/pWtmGTvhJzuBvQm7HbOvGLq3MceP5EJGu1r979w+VdjYsctogIk5encWf9wqh7JcDNsZHjEjGhT7qFnJBj1g9dVtcoZ8D7EBzfijiUxzI0OQnTSMOwalJwF7c0LfnFSQdj96qW/VKwD1ypummdDUO1XKjSlRk/bONQ/b17bhTd5wfPHefS92PaLkC/18QKliUXHYfleFTc9kUMichpsXmaE/rl8Ty45AsbzLApvT8hBiLEv+OduWHScwx/zZaOyDL8lCKNncue8RGUjHEA1k6jDAXzmgnG4ummIuybIgZ/FlP+ILnj9K8f8dhW+oPulq/8t7K77hqAwJNSpHgjTNRiMNMF2c5AlEYrpqxwIuXXQ5QHxgqigJrkRP1kcRXu0FxJLw3ZiSJdmzxt79kMnTR3d/fxHx9UENu5Vi/uh+ynDl1oNKDBNhpw7Dmlz/vwJi+55+USmrmfd0jrFeuZAJJDXCkWGoeiZ+PZrGu5/LQWxpgUFCpAQGRRunaYaM9s8EDp7yiBsedE/b8PCE3mGY7m2oiBMEAiZw10SVK3Fl96KT8w08OnzmhBO7ETQplyz+o3p7MKFjZfcmz+WF3svnNu78sqvRnybbyEXBcWO3rZiOx4ZqkMy3Or5CSnRlSRhhcLWiEaUbEv5fVQLpmZoPX6yJIS54S7IbgLMCcBwz7wyftaaR08WPZMdH1jCcqseqfHZQDEDXsLAccE0GV3ZMJTaxfObZp4YCOlZe9fM/IeQsvM2qpQwpLfgJWcybvndeqRjp6G7QCW1OITrAAAgAElEQVRMytZRV8DYIoEwxsPWSBK6YutLvvaOBSfrnd/pPpUFYWQWFKpS5pTgunmMd3fj1ssjmKV18/qPitCAhDF9cdPZy/74Ti/yXvx7fs1f3K2I264v+V306ZNw64odeHSoFplAGyST0pio9OGoTMFbkoeshTZJBklGXXI97lgcxrzgAagYpI4UyBWaTVmf9KuBrLrcdGMrplwwNSkIXk7l8RypjrN/HZS3f9gZSkJVqLizCygyGSthKa3oHpoxf8Kih09IEtJzua9+2l+yl6/X5dSUjAv0Rqbi1x053PVKEUZsBg+ap9pblAI1tkjq6AgQSq2v+M7qmHc873c814wKB7yldTQyi++LFCMDX6kLH54dxGdOLyKW2IiQpKFkTX7Sd07HZcfzYpW6hkp5BALd5zcFrI/bhZ5rJTWPjC+Mbt9MfHtFJx4frEfB1watKHJnvSGZfI9SkYPGpegkx+FV6OqTm/DTK+KY5z8An9DDH8m2wnDlIEwqDyiKKJVKT6tK7aMC6p4qCaHdze2PHrUhpbfjqvkwX3xRFwegww9ViaBz3wBa28aiaHTDtOPQA5fNi5z205Oi/g6uvuS6uNBxj6CmkNLD2O5OwBd+3YW+wBnIUK1aapcgDBtmRoBQaHnVN3fD3NGak1EBYeeKpZHW6LqU4/bC1GLcRfGFZSX0x87g4WoxK4Hm9Ebcev3ZmGq+hlrkADuOnsLU85ovWLZ6tIhxIuNsX7t0TEzZ91eCc/DDEa14hphLQ/LJcB0TaV8tuvWZuG35HjwxWI+8bwIHIe0JLQoopg1xJY4yCB0OQh315Ce8vJaDUMMg1aZAQWlE1mQQZQM+1YLMCiimC4j5m5HL+Ddb9pjHiyz++2LQv/ntXEe9z19cb8tbnowE87NtMJRYAzZv3c/Lm8yY0sLHE4V65HLt85rOPjkuEffVTyuMPfeaaG073dZkJILT8aP1fty3Po9CrA05UeU9LXgA9zVRTHV3A5YAJra+prd3tI/WlIwSCP8yMia+PwX0wWJBrM7U4/89VUB3fCbIchga2IzPTzTwqfYY6sydYJaMgjznt7G5T39otAhxvOP0rb1ikWMe/EJYySxVpDwvnEsWR5XCg0Uqa2wjJUcwoE/Hd5bvxiODtcgHJ0GgytblHg8nmhd4vM/u7QkplYJx41h9Yj1+fEUM7ZE+iMgjrYSx7KAC21+L01tqELaHEDEHELSykEsmFMkHRmUdZR05R4QrBf6osMDvinm2NuM6fcyqL0Xrc3EzN3BBSEveogesiVmjgFJ4LHbb0/CjOx7FR86PY+GkGAL2EBwriqywcF7TSfRLpl+cfV1Y3XIPlbksBMZinTUBNzywHju0qcjodVDhojW7Dvd/uBETzV0QHQlMHL9ObV9/1onQ9ViuHRUQ7nz8svCY5t1psG64LII1qTp8elURPbHpcO08xhe3465LGzFX64JS6oartmBvcdGFk8/99TPH8jKjda57002ivaTj4lJm0xcgDVziC4mQ5CAypgtTD4GJCtIDAwhIDOEAQ1FQMaS34fvLduKRgVqkQ5Ph8orXXql+KmRcmYNCBEVedEkWGd8T/viyOOaFkxDdAvr1GvzTI9uxLS1ibCSOKfVhLJgYxcSYgggsRGQLrjHEHd5k3IFpQnFM6DLVfZVKpm0fUP36+EKpqJDaZ4g+5P1jcEAai289vhv93V34/DlhXNSmImwSCONIiXPnNc37zUlRR4mm3a8u8UfcV/sUczBYonmITMNdryZxz64ACjVTYRYKmGBQZn0Ek6ydEBwGU5i0PjJ/45zRmpNRBaHodIGxMJ7P1OIzK6n47zQEnBzm6n349vviaDN3QnKyGMjUdNQlFp4lfOi3FdLT3pz85NPuWX3mxRFf5kYn370wFFZ4bdOioyEhNWJQacK6AQcvdmxDYs8BfOyyWWhvoaDhAopKM3745Bb8YaAG6dBUMB5ATECsFAC9/n+Us+lQkxpZRP3gRtx5aSMWBRLQnDQG9Ci+/OQWvJQOwpLbeCMWn5BBQMxjSm0Us8bXYnZbEA1iHg2KA5+Vgl3shV+0oLsmisUibCgIxhuRtYABaQxez4/Bfz+1HdvMIJTcQdxyvo7LxznvGghpJntfOP9va5Sd35PlIrJiLdZm6vDph/ajPzoTquLHJLYLdy4OYBq2A8jDwIzXY2e+PvuUA2Fz0/a05HTxCtwvZBvx2acKSIUmwtffiX9dMhNX1e5FbakTsqQiZU36Qs2CV747WkQ4mnG6Vpw3WxV23qhoiavCceqJJyBVEgFfC/ZmNazZXcTq3RlsTpF65oOe68ffXz0PFzUOQrdTyKuN+OHy7XikrywJuYP+cLuUo3mGk30OB6Egw6LIXdlFbXIDfnJZI871JRA0c8gpAawsaHhoyxA69uvoMVSIsSCoVJxo2dDtDMJWD8YFHcwZE8f0MVE01SiI+4CA6ECj2FiHitsr2NqVxkv7LDy2zUaP1ARW0wot0YF/PxtYPNZ4V0F4YO0n41H52b6g2i8bpoqB+Gz81e/2YbMwFgYLoKW4E/deXYMp2A4mDKGEMzrqZr0262TT+63uN2qSsKV5U1q0D8JGDC9lGvGZFRSKNgbTnV7c9MHZmGK8jHolg4ypIpmf3tR24TO9o0WEtxtn75PnNtUE+m/UfMXPQMmDuQYKgsKzuQflMXh4XT86ehj2DlFP+DgKkBCUbOgDO/HVD87GX0Q74XPSSKgT8J0Vu3i1tUxwIm/z7XWQGa4xU4m3pTK9VKPcBFMLiKVfw/cX1+JcXxrxvNdUtRAJoMfxY9uAhsde78XzAy76pDjcYB0sx0ZIMcGKGYhmCaptoj6kI+Kn4lECaqJBMLOA/b19SNg6SlIElhQF1BgGUgW0id34ygILl4+3EDKSXB0dEubNbZ7/61dPNjUKHQvvkY1XrlMcGUPR6fjmOobfbTdgBccjOtSJ3ywdh8kOlcFPoiTO7qhpX33qgbC1aUNaZD0w3TheyDbhs8sKMJU4rogm8aWL2zBJ24dSqhcZNuahhgXbPnCyJ+FY7/fAA0ul+cqeT45tHvw+nB41K6mwtBBYoBa7UiJePGji9y/vQ7/cioRQA1uNQtACKBpFxMU8okO78K9LpuGy2Db47BR6fTPw7RV78fBAPXLBiVCoyjflElYchFTk3wJTi4hwENbgXP+QB0LHhKtaSFkC5MA49IuNWDso4ZGN3aAU7ZQYRUHywxV13uiV2ptTahr1lSwUi9A0gOWTiNXEkXF0ZA2vvGI+n0fEH4Xa/SK+fqF6CIS2GUNKnP+ugLB7zfxzYvKG1bosIyPX4mVpBr7621fQH5gCf7oLD354AiY7nbCdLAzpzI7wnKdPLRBSPmFL3Y6Uy3phoB4vZJrxmSeycCUd3zhXw1/UJlGn9sM0ZaTE2de1nrXil8cKmpN5/vZH/2LMuOj+H2j+3PttswhTVlHS6jGkNOPxbQU8tq4L+6wAnGAcJWoaqgRQcn2wqaWz4CCMHILpvfjHiydgcXwbl4Q9vjNw61P78Eh/HXLhNmiWyeubVBSEvFMv4+qoI0te2NrltTjX34+AOcS7JFmaCIMKLRsa76Ik19Rzybh2Xx4PrU+iw5yALkNDbZT6BwK5EjVs8fP2diqV2Tep4Y6IkiVADYbQV8hB0TUemdKQ24obFwGXjzW5JOQgFBa2Ny84uQHj9FzuqgvkjL5zo4bitJyoIBOfiv94ZAOeTNUjjDx+eU0jprLdPI7ZUuZ0hGc/e+qBsLVhT8px+lESavBCqgl/+/gQLy3wo8uDuLAuBSPXDcNtQkK4cOykeXdXLFNi77Kzr22tGfyFXeqUXF1CQRuDIaEOu9Iq7l25Ba8PhWFTISEEUbAtyKIJVdeQKTDeAsznlyHmBxDKHMC/XTUDV4Q3ImCn0eUnEB7AwwN1yAYnQKWW4MwuN0M9mUvI0d+LdqRktbSoE5GiIZ7chDsvrcE5wW74nAGeed/v+D/LxHA4xtT/UqwCRDWPIlwUw43Yh1b8ZoOAJzf0wJYl2L4I+gwfBF+N1yrcKMEPaklO3bJsGNS/IxxA0bF5N6i6zEb8xzkyLh9rv+sgJKoMvjD3FtXt/qoekpBkCp7qknDjcznEQzp+ckUEU919PIzGlGZ1xGY/c6qBcGmkMbY+pWgZFNwwXkxE8M+P96Mu6sedV8fRkt/ES1xkrMkb4ovWn3n0bHTyzuxYdnGgRev8ZjxY+jsmFFAUROT8DdhuNeEP6/uwYksaA4hBjNTBlvywHdr3iNSJhu9/TEfm+YEUkB50cpAGOvH1D8/CFf7X4beH0OWfiW8/1Y3f9TcgHZ4CkVGWN3VxoOLHlTECk5+SslcIhEVdRCRFYWtRLAz0wMcSsOwQ+u3T2toWPrN361NX1wTswaXhQNftojKkB+p09GZt2HIbNiUU3L16F7YU/OjzNUOJtCCfonZ0PoguNQKl+GALlshgUIlF6mAlWqhNduAbiyQsbvVAyF0U75IkJE7pefkDM1VrwyZFGoSqK9hiNeKvH+jmrdzu/8RMjCnt4vVuTeG01+tnrzy1rKMUMdMU35Bi9kEwJYTN2RD+7eGdOGNaG/5pHtDEemDbIjLsrJ/WLFz96ZMHraO70/ZHF48ZW7P15zoO/qUhOkhLIeRrZ2DtQYb7nu/EbiOGfupp4q/1+jYYNlenFIl6DFLbbon6ikKUqP0zdThKIZzrwt9fPAEfqN8FvzOEPt8MfGvFPjw00IRMeBJE6k/Ijf5WxdwUkitCc1TYjMHUgFjqNfx4SRjzQt1Q3QR1rEWvsXDS5LMfPlTJbmDd/2k2Ctv+Xdf6r5elLMJyEINOCHvENtz36kEs35NFQqqHHh2PTI5B5qUFyZND9TxJ6lL1BBsOiqjPbcTXF6hY3Mq4dfTdVEe5SvrAUik3YfcOTdg5QZYcHBSb8J+rLWzevg/f+dBpmOKjItImDOmM1+tnrTi1QLhn1fujtb6OoWB4CGYug/15GQ90mFjQPgWLwv3QSyk4CCBZmvuJ+vOe+fnRQefknLVr2ZyzWmOpF1TkFds1UApGcEBrxR3rDfx2XTdYqBEmGR8EBdQukzkUdM1LxHFTPfUYdCnsx6ZfqVV2CQEziZpSD/76nGZ8cGw/dCeFQX2qV4G7vwa5UBtk2+W3sKms43BruJPzSsdwF7KOqrz4MpVvrUu+hh8tCaI9sh+SQCCMIlc478yG8x7Z8Mabbl192ZIxkd6fu8W9cU2X4ChNOGBH8OSuAu5+aT/2B6ejqNZAdR0emka1XXibeYrQkYjVM2gobMXX5vtw2VgcclEMgfIXT/6ecPj5jY4FN9q5F29SfQKKoWas2M3QfWAQ184ah5A1BOgCitKZG8JnrBw1jWxUXBT7Vn80VhfYktTYbp5JX2ISkr5m3r46ZnWD5XKQ1EYMFOfMbT7n8ZNunn4rrux8+tKLxtfu+qOZ36cZCIPFx2FrTsOPntmKZzJ1SAfaAFnjxYkVuwSRUYRJgLd7tqmFFolDyoTg2RAiFAoIdguIshTCye34+0sm4dKmAehOFoP6ZHx32U48OhBDPtgGhZL4KJKfpCh1vKnAMewnpNegOtUUtkZdmdojBwFxALYdR6Fw7qyGc3/f8WaPd2Dt0rgi7f+Bhr6PaCwPWwvwEv/LD9r49ye2IRWaAEWLwIUfriXxTHZJBu/ZaLIianObccNCcta7h10U7zIIUy+ec5ZP3vyqqDEkDBNWqAF2wcR4SYI5lIDtE1EQ56yva3/u1IqY2bf6czFN6EiGlG4oxgCUaAjpIoNNPchZHgF/FNm036jtOjMwWlEynasuuNav7/hlWMzBklSUaqZhdbeI7z+zD7udKAxfHKbgg2MKvDy/Tvs3QYRJoOHtakSIErXiLkGUFJSomCxZR6lKnJtE8ODL+Pq18zA/fBAKKyChT8btK/bi9/0NyIYnQKI+7LzWamUjZhyR9mw2ZNFFXaIDd1zegPZwLyAmeFZD0Vk0u2Xer15/uzVi55oP3Nzi23aDbe6D49cxoNXhxcEw/vvJndivTEFRHQNR8FwhLgyAjDi2iprUJtxyLnDFOM86yv2E7zIId+68TKtJHXhZlEpnyDoV2SrAKeURMEUoahAF5iDjnvZAU/vTHx6tdXFUJCG9zM5VH762OZ77G9nokwuZfjdU14ASlbvSfEikMOCTWr5fP+/+J0fjxfet+NCnFHnNHfVjXB4ckNXH4PGtafx87UHscMfDCLXCp8uwDRu6GqD+zjymkA6bCjPxmiwCZImMKyW4kooC84EJElwnjzq7F62Dr+KbH1+EGepeCK7hqaMr9uKhgUZkQxMhO9Z7AoQ8bM11IIk2r8D908vq0B4aBIQ8DCeGIeOMOZMXPfiOjXX2Pfu+z9T59/0Y7CAsTUEqMB5P7xPww9U5dKEZjhYFUxRYNjVUVeC6PtSkNuOW86wjQfguOetH8tWBFz9yBszu28MhFisaA068Jg5ziOpd6XJBEPOmOu1vWs/4wcbR4EUaY9RAOPxCh5tret+MbKo5Gi+965lFnwrLe+6oC1pIGAJ6w9Oxcp+Fn63cjmK4DVm5Dq4aRS6dha76YNtUjJFKc8oQqMEoST/XgkPde+wSdFWBxVQUxQjvMCuyPGrzu3FRsAdfunQSxri7eHu3AX0abnuKQNiAQnA8NKfEraM2d9ZXRh3l9KbxmQ1RYqhPvoqfXhFGezDJU8yoxoyln9Ved8bR7dF61lz7V/Xaxp+b5m5ItVEcEOO4d10RK/cCncU4bK2ZG7BoTCp1Gkt14KZzxFGJmHkz3iqvpRWsvlzGwGgw/ntljH3Pv+9jAWXdvQE9j0LRh0J0Ou7dWMJ9Lx0Eq5uKwaIKKAEeXC24MlRVhUE9BFUZNrURo5ZtzIAiUC97xktzkDC3HMoLDEKSBPhFA7GhTfj0dOAjZ0UQNHfDhISENs2ThIMNyIfaoFsl3geBWpNVak/oLYIeCEmT5iBc7Mfc4CAE10XJrkPKmNk+7tyHj7rQU8/KJdfHA/vutrAHZsiHVHg6fvzUbjy2W0VGmwDLDYKJMqgNVDD5Kr5+gYrLx1HYWgrMjGOILTxp+YTvFb57p+cYdUn4Tg/0bv1998pzlsb1nQ9EggWkHBFd+jQ8sdvBvWsHMKS1wJICYNRrnlQl3jyTGkhSeCdVqfbacnLB4bo87JrnArrDWQg2VJHacBuQjSFMsvfihx+cjMbSdki+PIqCHxllCr6zfC9+T7GjoQlQbYe3I2NUd/fdeumjuK/Ey1swLulrkutw55IAFoT6ITpUaDkOAwvn1h1jLOfWpz7ypabIS7fK4kGYLIBs7Ex85Y/78VyfDhZq4ylNhUIBY9lu/NsCUkdthEtpMLMWGXf+vNqTlNR7FK//njjlzwKEO56+an5reNsLutwv5A0TRv3p+PkGCz99bh9y4akoyDGI1AWDh53RHom6ApPlnqyfNE+HYcI7Cbsi7/jK1UjR8xVapTTqNRN6cis+c24r/s84AzXOARhSCTkxjIwyiRf/fShBAdyTIJNLg5qTUPXxSpW34NXWvPbjjMLWEutw5+II5kd6IDtZmGYcJZx9zCAkivW9dPbP4+rOv5JRQsKNYlu4HV/51UvYK45HWqzhWkag/xXccr6Iy8cxRKogfE8sCO/KQxx88bKWiLh9rR8Dra6qo19qxtP7Zdz+zAAOyGOAUJyrhBxrvOqZty45IElIBpjDIDnUzruckOul5NJl1D2WIZDagatbUvjbiyaghXVBMwZ5ab2cGEBSb8Pty3fh99wwM/WUB2H/qqVB2b/zRVXqmmlKDnL6GKzoUvFvTw6CtbSjmM+jIbsNN5wn4YqxDiKlFGwrjhxbWJWE7woSKnTTTauWBmukNb9rjGUvKWUt5HxjsTbbhNsf344D6iQU/M2UxMOlHsU6kj9w5MElFJdShyUhByKBsHwiddqlomGuZaCxsB23LmnCvEgSSnEQopWHIsnIiSEM+Ftx+1M78XB/E7KhKZBtal5J+Xyew6NSxztJwpy9aF7T2ceX6b7/pSvbg9KmV6KBAgZTWeQazsJXn8nijzsdSKEmRHO7cMM5ogdCI8n3hFlx3nFJ3krR72SMe0qro32rzv5BXWzP39hsCBkrjO7Aafjcb7ZjvzIeNvWtEzQ4kgJqoEyxnLTf48YK+sZDJS8RTxJx5CFQ9yiX9okibAiIaA7cvu24flEj/nqmhfrCNoilAi/kBKYiK4XRH2jFbSspqbeOg/BQvdFTGIREs65npv9jc7jvv8j406/U4RVMwL/e9yKS8XnQ8v24caHDXRSRUhKuGa2C8GSg+r1yj91/PO9DLbGe3whKP4q+ALqVKbj1iU6eSpQPjoVG+d6SDNMlEEq8USk5zwmA3BVfBh4rS0r+XiQyywrrIRCKQMgcwDRnD77xsfkYV+xAmKpLSz4eTUM+xrwURm+wFbc9tROP9Nf8WYGwt+O6QDj97HpFTE82fD70BMbg/s15fPcFB2Fdww2LGK4YayNiDIJZYeSEBVVJ+F4B0Yk8x4G1Hx/TJK/fCntfyPZp6JSaccc2DQ+8nkXRNxGyLwoVOZhGHo4UegMISbYNq4ckBQW+7/Nq9B7255Fa6iHSQU12J754loKrZ+oQC3uhGTkEWeg7ENx5EIxFOTmAvsAk3LayE38YiCEbmgTJVkAS1bO7nprq6PAcDq299OqotPchKGne8GcDa8LnH+pHOl/EVy8I49JxDCFzEDAJhGfNrZv/4KiFLp4In52sa09JdbR35az76nz7PkptqhP6GDyTjuOflw9i0DcdshsEbBsyMtw3RiopSUKSguR6EEjB5ACjnoleNAETyABD+7dh440HR9k1QDGTk5zduPujkxFLrIeqkg8xgIEup6026P+WrA58MC8p6A1OxW0r9vJCT9nwREg2qbR0l8oBkF7v3dwTDjOpu+om2fY98bxo75pniSYS0Un44WYNz728AX9zXh0uatMQMvu5JCy489vrFvz2qP2SJwsIlbzPKQfCnpULlzb69z3A3CFkAnFsU6fhhodex8tOG6zARGg5ET5VhuEM8T4IjEI3ylXPPBAy7qgmCUiHxURIioqiZUFVdN7MlPaJ1EsiphhQB7biU4sacW1bDrWlvfD5/DjYY97ceknqpv1Pn/Wr5uDujxiShX7/TNz61H4eMUN+QplqfVJaQQX7UBwNCA3hvJPSn3Dw6bOvqQl1Peg4OQz54tjgtuK/716J6/+yDrObdcRZCqrSiP7i6Wc1L3xoXSVBMdpjn1Ig3LP++mg0++zGqJ5ooRqTfYEJ+M7LOTy218WANhaWGIGvxODXZeScIqjqoMA8I4tX94xAeEjP5NKP8t9EmWIeKfqfgp3JWOOA8nkjZh8mlLbhxg/Oxix5D5Shg5CFKPYPSS1Tlxzs2rn6kvta9Y6P2mIOA36qMbMfD/c3Ix2ayAsE0z60ks1gRhOEFK6YWX3vbp+vMC5Nodz10/Gz+57F3Gka5k9thb/UD8uIIq8smlM/99fvGKs62kB5N8c7pUDYvWbOv9dqfV9RdAm9pTBeztTghpW96NYnQNRCoHLvCgy4kosiuSMkDSIHIbndjwShZ6KRYfNsCY3XTKFMesex+F9UVYI+uAkfHJPF585rRa2xGzWqjqFE9L74hR0fo0l7/ekl90wIb7rOFdNI+ibxiJmH+1t4qUcIlmcAOuRzfDen+a3v/U7q6MmShPQEmY6Lvwx707cktYASwth3cACDiSLOnTsRGOqCKDZj0J43Z8yiKggrww0nOOqBZ983OSJt2h7wlYSkG8BgsB3feHAj1ppjkdabIFO/BckC5BJKFHwNDTIl6/LsiMMg9HwPI/aBUHj+oHcwuMzijgmNQNj9Cr61eAwuqMsiUOqD5ERwsKf17Envf24tnf3yiiv+e3J8x+dcIYO0bzy+t3wPft/nSUJW7j/h+R1P8OVP4PLRBGHnmsVjw8GN+6L+DHKDBnyBOHYcPIjxY5ug5hNQlHFIsIVz6ufeU5WEJzCnFbu096nTf1If7vm0IxbQ52/Drzvr8ONV3bDrZiDLZAiWCUkXURRt2MxBmGJFTcaNLUc65XmDCL5PdKlEkUAZ4dTTnVRVKlNYgixSLqCFxuJ2/OLj09Ga3oSoaKKQi+/a0zt79mkf+m2OCLH9uatvqw9s+gfKWcvoBMJO/H6gHunQhEN0qiQAj0oddc6dX7voVyfcqoybulwI2e1nPe3kt1wQ1aIwSjZsCo63CogINorFOhjqpXNic39cBWHFkHScA297eunUKeEt2wR2AP0Q0B89E39z/3506TOQtVXwqBbJ4ekzKZuslyoioo5iNgdBU8A4Eg5Hx3ieA27/5D36yBBDRWW4cuoUoUo2HDOLRQ0GbrnAjwmFnVBLFBVSf1PdFftvHn6NDU9c9O1x9bu/aIsFZNVp+O6KXXhoMMYzziU2HDHz3raOGicRhESXntcXfUIxt/4sIuuwLRO2JkBwSggwB4bRgF5nwZzx51fV0eOEQuUu63v6nDtqlM2fEsMC9igtuH9jEfdu1tEntXDLpis6kDUGy3FhGRo0KQDKlTBLRUAlaUcALIePDQdTl0FIqimXi8zmdTRFOwdVsWDlh3D9BePwVy3dmFDcC6HE0NNVM735g3u3DVNi6/JLvz02voGDMK3O4sV/fzcURio0Hj4KW3vPZFG8dQD3yQYhFYpSsanLNRPQNBd5M4ugJkEzinDcNgwJ75taN/+HOyrHTaM/8p+8YWb/8iubW+J7DgqlnUI2GMGWyEL8yy9fxj5xMrJCDW/FbbESmGzAYSJUNwYFKmyrCEWhWmde0i4/KGCbGndT9jwoIJSA4vJuwnAtyILLM8NDchF6ahdu/MgCnO/bjtjQfqistrB9oLVx2lXPZ4dvt/3JS24dV9PxJQLhoD4bt63YhweGosiExsNvMchsuNDT6E/88IhvuicM90JmGUNEtrUAACAASURBVJ5FcbJBSOPuf+HKz0XDzt/aVo7JmmybhSILSRIbSgcebjz/yW9UjhqVGflPHoSDz571z3Hfnv8UUESfrw0/G5iM76/cAynQjBLtBUWRg4oacZLTXXR0UKk/0bV4bCjfE3LAecYXUTC4ampR4qkrQmGgSpm8HiVVRsuLKmrMbixyN+Oma07DeHsHlIKDvsHmn7Uu3vjJkdO467GLbh1Xu+NLhpxHX2Aqvs0z65uQoaReXt6CFoBKVlt7C2f9IRDWwnDOPml7wpG08Sos3OgKPEj3z/v4kwbhnlU36fW++7eqwp7xLhTs95+JTy3L48V0DJFAyAvFJpWPcgNFkm4k2RSekCvAA6G3GyTAabwEoSAUOWiHQagSCF0brujygsAFNYiafCeuDW7FP7+vBTFzP1jJj57smde3XbL8FyPZ6eDyy25tDG35UknJozc4Ebeu3IOH+1qQCU2E7hjcT+h5KCtX3uLtJeG7B8I/b9gd+fZ/0iDcv/L9F9cEX18miv1gSh1eLTThYw/sRaZuNmRBhszrp3ipShbncw+E3DnP/XRUbYuMMARCL6eQwMlBKFDrMkClBF/b4sVri1Qk1xdGNL0VX56axifO9CNidsEs+nEgcfqsqUuWHVEacOdjF9w6vnYPB2FfoAzC/mbelUmnzHrum6xse7QqCCu/HPxJgzD92kV3K/bG60lnLATacNfaAdzR6Uc6Ph1m0YRKWe+k8pVBSKqfwLRDYCMVk1tFyyD01FGLuyxsgerMCFCpg5llQlEFFB0Ljh5ENLkJt57vwyVNefiNbghuI7YmFwTPvOTe/MgpPbDi4m81hrZ/uSQX0BechFtXduLh/kZkg5Og2YflXyXto1UQVkF43BQY2PZ/Q0ivOhBRU5GiGMA+aQq+dv/L2OQ/Awm1mReapTqhEjPL6qUXhD0MQpKC3O8nUNQKQdHrKkSB23wPKRCASRIOgxC8H58tK6jNbMFPFtejXT8Iv51EwWjcuLx79uwPvaGz8N5l5/9XU3TvPxIIByiL4inPT0jlLTSe1OuN+J4rbzEKe8LjnvhT8MI/WUm4+/n5F4ekjctiPhWDqMGa1Bh86/Ht2B+YgoxSC1XW4NgmNJgcWKZIUaEk3RTP6lkG4bAyyPeF3BnhFXiiTrwEQvIUCmREEanghQOLORhn78adVzVgqr0LPhhIpmt/V3/RrqVv5I+tjyy8oa2x+2aqMzPgn4Tbn+rEQ4M1yAQnQ7Mph5EUYDIYVU4WViVh5VH9JwvC5Pq539SFTf9Kls4+ZTx+vE7B73fYGNQaUVIjEEUVjlmET/QkniVS5WwPhJ4E8iJgjgQhQbBcY0agEhZeVTWJDDh2iVyKKJaymOXrx48W16KttAMwTWRK42+tu3DDP75xOg88fdENNcFdN5tyCQmfJwk9EE6EZnlqcRWElQdBpZ/gTxaE2fWzVsHZeIEk+zHgm4nPP9SHNYkohHgjHNkPxxZ5GUKZFbl7gaydPIOekdpJABsuZj8shTwrJQ/cPpTA66mqkujCMfPwawJK2QQWxbL4/qVhjDM64VgMe/sb/m7S4m3ff+Nkbn9s4Q0tDd038xw6/wTcvqITDyZiniQkEJIkLO9BK8UIVUlYKcofHvdPEoR7Vl0fHRN7dchx98MWVHQKk/Dxew+gr2YO9+VZvAQauSIoa8I85ONzKHmXUasuigX1QFiO1/b2ZhyEEvcnciMNdyHQvWzIgncvJzeIq6eo+KczMmjDQdglAfuHxn9y4uUdP3vjdO588twbmmr332yKJaT1KbzQE4EwVQVh5Tn/PfQEf5Ig7H76/WdFA5tftdELWwvhtVwTPvdIGsn4LLjUo4wHC/Ooz7JT3nO0HzLM8Bjt4Qz6Q6VjjgDhsMrqpTjQjtGB4hqQcgP44AwVXzo9jTH2QdimgP2Jto9OvGLD/f8LhI+fe0NT/d6bDclERpuC21fswoOJOE9lqqqj7yEUVPhR/iRB2LfqA58N+9b/SNDSSIlRPLZXwS1rJQzFToPAilBkEYzCwkQqne2FoTkUeTZsHeVOeTLYeD5Cr34T+QxFuGS44eoqSUHPSENBHRyEdhFKoQ8fmx3G301LoK60G8xSsTsx9tLJi/9/e98BHld1pv3efqerd8mybIO7wd30YsBgOoHkJySbzW7qbhLCZtl/k39j82ySZfcPLZBC8gdISKGEhJIAbhiMDRjLxjbGTbYl2VafGY2m3n7/5ztXso0diMGIka25z2OEpNHce75z3vnO+cr7vrPsvUGoIalMxL3L9zFPOBAaD8mmwAx7gEJgJs8gyPftT0oQ9q687gG/uv6f+KCOuFyDX7zWj4f2VSIRPg2cnYZP4mGZJpOvZpSFFP5gK56k2OTByhgvakqVNt7lJe5dEs0kLzooW2ZyLuui5zkOspGGmu3GFxZU4AvjulGaboHrKNjR3TB/yrXb1x89mXufP/+75ZV77zAFDSlpEu5bsQ9PxspZK5NE5N6U/shjZJSet3AmzDcE86DK9FEMOf7StY8r6ms3mT4N8eBk3PncHrw4cBoS/kYGQpWEczUDgiAy72YP9Qyyc57MdpjCYOqCQOjVrRwGISXpRcfLIRI5MOnpEWAkM41gtgdfPb8On2s4gJLkLgA+bG+vnjflpt3H9Ny1Pn/xd0srdxwG4fL9HgjDjYc4ZvJZslYA4UexGk/8PU5KT5h89fzNqu+dGTkV6FCmYsnjW/GmMw0JpZaJjkq8Dc4gDWgBJqUqaJSsOoZw5dWOCiRWyc6OMvOAVLjNwjCDNaSS421jdZ6Dw5MCLyBbGQQy3bj14rG4uWYfSlPUcaNiW2vp5Gmfat9x9HS0vrBwSWnF9qWGmEVanIIfLduPJ+KVGAiNgeh6BdwsCJRHsqeCJzxxEJ3oO5yUIIyuLYuFfNmSlBxCmzwV/+e3zdgpzUJcKofIGeBIvowEWziOKQBZrCiUiJUIhApb/CJyfxOE1GOoC9wh6TKfqcGf6cBtl4zDJytbUJbZw0D79t7wpOk3dx7qIxyalNYXLl1SWrl1qSEQCKfhR8sO4ok4ddY3QITOaBYpbVIA4Yku45P77086EBI9aK7Zb6sShxhfgl3iJPzH75rRqkxHQiiDIFpwqdaTirddDgavsMjoEAh5hzhGaRPoES1RTYzHL+p5SgrUsJ56ar+gliZJhMXYuR347Sx86R7celkTbizfi/JcG3XDYuOekkmzbz7czHsYhAuXlFdsX6oLWSTlKbh3+X48FfU8IUVaKV47skC4Gf/vyhDmhbsh2UnoZqGL4uOA98kHwtUXqLb0Vo5TbESVMrzNT8S//Xod0iWzEXMjcAbBJNBZjyhheJ6BkFqZyPOoFlXQeO1NXlJ+EHuDX5nuILsGW4xkCbaeg4/LIehqcAcO4vZrzsCiUAsarC7YOoftB0smTf/E7mM8Ycuyi5bUluxeaggaoj5PLvvpvnLGO0rbXcpDeuVy+bnow4Y+CCivaosSSuKb8dDiEBaEPBBqZhlSztnzqj8ijpn8jHLk3/WkBCGUzTkKL/Yq5djITcB3Hl2HdGQm+hFhYCM9eQ4KEQuxi86EtCWlBeezDCb8YvGkkEuRqcG84iAYGCHvYOKeirhdksC2dQTdDFQ3DWegG9+8egaujuxlIIQtY9Oe4ORZn2o55kzYsuKipfXh1iU6K+Buwt0rW/FMbwVSASL/HWpiyl8r09EgLI5vxsODIJStNHJWCdLuefOrFjx6TOR35C/tk+cJTzoQtq6+QG1U1uWgSIgqNdjojsd3f/M64qHpGOCKWPOtF+zwCrIJcOTxLJ4Ksl3IrsGiow68gMy72pmO6O6jDSvJWBMkqfLGzxmQrSScVDe+sWgSPlXehhq9DZbJYVdH9dypN+zecPS073jhgqVjizs9EAZrWQE3a2UKjB8EofcX+SrfLoBwZAD1pAPhgdcW+OrUt7OuY6NPLMc70hQsfWwDuuVJSPClLKLpsrSEp0M9tNkjr0eJd4FAyM5+stdVzw+eBYc2oQygRInhla6RlLTEATJvQjDT4HJ9+OoFY/CPNe0oz+6BbQnYP3DavKZFW45JUbSsWLi0PnRwCZ0J+0LVh/UJAxMOgTBfAPQ23O/ejhY8YX5AedKBkAIz9sYiA9CEhFKJneJk/OeTb2EPNw4psWIwOU9ZP0o7EOiG6kM9rhlKjntRUi9JT9wznhDoUPLcozscAiE1BRNZFOeQxnwOop7E5+cW4ZtNB1CS2sWS9ft6x5wz/ppd646ewr2rFi2tDbQxEEaD1bh71aBIaAGE+VntI/SuJyMIufT6mpjMDxRnA6XYzU/E9596C1utsUgqNYzEiXkwAhXl9xxiHfUIlcgTDlWoCLbk9Q2ySppBcZbBsyBzlNTKRFHRQR4i0nXnYEAy0rh5qoT/mNiB4vQu2LaKtljDFeOv3v3C0XPMPGG43QNhoGFwO0opCipb8zzRiPeEOGdB1fzfvjFC1+8p8VgnHQjJ6pkN894Q3L3z0iKPTv90fP/pLXjDqEdCqYM02Cxr0aZSEgDHqxEdEl7x6Aypq11gXymQw3onXG9bOrQVJQDSJRCcOaqvoU57GzCSuG6siTvP6EZFbi/ghtE5UP8PtZdtO7aLYuUld9SGd3+Xytai1NS7Yh+Ljh4qW4P3LPm6jms7WgDhsE/PSQnCnlcueCSstvyd6XfQp0zE95/Zipe0evSr9ZBMiYm8DIHQZmdA2oJSQ+/glpRVjXqSaJ5ndCH9FRCysAwBj0BIqru0fdWSuKImjXvnxFFjeiBs74p8r/Ga1v84xhOuvPiO2giBMIeoOgn3rBwCIZWteVvlAgiHfY2P+BuclCBsXX75f9eVt9yecfqR8o/HL5uj+MkOG3rFDCANyJzMOh9s14YlEnsaINqip4A0KH9tUvqBUhRMFu1IT+h1VQwpNfEEQkFAzpHZzyXBwXS3Bb+5gkedthuWKSOhVf2s/MJdX/lr29GqyPYlrmwhJp+Ou5fvxTP9VUgEx4AzbYgCx3Qu8nUdjyfMcOefVTnv16/n6xlHw33ztwJOwLpd62/5rM9a8ytBTiHnr8dTu3V8rzmFXMVMOGkeMqcyDUGivzcEqgF1Pb5REvwcYl+jtqVBHpn3AyHnkDAhVZqqsDhi4wZON7fjkcUqTrP3sIKAjF6xomjVLYu4pUvfdcRrWXnxHVWhnd91FRMxecIRIBwHzjQLIDyBNXAq/elJCcL2jTfPCutrm1U1CUMqxZp4AN9c3oFE8ZmwTR94+MDZFlzRhcmbLPhCgRjajspDPKQMhEOe0IugDgVkvDOkJ5dGUVFK2FtUCE6tUHBQm96GX1wVwXSxDYKVgiyNSe5qqaqdetPLTI1p6GpZsfDuqnDLN13FQExuxD3L9+Dp/lokguMYlymV1oF0EvN0FTxhngx/1G1PShC+tfraotOLdvWLTgcMwY8dbg2+8sdW7FcnwuCKIQghJmdN0VGL0xnYqE+Q8YjaLD8x2N7kpSU80HkR0aH/P1THQiAkK4kKLIdqPV2U9m/D3YsiOLs0hjAXg6BHsHd/Vdmk69+MHWnfvasW3l0R2OuBUGkogHBkrPkR9xQnJQjJiqmNU19VnNZzqDb0oNqEbz3Xi/Wpcui+GrhqmBEwUWk2Tx2BjPz3cLPuodK0Q5yjh/OCx4CQtTu54ASB6RqSXmE4sR3/Ol/C9afzCJv7IGk+xHrGnVV31WvvOjvtXnbBPdWR9lsJhHGlAXcf4Qkdy2bkxGw/m6er4AnzZPhTwRPSGHrWTb6rVGm7zeUs9KpN+FGzD3/YrmMgXA9TDsOxXIicA8X2QKgJ1FdIzboUhPHygkfmD22OIEsEUO4RGvbkHTkmsy2LPCzLgivICGdacUN9P75yfjWKs9sRthUke+r+uWzxph8fad/dy867p7qo9VZXsRCXG3Hvsn14ur8acVLqtWxG0095zHxdBRDmy/Lvvu9J6wl71sy4rCyw50Ugi6jShOV9U3DXsn3oCdWC+gypE0lyHYRML0+YkmRWP0qdFKJrQbYJckRvwXldFu8CoZc4YFEWaomyTCgiGAhJ5z6kdWC2sAvfvm4axnFtkFJpWKmah8KXbH+XKlPr6oX3lAa33QrZQlw8Hfcub8XT/VWIh8Z6IBQ48LQ9ztNVAGGeDH+qeMK9zQsjVW5LtyT0qmmxCC2Yj+/+/i20BMejT4x4kUwXHghdHllRGuykIKlr55AWhM1RRwU17srv8oSH7ERF3LYFRXRgmyY4UYbfjKIyvR3fv2U+JltbUIMc9HR49+vW2CkXXvjyoWLU1pUX3Vse3voN8oRRaRruWdaKZ/srEA+N8Tw108qw86ZZ/74gtJPImWUopCiGH6gnrSck0xzYsPiJsLjxRl42kXSa8Mi6GH69vwh94SZorg5FlSBo5AmJZZRksakCxqOVoEgpBWWoYoYVrlG0dLCkeZB+zQvSsP+64GwDksjBIS/q6OCzPfj6pU24qeQd1Lu9gCZjW2dNw7Rrtx0YmrbOlRffWxl8+xuaqKErMAN3r2zDn3vDGAiOPRQo8ra/+Sle80DIDfYTCnhXATcDYQnSOK9QtjbMODypQbh51acvrQysXeYXuiBK1VgfK8FtL6ZwMDIRhuhFMklll3OIgVTxyH65nHfuGyzgpjMlXUNcLyxCysKhgxtS2r5ylFSnyhs6x3lnSUPL4BNTfVhyZhylsc0wDR5Jc8onqy/a8sTQnO197qJ7m8p2fiMrEQin4e6V+/Dn3gjTrGdldEwT43B6ZJjn+pi3PwaE/Zvw8BURLAh3QbZSBRB+TBNyUoOwe8u3AhG8lBbNneClIFqcBtz6bBzb+THIyQo757EmCVeGBRUO6dILlMpzwDs+lrIYorl4FwiZ8xv0gY4DWSAeGJMJzKiyANe2YJgazvT34CeLfKjP7oQsBdHVX/y72ot3fXpo7lpfuuG+6sDmr5tCGlE/kf+SUm8l+sNE9OTRW3CkEJwnoqcCCD8mlP2N25zUIKSxtS+f+oPK8MF/53gTPUoDft9ejIff6EMqPAZZ+JncNTX4WhyBktrZM6yChgifaLNJDbvkjTyxUC+B73nBwYuiqQI1BNuDIJRhmzqryKlKbcdDN9ZisnAQrp6CLdSiNT6pcsZlf+qlv377hSt+PbZ0z2dsPoV+fyMD4R/66pAIN0F0ySPTebAAwpEBhfw9xUkPwkzz9dWC/XanjT5o4WLsdOrxf37/JnaLM5Hx1UE1iVOGRD8Fj4V7UICFyaRRlwSjmTjcUjToAA/HZWgrSpFUdoB0oMgiDE2HJIkoir+Df18g4tqJIrj0bjhWMTriMz45ZfFKtiXdu/bqR8vUHbfYfBJJXz3uW74PT0QbkAhRU6/HeZPPZqaCJ8wf8I6880kPQhpMx0tzf1lS0vV5EuOMcSV4eruFe98QYFXOgmOSNvxg4IXR4VMtqbf0WUE3O5e9Nwh5IoqyLAgEYNdlpWaWYUKWZYTT+3BFeR++fnEDKp0dEDQXSWPqnyvOfu0q5gmXXfBoQ1nrLRafRVJtYimKp6JVrJ+QOj08Uqn8AbEAwgIIPzILtK773Bk+Zc1bstIJzvGhxzcZ//yHXmw36mAqAaZLKDoUDXVgC1TG5kVHKVZKHpG2oUPNDPQ77/K+EmkUJetFUWa5PboEkljjeQT1HoyzWvGtK8ZjprQLYTsOnSvF/j65euritu7ONZc+Gg5uvEUXTPSL83HvijY8G5ORCDVCcIh4w2scztdVAGG+LP/u+54SnpCG1PLatMcriw7epGgZRLla/La9Cg9viCMRqENWCEG2BJacdwRi3rYYKOEKLHhDqYn3AiHzVeQBJQWGYTCeREmSYNvEQ5qGv38fS1VcX3sANehAVsshlpD/teGSzA87lp/xTGnp3qszgoC4tICB8LmogoFQ3SEvnM9lcFwgdM+eX7XgsQLb2jBO1CkDwq2vXTqxzr97R8jsQlYIoN1/BpY8uRlbMB4D/gaYOYttIXXK8bkmwoICPWewwmxi6naG2igGPeBhjzhk/aFgzeEUhuSYENJRLChP4ofXhVESa4afdA9N6UA6rUwTeN9vAuHUlSkxgD7/FNy3vA3P9BQjHWoE71KvPglwU3V5frrrCyAcRmR9gLc+ZUBIY+7ZeN7SkL55iajarGvhle4A7lzZg27fOOSUItgiSVS7cHUDIVIvtB1WLeNxzwyVj3nbw+MBIQV8qCqnNPU27r2xHKe7LagVchAMA3ZW/KwrcJ8XFeeCpBRGn38io7f4Y08lMqGxkG0NPOcylvB8ddfnG4Qtz38tLHKpEt3gYxOveSj1AdbtKfXSUwqEe5u/GCnWl22Xhe4aPliGbq4ej2wy8Nu3YohXToYWKIPen0BEDQA5x9MxhHkEAA+fBYdm+eho6VD6gnKMBFtekiFGt+IzE3V88ZxKlMZ2okRxgFR2B2S+CoJRnBKKEFcn4Ycr2/BktALpUBN8BlFuOKBdcb5OhfkE4cHVV1zpV7oechy7nJdrN2k99efWXPXz7CmFruMczCkFQhpz19qzLy8N9jxvGAmYvgoc5Mfhv57dgpfNSvRJZQipQegZAz4pzKKcvEDZ/CM7Gd4NiWNBSHehgAoxe1MNjYsidwC1ya2485azMF7bilrEwJm616zI2yAQ9iuT8MNVbXgsVoFUqAlB3WFpCou6mY5zsj7ql+ULhPteWHxLdUnbo47VBiWoIq2Xoat/+uxJlz258aMe48nwfqccCMnosQ2XPsjpu7+o+l1kuDB2mGX4+h/eQaz4NOQ4AaJainRahMDLnkQaR+IwR5/9jp6+Y6FCeUcCYZCzEew7iE+eGcI/nmXDH9uECkGEq+VYH2JKKEG/OpF5widiZUiGG0EKT6Jr04Z2RG9HU865H6kWxYF1M78QEXp/riIHSbVhmQn0JX3wF103u2jW7wogPBk+NY7nGd9a/bmiJt/O9UG5+zTdTGMgUIen2wN4cM0e9AZrkBZLoSiV0HIWRME8Kk3wXkGSQRAeoazL6BNFDrymo9jgUWe34zs3lGOSrx3legyqpcF1FaS5EsR943HXyr14MlqGZKgRqk3SaB7r20g+E36UIOx6debSkK9tiSokwdsSLCMHKUjikHXY1zlxzrhLVzQfz/yeaq85JT0hTVLHKwvPLJNaNslqCv26A6t0Gh58tQMPtYno8zdCFQOsnWgoADPESzp05uMP7UOH+C+OzB8OisbwNhzBhmA5CFoq1P5WLD7dxefOLUWVvhHlXA6uGUSKK0M0MAb3rNyFP/WWIBkaC8nyJNjytRX1NtVHdVH8lQLulH3W3OqzHz9GZ+ODAGHbthvlmvi+H/qk7q/JUgoul2NbdUGUYGdzyBnV0PmL5pad/dsTus8HeaaR9NpTFoRk5Oias//Zr7TcL/IZgC9Gu9iIO14bwEudAJRymIKPnesY3liagIfgUI/fIOnMID3i0eEa1vlAG1HOguGaUGUfVEOAkOhFmX0At39iGuYGW1BudUK0FeQQQr+/Hves2oWneomBewKjYKTgTj4rZmhcVHhgUu2syKMktgkPLy4alEZLMFWmJM6dUzPvsQ/tofrWXF/tE9seE4We8zjRRJL4XwUOQdWGme5HUFKRHiiG47tkbmTuLwogHEmfDh/Vs/SsP+Nun7Pjm6FABGk7hF3yODywai9e6fBBLxoLjfhJB8Vi4AiQDJW1PwmCDtvVWb0p+z3bNlJ1jcRYuWU3y4iDDYHOlQJUw4RkZQAzitN8Mfzgqolo5A5CtaKsICAj1+Kula34XWwMEuGJkC0Lou0yFan8XSSeKsKEBQJIWWwTHllUgQWhXvDoQ9YqQb97wZyGDwnCrjVXnR+Rdj6jiLGILdqI80XoC07G6jVvoElN4sJpjZCzSVhmKXR3zpyicx790GDPnw1P/M6ntCck87juUj6z6alnhVTbYjEgo99fjb1uDe58djc2xSXkIjWwfWGvdcnmISPIgqWuk4UgUgLDhMVTPpECKCLjL6WopuIQCEHhBVYEwOs5iLDg8gaKMgdwY4ONzyyoRpnTCs41kJNqcc+KVvwmPhaJ8GlQLYO9D/Uv5mtLSvcmFjoLBnjRQEXfW3h4UR0WhHrAcV3I2sWI6/Nnjzn36Q8cMDn48sKvVBb1/0S025HUB2CWNqFLGYufrGjDvr178YV55bj09CKoZgyWFkZ/cvrM2kueeuvEl/TJ9w6nPAhpSqjvMJB7ca3qj54xYJqIcyFGvHTPn7diXbYaCbkWfkllcmk5xwUvKJAckQUOeIW8oaddT+VtRF8v295X2r5q9FORhERdVk9KnlNN7seY9DZ8/zNnY5LUDtnJIC3X4EfL97AuilR4HBRLYx3+Fpe/6Cj7AKBndm0IvIPKvq14+LIxmBfqAYQOaGYESeOsWTXnPb7peJd2zxv/q1LVd/6Pzfd9VlSyEBwdTukENJtNuPv5rXi7X0ExBnDbmRwuG+/Ahy7ALUdWP2dG5fynth7vfU6l140KENKEda25sTwU2POipu2dGSgLIGX5sMcqxX3r4tjYqyLnlIAPViBuWTAcHgEhyPoMyddR54VJ+TwmtWazc6NoS6z+k0KcmqFDVn2sGDxnWigVsgj2bMC/Xz0ZC0t74bMHkFBrWCvTU311SIfGQ7ZzeQchW8jUFeJaJLeBsujbeHhRFeaFo+DQC8MsRlafN7Pi/MeOy0PtfulTi8eEWn9laXtL/eURRDUDjlqONzqBO19oRZZSRA4HX3Qb7rmqFmdVZ2C6XcjkwjDcS6c1zHl826kEruMdy6gBIRmkdfXnqiqLmjc75u5KmROQkYvQoTbgxy/sxEttEZgVU5FRFKQ0EyIvQaFcH2miDWrcM1Io0SOOEmwf205KvAlDz0LxB6FZLkyHQ4moM9m0r55XjeuruhGw+tGrNuKulW14qrcRyfB4iDbVjlp5qxsdio4Krieeo8tA8VB0NBQFb2dgaxVIcSce6gAAGV9JREFUxKedWbf46c3vt6D2rLu0oiKS/TfH7LxNT3YhVFyMAacIMaEaK9ss/HLVVoh1UxGL96Istx+3Xz0TC4ujCJsHkZYdGPxYdEXnFp954SOJ4124p9LrRhUIaeL2rlgYqY50PCO7sfNdyUaKl5AINuH3m208uvYAomIJ1Oqx0B0OmqZDEX3gbYGF84lOn2lbsIoZ8oTkCLMQeaZmAU23Ifh94LO9qNA78C+XjMENZXvgM2PoUSfgf1YdxJN9YzAQPg2SbYDEZvLJMUN5Ssl1QVRYGVVCUf9G/HqxigWhLvCWDluvgJWZf0bxRY9v+WuLvrn5i1KTtvWmgK/3V4lUm+Ar8kMIlyHmFGF7n4pnNnZjdasGLlIBM5dAU9DClxdOxlRfHOPcTrhaGgmlBnu6w9fOX9T8zKkErA8yllEHQjJOy/NfU8KRbb8U3AOfDgZySNomspGxWN/J46crDmB7OghfQxNSFnk8FwI14LL+P4qUeuc+j6ENsIl9TZTgmNQR4YJXJSDdg4p0J759xThcU/Y2fFYUvepU3LmiA49RdDQyHgpFRx1rkGcmP6EZ+gAgEGrgkZODjG3tt1dIWBDeD7gJmEYForHZZzQt+vMxIOxcOfPcqmL+24bWuchxU/AVhRDVXMSCY7Cmy8JPX96HKFcKXygCrm8vLqsz8HcXTEGxmAUlPpSBA+C4sq5ObdaipvNeHJVnwSGgjkoQDg3+4OvXfKdM2vc9getDyjWRUmuw06zDU82dWL67GzlfGXi1EjZ8TFR0iCGNEvxElUj/wEmMcp/SGqIoIutqCLo5VKS68LULqvGJ6h3w2zF0KzNw54qD+F2slm1H/ZYOmRSiGMVFfi4CIbVTGa6InBRmIHx0sYizQwfAu3GYRgSZ9KQzKi9aewiELa/MPrNY2P+tUj93czadgBopRUwjmcZ67EkJeGrzAaxoicKsmIB4MoVSO4F/uGACrh8LlOndbPut53RiFnjIMMb97+rzXu3Lz+hHzl1HNQhZ5HT15fMjcuuTEBJ1ruqin5fQ7y/HqpYUnnyzG612EzJqLXQXUFUVgkbnOAu2SMXZLiRqiTKJ1dsrdzN5C6qdRjDZg28tGofrK9+BaiXQpUzHD1d14Mm+SqRCDfCbWdYG5XD5LVtj21EiRyZPmNiCX14h4pxgJ2Q7C8mSwaX9s6HJ2+Ih5ULT6r8l7E9/WuZzECQeSZ2D5h+DHqURf9ph4LG1u5BRwlBUFW66G/PrVXx2ViUm+bMot5PgcyZsoRRdSd/Xxy588wGqkxg5UMjfk4x6EJLp+9ZeHYLe+eNAcOAzvBDFAExwVRPxZreIp7dmsWpHP6xIFVylGAMpA/5gBCZstv1kRFHkEAmEg3JsIV5HINWDr13ciE9UbGcg7FRn4K6V+/FUXxmywXoGQtEl9m+J0W/k42IEHy5FR12YkoyS/rfws8U+LAh3Q7Y0KI4NLmu+DtuucVSMsSUenKIiafKwlVIkhVKsb0vjjxu78HqvCjdcwwoWSrg4bpjXiEsmBDHO7kCRngRvSrCdst7+XO0llQufHdXbz6PnugDCIyxy4KWbrigR3vm5P5CszWoZOIEI+qUSbOsX8fSmLqxpN9EfOh1msIo14+qmAZHnGBObKFCe0YZrZaCaSfiz3bht0QTcVLYbihnDQf803LOqHc91lyIbqIPPyjL263yC0KscFUHCHQJvo6x/Ix64MoJZRd3guQGIjgGZFI45CRD8GLBEpANVOGCUYEO7gFVvd2Nf2kS/BQRDJZBTHbi83sV1c2tRF8wg4A6w5mU9zUHP1f13OjDxBxPm/zaZjw+ckXzPAgiPmp3Eq18pdnKb/isgJL8k+3PIYgBCRS32ZErR3KfghT1JrNsdZTlFWw4xIqmMQedDBZIgQnQNqHocvmwHbr9yIq6P7IJqxHAgOBn3rmzH813FyAYavBQFpTzymKz3QKgyDy4KOsr6m3H/VWHMiVDZ2gALPOkoQcpUAV8V9qc5rD+QwrpdCbTGA0haCvggD9HOoiks4LozqnFJlY4a9IIzEyBeLFMq782i9hM1s1a9OpKBkM9nK4DwPax/cPVX5qviru/J8o6LOXEAHLU+CRHEHR+29epYtvUANvY6OOBUwy0eC0eOIJ01AEtHqWIikN6PL51bjZtrO6Ea/ejyn4b7VrTiLz3FyAQamSQaQSCfZWtUbCA5KizHgS1bKB54Ez+9MoCzgr3wmxpyfBH2BydjzQEL6w9msK6lB1FNRiAYRohSNQP70RjScf1ZEzG3UsT4oAMu0Q1BN2DqMixxzJIkP/GB+rN+Gc/nIh/p9y6A8G/MUM/GxZcpTscSGPEFquQgracglVcirlZgawx4ZXcGG9oyaIvZcHzFEH0hcGYK/mQHbr9mOhYVt0OxEuj1T2AVM3/prUAiMJbEgiEQv42bR31CAqHLM0EYTeFRPNCMX1yh4Dx/FD7dREqoxY+2GHh8exwxqRhypAycaYNL92BGrYSLppTj3AkRlBtdCA0cgI+qjUwfbL7yWbi1t4fPeXHXSAfASHi+AgiPcxbaXrrhipJgzx2y0Dtbs/vgKDxcUYVuq0iiGO/08XjpnS5s6kwyWe1iI40vXT4D59fEIdhZJOTxuHdFK56OViIeHA/q2BA44kKlVHn+8oRENmW4QE5RWGDm4UV+nO/vg6o7iEkN+KcXurA544Ms2ZBzvZhXreCKWWMxsVqBmO1hJXlFkgs3nYYkVXb2GXVfrTQu/Qt34dJDEnHHaeJR+7ICCD/g1HevvXw+L/R+WRSzfyc4CYSkFDTTQUatQVKpwgHTh9aDvejYtgVXLZiICYEEHNdGXB3rsa1FaxEPnjYIQlLDMPJHAMx5eUJiYtVlGaX9m/HQZSGc749C1U0MSNX4wau92J0G5kyqxZxxEZzuTzPSY8FMQqHOEtsP11F32hb+MyOPe7pm9nOjkqzpAy6jd728AMIPab1dr/3LRDit15eJb33ZJ/TUW64DXgnBcPwsciqJLgQji4BlQedU9ASrcNdLrXi6pwmJ0ESm8kTRVIGj9EZ+PCELzPASTMeGLRoo638Lv7i8HGf7o/ATURVkdNkhSJEIBDMBOCmEQyISiR6oogoLtW/EspPul8cs+FN9/W25D2nKUf9nBRCe4BKg+snxztvnpRPtn66uLPp7I6cB0CCqGkQikMrZ0PkAusM1jOjp2e5xGAhN9PyPa4F4wQ9Rnp7gs3zwPycQKjAdExA0lMW34GeLK3FWsA8BMwXYJsArLHjkWiZcQULKlmELoYdcN/ybdG7S62MvfIQGXLhOwAIFEJ6A8Y7+051rPx/yc5kFIjr+UQ103mhk21CmuNCYJ2zEPcsP4rkuAuFkUMUmya1RhDSvF++CeXEeKIlvxYNXVmFuqAsBuweuo7MPEIcPI5tRVzmo+FXGbnhxwnlPjvpSs49yzvK8Aj7KoYys99rzypz66ur0eiG9t1rjBUQDTbh32UH8uXMsUqGJcDgN4CymIpy3i6POfpvVr/K8gOLYVvzk6jLMC3XA50Qph4+UU/49Tav6af3Zb3UVysyGZ6YKIBweu7J33fvqvJeqfO0X6oKBWKAJ9y1rw/Od1FnfBJvTPLHSPIqEUp7EIe0M6v5gnnAzHria5LL7IFvE/RJCBrOmVc1ZPiqbbYdxaRQCMx+Xcbs33LA6yDVfYAhZ9PlPw/3L9uHFrjokQ2Ng8SMAhCw5YjP5N57jUBTbiAeuCWNBURQ+IwXdLEFnctaMiYVaz2FdMgVPOIzm7dtw7fYA3pykSzrrJ7x/WRuWd1ViINTAmoMdzgJPFIB56iXwqnU8uW5ijCuKbmSe8OziXvhJa9EIIc2fM61qlNJODOPSKHjCj8u4fa9f1RYSN47RRRNdvmm4f9l+rOgu9kBIxMEcicJ4uhb5uBgIXcnjWwWPor5NuP+aIpwd6UXIicLQw+jTZk9ruPBPhe3oME5QwRMOo3Gja65t9QkbGq0gh07qrH/6HWzW69HNF0NnQr0iOCd/KQqWeuA80mPHNFCV2YHvXyjjktoM/NleOHoZBsQFMyrn/77QejSM66QAwmE0bvbNGxK8sz6i+Xjs903B957YhO1uA6J8BXRSCBZoK2rn1RMS3yq1QlJBeWnybdxxgYxL6zUEM33gjHLEtLmzai4+fsrDYTTnKfvWBRAO49Sm37woxvPbStIyj3bfNHz3N2+gnZ+AfqEGBvwMhNQsRNpO+bl4OI4LQaCkvY1wfAu+c56Ey8fYiGg9EOxS9Ohnfijy3/yM5+S8awGEwzRvbsvXlP7oqqQkHZAHZB/2yGdgyaNr0SWfjqRUBdP1MaluSojnq4Cbla05PDg6lAoufPFNuG2eiBsmiijWOiHYEcS0GRfWn/vnl4fJTIW3JdmTghWGxwJEmaEqG5Pg+5FQyrHZmYzvP74BMd8YJIQIbF4Cx8twmdBFfjTrCYREYOU6HFzRgS+xDf8wKYe/n1eK0lw7eENBT3L8DeMuWfPH4bFS4V3JAgUQDtM66Ft7dk3At60DgoW4rwGrY/W45y+7kPSPQYJX4fBUuU203iQykz8QipwAw3LY1jiY3o1rKrtw66WNKNf2QtSBWGbM7Y0XNf/fYTJT4W0LIBy+NRB9/cyFIfGdFVAkRJUmPLY7gkfW9SIVGI8U7wdEgRVO57eLAoxiw7RdOJIfkdwBnM3twH/cMA015g4oho2kUXd/1Tlbvj58liq8c8ETDtMaiK057Y6wf993Hd6HbrUJP9ms4PHNOWjhccjwPkiiJzhDyk90JqRjGQlDfbxfHXC8BcN2IIohFOtRnJZ5B9+7eRbGOG8jZCahGyUvRxa0XVSoGx2mhVLwhMNj2C3LPhNoKHplS1DpHaflRGQq5+CWR7ZjKzcOmWAVdFtCsSDCMXQ4rHKGkvYeo/fH+9WBI5hwSFPDkVFi5FDcvxvf/sQMzAztRDW6QXXmnbH6sWMv39k2PNYqvGvBEw7DGuh+Y/YXS9TOB5PpHojhsVjbG8aSVf1oC56GrD8E2+EQMBxGse8I1MkwDA9xHG9J4IfoMhDKtoyAnkMo04drpsn40jkWQonNUDQTjjDuvtDcvbcex1sWXvIhLJCn6f8QT3qS/EnHm3PqA8bO9ojocIa/DO2+cXjgpYN4fk8IqaJxSMkZJhct5Dj4BAU29Qvl6XI4Dib1EnKkNixCNCz47Qwmym1YcjmH6cEeyJaBzICKjt66L0+5tvnBPD3qKX3bAgg/wuk9uPryOlVqfqI4kFrgmDJi6lg811eKB1/pQKdZAydSiYybpmo1wLChUIqCucE8RUcpPWGRxqIEV+RhWQZkzkV5bheua+zBFy6ogT/RjoigwNKK0Z8p+lbdwrV3fYQmK7xV4Uz40a2B9GtzLobb9oAo6xMtQUCWL0KbMAHffiGO9VEFcigEweeHbXGsdQh2Gi6pMgkqExfNx0WSb37HRyLf0H0WMq7B1IaVbB9qrR58bm4FPjPBQGBgN1RRgK45yGrig7o7/r9qLn6tPR/PfCres+AJT2BWXRfcwRevnubj93+pONj2FR0JjiuqQ1QoQadbhvtf2IWVvfVIBerhVw04RKhkSYxXRhJ1OA7Jc0pwWBH1x3+JtgjVUWDaBky/CQ0WOMkHK5tFsauj0erAl+b4cE6tgQopBUmPse2pi1K3rbvi21n/1IenLfx9z8f/5KfWHQsg/ADz2dw8S5qFGimWaR2jcPr5tpG9IRAJL7SNFBQ5TfROaBNrsFkrwq829mBTNw/X1wRejgB2FpZjQndF5m1k14AoCjAch4mO5ufiWUiWcylfSeDi4UhFsHQNRT4dRrQVRVwCV06rwKemF6HB7UaR0wvBNOFKpbDcELp6Mz9Tg1XPOkJ4Wxff2D179s/N/Izl5L3rqARhc/ODUkn25UmyoAVcR3EACRJMWLzOiw67ItTkSmIKkmSWOGayPBwwp3FCcrxj9o8DMg0CcXYGIkiaCrKuCCmooo8LYVWrgSc29mBjKgh/5RRA4+Ha1L+uQ1BFWKx/D3CyWciyyERHTyQ1caJLj1oZZcEFb2bhOHQ+LIJlUd4yiZCfg+3ocKN7cW65hetnVmNmrYAw4rAzSfZB4hds2KYFx/UDKN7scsW7TCOw07HVPY6rxHlOGXApGUmycSJgcQbHCxAchwsCCjinuK9xwY83neg4Tua/H5UgPLDuyufK/LEr9YEOyLwAVVWQ1pJQVRmcwCOTzkGWVXC0WSQeFpfoCTVQCZojuKzEy+ZVaFwRMkIjuoQwVnfvx1+2tGBPjwIhOBZQy5FNcwjCD5u3kFY1mIIDiURUiH17UByU0gNDSXqQwtMHTNr/rcXHzp9HXUcKk1JslqdtKE/A48A7Pq+MjnPAwWQqU34JcI0sfEYfptfyuHRGFWZVyCg2uxC2exGA5n3QmDarQxUgATz9kQjH5WFbAjiO1J1ERirFQlEuYDoSbLkcMaNxwYRzHnnjb43lVP39qANh61ufK6pTW/qNaAv8AR4uzyNrmeBlhWky6JYDQVIgCAJs2wZsEo0AQJLYogwDMpIW1VTa6NcD2Lg7jTc7B7BBH0AuVAVVGQNTU8DpPHyCDMlyYQo2kqoOS3AgGzKT3h7qpqfzIROmGFz49JWk1oa+p0V99O+Hvqdf/a3rSMAdqwrsQOA4Vixg0zOAh2RTkIg+fjwgCoNaGaYgs2CSlG1HOfoxu1LBrLogplbJKFV0RFQBqmhBcgmQGmzbZPYTRBW8IMN1JGZLxXGh8C5kypE6DjSbgyGcMb9s9jPr/9ZYTtXfH8c0nlpDj71xeZhH80ARZwOSggNiGL2WCF+4AlmTB6+EwUkSeqN9UBQfy+NldBuJtI1oxkFHUsf+WBYH4kn0pU1wUhim4kNOkVmjLm8HwLsqRFcGZ1uQqSKFN5ATXDgQPdntQZpDdhIkJm4mWEtTcexXz2G+1+/BIq3vx45B588jryOBSHcUHRfUyKELpBBF3w9V7fDsjCjLMjRNY5Fcn0S5zSxcLQM3bUC0NIQloDIkYUypgrpiAVURDiVhEaGADFGRodkuRFliZ0/B1OBzs1DNDHyOhmJJZ3oW/uDc+f4pqwsgPLWg9t6jSW26vJxz1vcGYMLhZGzNAA8/F8NABvCHAc1hKTxkMoAaACQR0G2AVLJ1HjDEIHQpAE0QYJLUteNAlGUIIgeHtF2ItNpWmO4fT/cQE3B5EySs7bgyA+GhrgkqV2Me6L1rR+G4719TypGaxHtB+P1/Tt6YHwKh6G2LZdt7FpsnEArIaBYCfhUBgYdppJmH43kRguuHY3LM28mcCZ87AMWmfzRSQKIdqejZM5XxamKLg4BqA1wamHk6cMMlDYg4cfD89LP80197fbSswaPHOeo8Ybz5xogsvZEI2AmAMzHgCowh2+UCyGrsFMg+/Q0zB5k6zjmXnWMsSLA5gYl6moIAynGDFyAKMgw9B96y2PlS5HwAab27QYr3IMt3w+V1cI4CATLzgrTbZMEZnjZ9Q4RPH652lAD9foXf7wdigihHD0nbUV5nZ0OBPRvPAkb0T5RUWLoO5LLsdY6sQhAkBAYlwk3Q/R0IjgaBZMZdYhY32IaWDVTgISs+6DkNhqbBJ4lQeZ7tEvyCA79goj/dNK/srOY3CyAcJRZobV2qhqPPviMaO5v8igbRF0QymYEk+uELRKDlTLbFU2grlU2y8xnt6Djyep7PguO6TK+eXkeewCcrkBwLlmGwIIQo+CC4IRiuDltJA7wJ3pbYWXDo4lhukM5F1gnWjhJt4Xtffy0wQ6/2fs7Bsb0zqMvnwMEeBCERbghwOJGxcJNPlKm4XBCQ43hYpgnVpLQGD1tSQNlOErURmJ1cOK4F2PThAlgUYZZlZj/6xyTCdcqZOlBlHyzdQr85ZV71ea8WQDhKMMiG2bfh07eq/Pp7YLWz85tfUdk2SzdybHF6i5qHIIne/xNdPNVYssVG3ej0Ww9QHM/DMDVwg2cfXvBUjlyqjKHXUdSf/pY8jEsL0/ZAR93s4CAIyvua/thgyuGX07PRwn6/ixb7ewHRG5oXmKHILWkVKrZHPGVTlwcnwraIg4bOsTYsx4YhcuyMJzkOHNOCLRJPDdmFFKZ4ZiNGq08kioIEmyKuvAjTtmCbBkQeUGRqZLZgGDwcOwCxYvFc+fRfbBhNa/DIsY667ejQ4A9svrE2wA0Uy5zIwTAgKYd7GTjOodQ1J0uKaxxKPevMVjI78TCQujAA17VcyN5PTN10DfY91YSarqwNbjzprChR7JVebzJZUMbvxN5G8ubAMd1D//8+q5Ezj+q5kEGd8e85j7S1PnQ5Qxvhwz8a+q0uefFaheK/nMQZvMOZnMTJpsiBdzmQnWT6mcuRfeh7icbK7GECIs8yoAZx+xuWK0OGKynsPU2XsqPS4EvJoGQ4ehYJpi0LwZl/2jJaAcg+CEfz4AtjL1hgJFigAMKRMAuFZxjVFiiAcFRPf2HwI8ECBRCOhFkoPMOotkABhKN6+guDHwkWKIBwJMxC4RlGtQUKIBzV018Y/EiwQAGEI2EWCs8wqi1QAOGonv7C4EeCBQogHAmzUHiGUW2BAghH9fQXBj8SLFAA4UiYhcIzjGoLFEA4qqe/MPiRYIECCEfCLBSeYVRboADCUT39hcGPBAsUQDgSZqHwDKPaAgUQjurpLwx+JFjg/wPbn7pXqyC5qQAAAABJRU5ErkJggg==';

                var text = '<div class="row print"><div class="col-md-12"><div class="ticket"><div class="header"><img src="' + img + '" alt="LOGO TC" class="img-logo"><h2 class="text-center pt-3">SINDICATO DE CHOFERES, TAXISTAS Y SIMILARES ANDRES Q.ROO</h2></div><p class="text-center">SOCIO</p><hr><p>TICKET: ';
                text += ticketnumber + '</p><p>FECHA: ';
                text += date + '</p><p>USUARIO: ';
                text += Usuario + '</p><p>SE REPORTA CON EL TAXI: ';
                text += Taxi + '</p><hr><p>ECONOMICO: ';
                text += ECO + '</p><p>'; 
                text += nameSoc + '</p><hr><table class="table table-borderless"><thead><tr><th>Cantidad</th><th>Codigo</th><th>Descripción</th><th class="text-end">Precio</th></tr></thead><tbody>';
                text += row + '</tbody></table><hr><p>REPORTADO HASTA: ';
                text += reportdate + '</p><p>A ESTA FECHA SU PORCENTAJE ES ';
                text += porcentaje + '</p><p class="text-end"><strong>';
                text += total + '</strong></p><hr><p class="text-center">Se le informa que a partir del 15 de julio todo operador debera portar a la vista del usuario su tarjeton de identidad vigente</p><br><p class="text-center">SÚBETE A LA OLA.</p><p class="text-center">Para registrarse, envia un mensaje a los números: 9989371528 o 9988962606</p></div></div></div>';

                style = "<style>body{font-family:Arial,sans-serif;margin:0;padding:0;background-color:#f5f5f5;font-weight:bolder}.ticket{width:58mm;max-width:100%;margin:10px auto;padding:10px;background-color:#fff;border:1px solid #ddd;box-shadow:0 0 5px rgba(0,0,0,.1)}h2,p{margin:5px 0;font-size:10px}.table{width:100%}.table td,.table th{padding:5px;font-size:14px}.text-end{text-align:end}.text-center{text-align:center}.img-logo{margin:10px 10px 10px 0;width:40px;height:auto}.header{display:flex;align-items:center;justify-content:center}@media print{body{background-color:#fff}.ticket{box-shadow:none;border:none;margin:0;padding:0;width:100%;height:auto}.table td,.table th{padding:2px;font-size:12px}}</style>";
                newWin.document.write('<html><body onload="window.print()">' + style + text + '</body></html>');
                newWin.document.close();
                setTimeout(function () { newWin.close(); }, 10);   
            },
            check: function (cargo) {
                var that = this;
                if (cargo.CHECK) {
                    cargo.PAGA = cargo.DEBE;
                } else {
                    cargo.PAGA = 0;
                }
                that.calculateTotal();
            },
            loadTicket: function () {
                var that = this;
                var barcode = '';
                var interval;
                document.addEventListener('keydown', function (evt) {
                    if (interval)
                        clearInterval(interval);
                    if (evt.code == 'Enter') {
                        if (barcode)
                            handleBarcode(barcode);
                        barcode = '';
                        return;
                    }
                    if (evt.key != 'Shift')
                        barcode += evt.key;
                    interval = setInterval(() => barcode = '', 20);
                });

                // Esta funcion busca los datos del Tarjeton o codigo de barra de la credencal
                function handleBarcode(scanned_barcode) {
                    that.id_ticket = scanned_barcode;
                    // Validamos que tipo de documento escaneamos.
                    if (scanned_barcode.match("^C")) {
                        that.taxi = scanned_barcode.replace("C", "").replace("'", "");
                        that.reference = scanned_barcode.replace("C", "").replace("'", "");
                        // Esta funcion recupera la informacion que se despliega por medio de taxi y reference.
                        that.getTaxi();
                        var url = "/ticket/json/get/cobro/" + that.taxi + "/" + that.reference + "/";
                        // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
                        $.ajaxSetup({
                            headers: { 'RequestVerificationToken': csrfToken }
                        });
                        $.ajax({
                            type: "GET",
                            url: url,
                            data: {},
                            context: this,
                            cache: false,
                            success: function (data) {
                                console.log(data);
                                that.cargos = data;
                            },
                            error: function (response) {
                                that.order = null;
                                Swal.fire({
                                    title: '¡Oh, No se encontro el servicio!',
                                    text: 'El número ticket es incorrecto, verifique nuevamente.',
                                    icon: 'warning',
                                    confirmButtonText: 'Continuar'
                                });
                            },
                            complete: function (response) {
                                console.log("Complete");
                            }
                        });
                        //Valida el tarjeton.
                    } else if (scanned_barcode.match("^T")) {
                        alert("Tarjeton : " + scanned_barcode.replace("T", "").replace("'", ""));
                    }
                    that.calculateTotal();
                    return that.order;
                }
            },
            add: function (cargo) {
                var that = this;
                if ((cargo.PAGA + 1) > cargo.DEBE && cargo.SERVICE == false) {
                    Swal.fire({
                        title: '¡Oh, No se puede cobrar más de las que debe!',
                        text: 'Ingrese una cantidad menor para continuar con el cobro.',
                        icon: 'warning',
                        confirmButtonText: 'Continuar'
                    });
                } else {
                    cargo.PAGA = cargo.PAGA + 1;
                }
                that.calculateTotal();
            },
            less: function (cargo) {
                var that = this;
                if ((cargo.PAGA - 1) < 0) {
                    Swal.fire({
                        title: '¡Oh, No se puede cobrar menos de 0!',
                        text: 'Ingrese una cantidad menor para continuar con el cobro.',
                        icon: 'warning',
                        confirmButtonText: 'Continuar'
                    });
                } else {
                    cargo.PAGA = cargo.PAGA - 1;
                }
                that.calculateTotal();
            },
            validate: function (cargo){
                var that = this;
                console.log("validate");
                console.log(cargo);
                if ((cargo.PAGA) > cargo.DEBE) {
                    cargo.PAGA = cargo.DEBE;
                    Swal.fire({
                        title: '¡Oh, No se puede cobrar más de las que debe!',
                        text: 'Ingrese una cantidad menor para continuar con el cobro.',
                        icon: 'warning',
                        confirmButtonText: 'Continuar'
                    });
                } else if ((cargo.PAGA) < 0) {
                    cargo.PAGA = cargo.DEBE;
                    Swal.fire({
                        title: '¡Oh, No se puede cobrar menos de 0!',
                        text: 'Ingrese una cantidad menor para continuar con el cobro.',
                        icon: 'warning',
                        confirmButtonText: 'Continuar'
                    });
                }
                that.calculateTotal();
            },
            search: function (e) {
                var that = this;
                e.preventDefault();
                if (that.taxi != "" && that.reference != "") {
                    // Aqui has la busqueda
                    that.taxi = that.taxi.toUpperCase();
                    that.reference = that.reference.toUpperCase();
                    that.getTaxi();
                    var url = "/ticket/json/get/cobro/" + that.taxi + "/" + that.reference + "/";
                    // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
                    $.ajaxSetup({
                        headers: { 'RequestVerificationToken': csrfToken }
                    });
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: {},
                        context: this,
                        cache: false,
                        success: function (data) {
                            console.log(data);
                            that.cargos = data;
                            that.calculateTotal();
                            that.bloqueos();
                        },
                        error: function (response) {
                            that.order = null;
                            Swal.fire({
                                title: '¡Oh, No se encontro el servicio!',
                                text: 'El número ticket es incorrecto, verifique nuevamente.',
                                icon: 'warning',
                                confirmButtonText: 'Continuar'
                            });
                        },
                        complete: function (response) {
                            console.log("Complete");
                        }
                    });
                }
            },
            bloqueos: function () {
                var that = this;
                that.taxi = that.taxi.toUpperCase();
                that.reference = that.reference.toUpperCase();
                var url = "/ticket/bloqueos/" + that.taxi + "/" + that.reference + "/";
                // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "GET",
                    url: url,
                    data: {},
                    context: this,
                    cache: false,
                    dataType: "json",
                    success: function (data) {
                        console.log("Bloqueos");
                        that.blocks = data;
                        console.log(that.blocks);

                    },
                    error: function (response) {
                        
                    },
                    complete: function (response) {
                        console.log("Complete");
                    }
                });
            },
            getTaxi: function () {
                var that = this;
                that.data = null;
                var url = "/Operator/SearchCarCreate/";
                // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        gafet: that.reference,
                        taxi: that.taxi
                    },
                    context: this,
                    cache: false,
                    success: function (data) {
                        console.log(data);
                        that.data = data;
                        that.calculateTotal();
                    },
                    error: function (response) {
                        console.log(response);
                        that.order = null;
                        Swal.fire({
                            title: '¡Oh, No se encontro el servicio!',
                            text: 'El número ticket es incorrecto, verifique nuevamente.',
                            icon: 'warning',
                            confirmButtonText: 'Continuar'
                        });
                    },
                    complete: function (response) {
                        console.log("Complete obtener taxi");
                    }
                });
            },
            getOperator: function (operator) {
            },
            calculateTotal: function () {
                var that = this;
                that.total = 0;
                $.each(that.cargos, function (index, cargos) {
                    var price = cargos.PRECIO * cargos.MULTIPLICADOR * cargos.PAGA;
                    that.total += price;
                });
            },
            searchCar: function (taxi) {

            },
            searchOp: function (gafet) {
            },
            updateDebs: function () {
                var that = this;
                if (that.taxi != "" && that.reference != "") {
                    // Aqui has la busqueda
                    that.taxi = that.taxi.toUpperCase();
                    that.reference = that.reference.toUpperCase();
                    that.getTaxi();
                    var url = "/ticket/json/get/cobro/" + that.taxi + "/" + that.reference + "/";
                    // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
                    $.ajaxSetup({
                        headers: { 'RequestVerificationToken': csrfToken }
                    });
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: {},
                        context: this,
                        cache: false,
                        success: function (data) {
                            console.log(data);
                            that.cargos = data;
                        },
                        error: function (response) {
                            that.order = null;
                            Swal.fire({
                                title: '¡Oh, No se encontro el servicio!',
                                text: 'El número ticket es incorrecto, verifique nuevamente.',
                                icon: 'warning',
                                confirmButtonText: 'Continuar'
                            });
                        },
                        complete: function (response) {
                            console.log("Complete");
                            that.calculateTotal();
                        }
                    });
                }
            },
            detailpay: function (family) {
                console.log("VALUE " + family);
                var that = this;
                var url = "/ticket/json/pay/detail";
                // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        reference: that.reference,
                        taxi: that.taxi,
                        family: family,
                    },
                    context: this,
                    cache: false,
                    success: function (data) {
                        console.log(data);
                        console.log("Success : detalle deuda");
                        that.datadetail = data;
                    },
                    error: function (response) {
                        console.log(response);
                        console.log("Error : detalle deuda");
                    },
                    complete: function (response) {
                        console.log("Complete : detalle deuda");
                    }
                });
            },
            onSelect: function (event) {
                var that = this;
                console.log(event.target.value)
                var value = event.target.value;
                if (value == "REG2TURNO") {
                    console.log("REG2TURNO");
                    if (that.taxi != "" && that.reference != "") { // Verificamos que exista el taxi y el operador.
                        // Obtenemos de inventario cuanto debe de pagar.
                        var url = "/ticket/json/pay/inventario/";
                        // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
                        $.ajaxSetup({
                            headers: { 'RequestVerificationToken': csrfToken }
                        });
                        $.ajax({
                            type: "POST",
                            url: url,
                            data: {
                                reference: this.reference,
                                family: '36'
                            },
                            context: this,
                            cache: false,
                            success: function (data){
                                console.log("success : REG2TURNO");
                                that.inventarios = data;
                                that.reportedoble = 0;
                                if (that.inventarios.inventario.PrecioConIva == 8){
                                    that.reportedobleUnitario = that.reportedoble = that.inventarios.inventario.PrecioConIva * 2;
                                }else{
                                    that.reportedobleUnitario = that.reportedoble = Math.ceil((that.inventarios.inventario.PrecioConIva * 2) * 0.7);
                                }
                                that.updateReporte();
                            },
                            error: function (response) {
                                console.log("error : REG2TURNO");
                                that.order = null;
                                Swal.fire({
                                    title: '¡Oh, No se encontro el servicio!',
                                    text: 'El número ticket es incorrecto, verifique nuevamente.',
                                    icon: 'warning',
                                    confirmButtonText: 'Continuar'
                                });
                            },
                            complete: function (response) {
                                console.log("Complete : REG2TURNO");
                            }
                        });
                        
                        // Hacemos el calculo de cuantos va  pagar por los turnos dobles.
                        console.log(" PAGO DE SEGUNDO TURNO.");
                        // Aqui se abre el formato dale ingreso.

                        // Se cargar a la tabla de

                        $('#regDobleTurno').modal('toggle');
                        $('#regDobleTurno').modal('show');
                        //$('#exampleModal').modal('hide');
                    }
                } else {
                    console.log("....");
                }
            },
            updateReporte: function () {
                var that = this;
                /*
                servpaga: 0,
                reportedoble: 0,
                reportedoblepaga: 0,
                */
                that.reportedoblepaga = that.servpaga * that.reportedoble;
                console.log(that.reportedoblepaga);
            },
            addServices: function () {
                // Se agrega el servicio como cargo.
                var that = this;
                console.log("Se agrega el servicio para ser cargado.");
                var family = that.inventarios.inventario.Id_Familia;
                var descripcion = that.inventarios.inventario.Descripcion;
                that.cargos.push(
                    {
                        "CHECK": true,
                        "RTYPE": 'D',
                        "SERVICE": true,
                        "TYPE": null,
                        "CLAVE": family,
                        "CARGO": descripcion,
                        "IDA": "1",
                        "A": "OPERADOR",
                        "DEBE": that.servpaga,
                        "PAGA": that.servpaga,
                        "PRECIO": that.reportedobleUnitario,
                        "Id_Producto": null,
                        "FechaOp": "0001-01-01T00:00:00",
                        "Descripcion": null,
                        "Message": null,
                        "StatusAction": null,
                        "MessageAction": null
                    }
                );

                console.log(that.cargos);

                $('#regDobleTurno').modal('toggle');
                $('#exampleModal').modal('hide');
            },
            adds: function () {
                var that = this;
                that.servpaga += 1;
                that.updateReporte();
            },
            lesss: function () {
                var that = this;
                that.servpaga -= 1;
                that.updateReporte();
            },
            double: function (event,cargo) {
                var that = this;
                console.log(event.target.checked);
                if (event.target.checked) {
                    $.each(that.cargos, function (index, _cargo) {
                        if (cargo.CLAVE == _cargo.CLAVE && cargo.A == _cargo.A) {
                            that.cargos[index].RDOUBLE = true;
                        }
                    });
                    that.cargos.push(
                        {
                            "CHECK": true,
                            "RTYPE": 'D',
                            "SERVICE": true,
                            "TYPE": null,
                            "CLAVE": 49,
                            "CARGO": "REGULARIZACION 2DO TURNO",
                            "IDA": "1",
                            "A": cargo.A,
                            "DEBE": cargo.DEBE,
                            "PAGA": cargo.PAGA,
                            "PRECIO": cargo.PRECIO,
                            "Id_Producto": "REG2TURNO",
                            "FechaOp": "0001-01-01T00:00:00",
                            "Descripcion": "REGULARIZACION 2DO TURNO",
                            "Message": null,
                            "StatusAction": null,
                            "MessageAction": null
                        }
                    );
                } else {
                    console.log("NO Seleccionado: NO generar el cobro.");
                    $.each(that.cargos, function (index, _cargo) {
                        if (cargo.CLAVE == _cargo.CLAVE && cargo.A == _cargo.A) {
                            that.cargos[index].RDOUBLE = false;
                        }
                    });
                    var temp = [];
                    $.each(that.cargos, function (index, tcargo) {
                        if (!(tcargo.CLAVE == "49" && tcargo.A == cargo.A)) {
                            temp.push(tcargo);
                        }
                    });
                    that.cargos = temp;
                }
                that.calculateTotal();
            },
            turno: function (event, cargo) {
                console.log(cargo);
                var that = this;
                var selectElement = event.target;
                var value = selectElement.value;
                var url = "/ticket/price/turn/double/" + cargo.PRECIOFIJO + "/" + value;
                //Obtenemos el valor
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: url,
                    data: { },
                    context: this,
                    cache: false,
                    success: function (data) {
                        console.log(data);
                        $.each(that.cargos, function (index, _cargo) {
                            if (cargo.CLAVE == _cargo.CLAVE && cargo.A == _cargo.A) {
                                that.cargos[index].PRECIO = data;
                            }
                        });
                    },
                    error: function (response) {
                        that.order = null;
                        Swal.fire({
                            title: '¡Oh, No se encontro el servicio!',
                            text: 'El número ticket es incorrecto, verifique nuevamente.',
                            icon: 'warning',
                            confirmButtonText: 'Continuar'
                        });
                    },
                    complete: function (response) {
                        console.log("Complete");
                        that.loading = false;
                    }
                });
                that.calculateTotal();
            },
            payall: function (event){
                var that = this;
                if (event.target.checked) {
                    $.each(that.cargos, function (index, cargos) {
                        that.cargos[index].CHECK = true;
                        that.cargos[index].PAGA = cargos.DEBE;
                    });
                } else {
                    $.each(that.cargos, function (index, cargos) {
                        that.cargos[index].CHECK = false;
                        that.cargos[index].PAGA = 0;
                    });
                }
                that.calculateTotal();
            },
            showmessage: function (item){
                $('#messagemodal').modal('toggle');
                $('#messagemodal').modal('show');
                $('#messagecontent').html('<span>' + item.MENSAJE1 + '</span>');
            },
        },
        filters: {

        },
        mounted: function () {
            var that = this;
            that.loadTicket(); // Esto pone a funcionar el lector de barra en la web.
            that.calculateTotal();
            that.payall();
        }
    })
});