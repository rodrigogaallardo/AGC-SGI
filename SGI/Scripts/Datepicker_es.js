jQuery(function ($) {
    $.datepicker.regional['es'] = {
        closeText: 'Cerrar',
        prevText: '&#x3c;Ant',
        nextText: 'Sig&#x3e;',
        currentText: 'Hoy',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
        'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: '',
        maxDate: "-0Y",
        minDate: "-100Y",
        yearRange: "-100:-0"
    };
    $.datepicker.setDefaults($.datepicker.regional['es']);

});

var isValidDate = function (value, userFormat) {
    var

    userFormat = userFormat || 'mm/dd/yyyy', // default format

    delimiter = /[^mdy]/.exec(userFormat)[0],
    theFormat = userFormat.split(delimiter),
    theDate = value.split(delimiter),

    isDate = function (date, format) {
        var m, d, y
        for (var i = 0, len = format.length; i < len; i++) {
            if (/m/.test(format[i])) m = date[i]
            if (/d/.test(format[i])) d = date[i]
            if (/y/.test(format[i])) y = date[i]
        }
        return (
          m > 0 && m < 13 &&
          y && y.length === 4 &&
          d > 0 && d <= (new Date(y, m, 0)).getDate()
        )
    }

    return isDate(theDate, theFormat)

}
