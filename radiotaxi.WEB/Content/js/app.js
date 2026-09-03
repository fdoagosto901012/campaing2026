!function(a){"use strict";var e,t,n,r=localStorage.getItem("minia-language"),o="en";function i(t){document.getElementById("header-lang-img")&&("en"==t?document.getElementById("header-lang-img").src="assets/images/flags/us.jpg":"sp"==t?document.getElementById("header-lang-img").src="assets/images/flags/spain.jpg":"gr"==t?document.getElementById("header-lang-img").src="assets/images/flags/germany.jpg":"it"==t?document.getElementById("header-lang-img").src="assets/images/flags/italy.jpg":"ru"==t&&(document.getElementById("header-lang-img").src="assets/images/flags/russia.jpg"),localStorage.setItem("minia-language",t),null==(r=localStorage.getItem("minia-language"))&&i(o),a.getJSON("assets/lang/"+r+".json",function(t){a("html").attr("lang",r),a.each(t,function(t,e){"head"===t&&a(document).attr("title",e.title),a("[data-key='"+t+"']").text(e)})}))}function s(){var t=document.querySelectorAll(".counter-value");t.forEach(function(r){!function t(){var e=+r.getAttribute("data-target"),a=+r.innerText,n=e/250;n<1&&(n=1),a<e?(r.innerText=(a+n).toFixed(0),setTimeout(t,1)):r.innerText=e}()})}function d(){for(var t=document.getElementById("topnav-menu-content").getElementsByTagName("a"),e=0,a=t.length;e<a;e++)t[e]&&t[e].parentElement&&"nav-item dropdown active"===t[e].parentElement.getAttribute("class")&&(t[e].parentElement.classList.remove("active"),t[e].nextElementSibling&&t[e].nextElementSibling.classList.remove("show"))}function l(t){document.getElementById(t).checked=!0}function c(){document.webkitIsFullScreen||document.mozFullScreen||document.msFullscreenElement||a("body").removeClass("fullscreen-enable")}a("#side-menu").metisMenu(),s(),e=document.body.getAttribute("data-sidebar-size"),a(window).on("load",function(){a(".switch").on("switch-change",function(){toggleWeather()}),1024<=window.innerWidth&&window.innerWidth<=1366&&(document.body.setAttribute("data-sidebar-size","sm"),l("sidebar-size-small"))}),a("#vertical-menu-btn").on("click",function(t){t.preventDefault(),a("body").toggleClass("sidebar-enable"),992<=a(window).width()&&(null==e?null==document.body.getAttribute("data-sidebar-size")||"lg"==document.body.getAttribute("data-sidebar-size")?document.body.setAttribute("data-sidebar-size","sm"):document.body.setAttribute("data-sidebar-size","lg"):"md"==e?"md"==document.body.getAttribute("data-sidebar-size")?document.body.setAttribute("data-sidebar-size","sm"):document.body.setAttribute("data-sidebar-size","md"):"sm"==document.body.getAttribute("data-sidebar-size")?document.body.setAttribute("data-sidebar-size","lg"):document.body.setAttribute("data-sidebar-size","sm"))}),a("#sidebar-menu a").each(function(){var t=window.location.href.split(/[?#]/)[0];this.href==t&&(a(this).addClass("active"),a(this).parent().addClass("mm-active"),a(this).parent().parent().addClass("mm-show"),a(this).parent().parent().prev().addClass("mm-active"),a(this).parent().parent().parent().addClass("mm-active"),a(this).parent().parent().parent().parent().addClass("mm-show"),a(this).parent().parent().parent().parent().parent().addClass("mm-active"))}),a(document).ready(function(){var t;0<a("#sidebar-menu").length&&0<a("#sidebar-menu .mm-active .active").length&&(300<(t=a("#sidebar-menu .mm-active .active").offset().top)&&(t-=300,a(".vertical-menu .simplebar-content-wrapper").animate({scrollTop:t},"slow")))}),a(".navbar-nav a").each(function(){var t=window.location.href.split(/[?#]/)[0];this.href==t&&(a(this).addClass("active"),a(this).parent().addClass("active"),a(this).parent().parent().addClass("active"),a(this).parent().parent().parent().addClass("active"),a(this).parent().parent().parent().parent().addClass("active"),a(this).parent().parent().parent().parent().parent().addClass("active"),a(this).parent().parent().parent().parent().parent().parent().addClass("active"))}),a('[data-toggle="fullscreen"]').on("click",function(t){t.preventDefault(),a("body").toggleClass("fullscreen-enable"),document.fullscreenElement||document.mozFullScreenElement||document.webkitFullscreenElement?document.cancelFullScreen?document.cancelFullScreen():document.mozCancelFullScreen?document.mozCancelFullScreen():document.webkitCancelFullScreen&&document.webkitCancelFullScreen():document.documentElement.requestFullscreen?document.documentElement.requestFullscreen():document.documentElement.mozRequestFullScreen?document.documentElement.mozRequestFullScreen():document.documentElement.webkitRequestFullscreen&&document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT)}),document.addEventListener("fullscreenchange",c),document.addEventListener("webkitfullscreenchange",c),document.addEventListener("mozfullscreenchange",c),function(){if(document.getElementById("topnav-menu-content")){for(var t=document.getElementById("topnav-menu-content").getElementsByTagName("a"),e=0,a=t.length;e<a;e++)t[e].onclick=function(t){t&&t.target&&"#"===t.target.getAttribute("href")&&(t.target.parentElement.classList.toggle("active"),t.target.nextElementSibling&&t.target.nextElementSibling.classList.toggle("show"))};window.addEventListener("resize",d)}}(),[].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]')).map(function(t){return new bootstrap.Tooltip(t)}),[].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]')).map(function(t){return new bootstrap.Popover(t)}),[].slice.call(document.querySelectorAll(".toast")).map(function(t){return new bootstrap.Toast(t)}),window.sessionStorage&&((t=sessionStorage.getItem("is_visited"))?a("#"+t).prop("checked",!0):sessionStorage.setItem("is_visited","layout-ltr")),r&&"null"!=r&&r!==o&&i(r),a(".language").on("click",function(t){i(a(this).attr("data-lang"))}),a(window).on("load",function(){a("#status").fadeOut(),a("#preloader").delay(350).fadeOut("slow")}),n=document.getElementsByTagName("body")[0],a(".right-bar-toggle").on("click",function(t){a("body").toggleClass("right-bar-enabled")}),a("#mode-setting-btn").on("click",function(t){n.hasAttribute("data-bs-theme")&&"dark"==n.getAttribute("data-bs-theme")?(document.body.setAttribute("data-bs-theme","light"),document.body.setAttribute("data-topbar","light"),document.body.setAttribute("data-sidebar","light"),n.hasAttribute("data-layout")&&"horizontal"==n.getAttribute("data-layout")||document.body.setAttribute("data-sidebar","light"),l("topbar-color-light"),l("sidebar-color-light"),l("topbar-color-light")):(document.body.setAttribute("data-bs-theme","dark"),document.body.setAttribute("data-topbar","dark"),document.body.setAttribute("data-sidebar","dark"),n.hasAttribute("data-layout")&&"horizontal"==n.getAttribute("data-layout")||document.body.setAttribute("data-sidebar","dark"),l("layout-mode-dark"),l("sidebar-color-dark"),l("topbar-color-dark"))}),a(document).on("click","body",function(t){0<a(t.target).closest(".right-bar-toggle, .right-bar").length||a("body").removeClass("right-bar-enabled")}),n.hasAttribute("data-layout")&&"horizontal"==n.getAttribute("data-layout")?(l("layout-horizontal"),a(".sidebar-setting").hide()):l("layout-vertical"),n.hasAttribute("data-bs-theme")&&"dark"==n.getAttribute("data-bs-theme")?l("layout-mode-dark"):l("layout-mode-light"),n.hasAttribute("data-layout-size")&&"boxed"==n.getAttribute("data-layout-size")?l("layout-width-boxed"):l("layout-width-fuild"),n.hasAttribute("data-layout-scrollable")&&"true"==n.getAttribute("data-layout-scrollable")?l("layout-position-scrollable"):l("layout-position-fixed"),n.hasAttribute("data-topbar")&&"dark"==n.getAttribute("data-topbar")?l("topbar-color-dark"):l("topbar-color-light"),n.hasAttribute("data-sidebar-size")&&"sm"==n.getAttribute("data-sidebar-size")?l("sidebar-size-small"):n.hasAttribute("data-sidebar-size")&&"md"==n.getAttribute("data-sidebar-size")?l("sidebar-size-compact"):l("sidebar-size-default"),n.hasAttribute("data-sidebar")&&"brand"==n.getAttribute("data-sidebar")?l("sidebar-color-brand"):n.hasAttribute("data-sidebar")&&"dark"==n.getAttribute("data-sidebar")?l("sidebar-color-dark"):l("sidebar-color-light"),document.getElementsByTagName("html")[0].hasAttribute("dir")&&"rtl"==document.getElementsByTagName("html")[0].getAttribute("dir")?l("layout-direction-rtl"):l("layout-direction-ltr"),a("input[name='layout']").on("change",function(){window.location.href="vertical"==a(this).val()?"index.html":"layouts-horizontal.html"}),a("input[name='layout-mode']").on("change",function(){"light"==a(this).val()?(document.body.setAttribute("data-bs-theme","light"),document.body.setAttribute("data-topbar","light"),document.body.setAttribute("data-sidebar","light"),n.hasAttribute("data-layout")&&"horizontal"==n.getAttribute("data-layout")||document.body.setAttribute("data-sidebar","light"),l("topbar-color-light"),l("sidebar-color-light")):(document.body.setAttribute("data-bs-theme","dark"),document.body.setAttribute("data-topbar","dark"),document.body.setAttribute("data-sidebar","dark"),n.hasAttribute("data-layout")&&"horizontal"==n.getAttribute("data-layout")||document.body.setAttribute("data-sidebar","dark"),l("topbar-color-dark"),l("sidebar-color-dark"))}),a("input[name='layout-direction']").on("change",function(){"ltr"==a(this).val()?(document.getElementsByTagName("html")[0].removeAttribute("dir"),document.getElementById("bootstrap-style").setAttribute("href","assets/css/bootstrap.min.css"),document.getElementById("app-style").setAttribute("href","assets/css/app.min.css")):(document.getElementById("bootstrap-style").setAttribute("href","assets/css/bootstrap-rtl.min.css"),document.getElementById("app-style").setAttribute("href","assets/css/app-rtl.min.css"),document.getElementsByTagName("html")[0].setAttribute("dir","rtl"))}),Waves.init(),a("#checkAll").on("change",function(){a(".table-check .form-check-input").prop("checked",a(this).prop("checked"))}),a(".table-check .form-check-input").change(function(){a(".table-check .form-check-input:checked").length==a(".table-check .form-check-input").length?a("#checkAll").prop("checked",!0):a("#checkAll").prop("checked",!1)})}(jQuery),feather.replace();

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

const formatter = new Intl.DateTimeFormat('es-MX', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour12: false,
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
        el: '#Search',
        data: {
            reference : ""
        },
        filters: {
            capitalize: function (value) {
                if (!value) return ''
                value = value.toString()
                return value.charAt(0).toUpperCase() + value.slice(1)
            },
            DateFormat: function (value) {
                var date = value.toString();
                date = date.replace("/Date(", "");
                date = date.replace(")/", "");
                // Create a new JavaScript Date object based on the timestamp
                // multiplied by 1000 so that the argument is in milliseconds, not seconds.
                date = new Date(parseInt(date));
                var months = ['En', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'];
                var year = date.getFullYear();
                var month = months[date.getMonth()];
                var DateS = date.getDate();
                var hour = date.getHours();
                var min = date.getMinutes();
                var sec = date.getSeconds();
                var time = DateS + ' ' + month + ' ' + year;
                return time;
            },
            HourFormat: function (value) {
                var date = value.toString();
                date = date.replace("/Date(", "");
                date = date.replace(")/", "");
                date = new Date(parseInt(date));
                var months = ['En', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'];
                var year = date.getFullYear();
                var month = months[date.getMonth()];
                var DateS = date.getDate();
                var hour = date.getHours();
                var min = date.getMinutes();
                var sec = date.getSeconds();
                var time = hour + ':' + min + ':' + sec;
                var dateSplit = date.toString().split(" ");
                return dateSplit[4];
            }
        },
        methods: {
            Search: function (event) {
                event.preventDefault();
                let limpio = this.reference.replace(/[^a-zA-Z0-9]/, '').toUpperCase();
                if (/OP\d+/.test(limpio)) {
                    let OP = limpio.replace(/OP(?=\d)/, 'OP-');
                    let url = "/Operator/tarjeton/" + OP;
                    console.log(url);
                    $(location).attr('href', url);
                } else {
                    let url = "/Partner/tarjeton/" + limpio;
                    console.log(url);
                    $(location).attr('href', url);
                }
            }
        },
        mounted: function () {
            var that = this;
            console.log("Inicio el buscador.");
        }
    })
});

$('.nav-tabs-custom li a').click(function (e) {
    e.preventDefault();
    e.stopImmediatePropagation();
    $(this).tab('show');
});