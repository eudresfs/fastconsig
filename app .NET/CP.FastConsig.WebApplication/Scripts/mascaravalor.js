// pre defined options
var obj = $(this);
var is_number = /[0-9]/;

// load the pluggings settings
var prefix = 'R$ ';
var centsSeparator = ',';
var thousandsSeparator = '.';
var limit = false;
var centsLimit = 2;
var clearPrefix = false;
var allowNegative = false;

// skip everything that isn't a number
// and also skip the left zeroes
function to_numbers(str) {
    var formatted = '';
    for (var i = 0; i < (str.length); i++) {
        char = str.charAt(i);
        if (formatted.length == 0 && char == 0) char = false;

        if (char && char.match(is_number)) {
            if (limit) {
                if (formatted.length < limit) formatted = formatted + char;
            }
            else {
                formatted = formatted + char;
            }
        }
    }

    return formatted;
}

// format to fill with zeros to complete cents chars
function fill_with_zeroes(str) {
    while (str.length < (centsLimit + 1)) str = '0' + str;
    return str;
}

// format as price
function mascaravalor(str, qtddecimais) {
    var valor = str.toString();
    if (valor.indexOf('.') > 0) {
        var inteiro = valor.substr(0, valor.indexOf('.'));
        var decimais = valor.substr(valor.indexOf('.') + 1, qtddecimais);
        valor = inteiro + centsSeparator + decimais;
    }
    // formatting settings
    var formatted = fill_with_zeroes(to_numbers(valor));
    var thousandsFormatted = '';
    var thousandsCount = 0;

    // split integer from cents
    var centsVal = formatted.substr(formatted.length - qtddecimais, qtddecimais);
    var integerVal = formatted.substr(0, formatted.length - qtddecimais);

    // apply cents pontuation
    formatted = integerVal + centsSeparator + centsVal;

    // apply thousands pontuation
    if (thousandsSeparator) {
        for (var j = integerVal.length; j > 0; j--) {
            char = integerVal.substr(j - 1, 1);
            thousandsCount++;
            if (thousandsCount % 3 == 0) char = thousandsSeparator + char;
            thousandsFormatted = char + thousandsFormatted;
        }
        if (thousandsFormatted.substr(0, 1) == thousandsSeparator) thousandsFormatted = thousandsFormatted.substring(1, thousandsFormatted.length);
        formatted = thousandsFormatted + centsSeparator + centsVal;
    }

    // if the string contains a dash, it is negative - add it to the begining (except for zero)
    if (allowNegative && str.indexOf('-') != -1 && (integerVal != 0 || centsVal != 0)) formatted = '-' + formatted;

    // apply the prefix
    if (prefix) formatted = prefix + formatted;

    return formatted;
}