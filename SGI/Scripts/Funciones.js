
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function Left(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else
        return String(str).substring(0, n);
}
function Right(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else {
        var iLen = String(str).length;
        return String(str).substring(iLen, iLen - n);
    }
}

function separadorDecimal() {
    return parseFloat('1.1').toLocaleString().substring(1, 2);
}

function stringToFloat(value, vSeparadorDecimal) {

    var ret = 0.00;

    while (value.indexOf("_") >= 0) {
        value = value.replace("_", "");
    }

    if (vSeparadorDecimal == null)
        vSeparadorDecimal = separadorDecimal();


    if (vSeparadorDecimal == ',') {
        value = value.replace(".", "");
        value = value.replace(",", ".");
    }
    else {
        value = value.replace(",", "");
    }

    if (isNumber(value))
        ret = parseFloat(value);

    return ret;

}
function replaceAll(text, busca, reemplaza) {

    while (text.toString().indexOf(busca) != -1)
        text = text.toString().replace(busca, reemplaza);
    return text;
}


function ValidarCuit(e) {

    var ret = true;
    var str = replaceAll(e.value, "_", "");
    
    var parts = str.split("-");

    if (parts.length != 3) {
        ret = false;
        return ret;
    }

    if (parts[1].length == 7) {
        parts[1] = "0" + parts[1];
    }

    var cuitAuxiliar = parts.join("");

    if (cuitAuxiliar.length < 10) {
        ret = false;
        return ret;
    }

    if (cuitAuxiliar == "00000000000") {
        e.IsValid = false;
        return;
    }

    var sum = 0;
    var dv = 0;
    var i = 0;
    var factor = 2;
    var caracter = "";
    var Modulo11 = 0;
    var Verificador = 0;
    var CodVer = 0;

    for (i = 9; i >= 0; i--) {
        caracter = cuitAuxiliar.charAt(i);
        if (factor > 7) {
            factor = 2;
        }
        sum += parseInt(caracter) * factor;
        factor++;

    }

    dv = sum / 11;
    Modulo11 = sum - (11 * parseInt(dv));
    Verificador = 11 - Modulo11;
    if (Modulo11 == 0) {
        CodVer = 0;
    }
    else if (Verificador == 10) {
        CodVer = 9;
    }
    else {
        CodVer = Verificador;
    }

    if (CodVer != parseInt(cuitAuxiliar.charAt(10))) {
        ret = false;
        return ret;
    }

    return ret;
}

function ValidarCuitValidator(e) {

    var str = replaceAll(e.Value, "_", "");

    var parts = str.split("-");

    if (parts.length != 3) {
        e.IsValid = false;
        return;
    }

    if (parts[1].length == 7) {
        parts[1] = "0" + parts[1];
    }

    var cuitAuxiliar = parts.join("");

    if (cuitAuxiliar.length < 10) {
        e.IsValid = false;
        return;
    }

    if (cuitAuxiliar == "00000000000") {
        e.IsValid = false;
        return;
    }

    var sum = 0;
    var dv = 0;
    var i = 0;
    var factor = 2;
    var caracter = "";
    var Modulo11 = 0;
    var Verificador = 0;
    var CodVer = 0;

    for (i = 9; i >= 0; i--) {
        caracter = cuitAuxiliar.charAt(i);
        if (factor > 7) {
            factor = 2;
        }
        sum += parseInt(caracter) * factor;
        factor++;

    }

    dv = sum / 11;
    Modulo11 = sum - (11 * parseInt(dv));
    Verificador = 11 - Modulo11;
    if (Modulo11 == 0) {
        CodVer = 0;
    }
    else if (Verificador == 10) {
        CodVer = 9;
    }
    else {
        CodVer = Verificador;
    }

    if (CodVer != parseInt(cuitAuxiliar.charAt(10))) {
        e.IsValid = false;
        return;
    }

    e.IsValid = true;
}
function ValidarCuitSinGuiones(e) {

    var ret = true;

    if (e.value.length != 11) {
        ret = false;
        return ret;
    }

    var parts = new Array();
    parts[0] = e.value.substring(0, 2);
    parts[1] = e.value.substring(2, 10);
    parts[2] = e.value.substring(10);

    var cuitAuxiliar = parts.join("");

    if (cuitAuxiliar.length < 10) {
        ret = false;
    }

    if (cuitAuxiliar == "00000000000") {
        ret = false;
    }

    if (ret) {
        var sum = 0;
        var dv = 0;
        var i = 0;
        var factor = 2;
        var caracter = "";
        var Modulo11 = 0;
        var Verificador = 0;
        var CodVer = 0;

        for (i = 9; i >= 0; i--) {
            caracter = cuitAuxiliar.charAt(i);
            if (factor > 7) {
                factor = 2;
            }
            sum += parseInt(caracter) * factor;
            factor++;

        }

        dv = sum / 11;
        Modulo11 = sum - (11 * parseInt(dv));
        Verificador = 11 - Modulo11;
        if (Modulo11 == 0) {
            CodVer = 0;
        }
        else if (Verificador == 10) {
            CodVer = 9;
        }
        else {
            CodVer = Verificador;
        }

        if (CodVer != parseInt(cuitAuxiliar.charAt(10))) {
            ret = false;
        }
    }

    return ret;
}
function maxlength(control, tamanio) {
    
    var texto = $(control).val();
    if (texto.length > tamanio) {
        $(control).val(texto.substring(0, tamanio));
    }
    return false;
}